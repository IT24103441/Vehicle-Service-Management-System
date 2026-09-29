using System.Security.Claims;
using BillingService.DTOs;
using BillingService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillingService.Controllers;

[ApiController]
[Route("api/invoices")]
[Authorize]
public class InvoicesController(IInvoiceService invoices) : ControllerBase
{
    [HttpGet("eligible-jobs")]
    [Authorize(Roles = "Accounts,Administrator")]
    public async Task<ActionResult<IReadOnlyList<InvoiceResponseDto>>> GetEligibleJobs(CancellationToken ct) => Ok(await invoices.GetEligibleAsync(ct));

    [HttpPost("job/{jobCardId:int}/generate")]
    [Authorize(Roles = "Accounts,Administrator")]
    public async Task<ActionResult<InvoiceResponseDto>> Generate(int jobCardId, CancellationToken ct)
    {
        try { return Ok(await invoices.GenerateAsync(jobCardId, ct)); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpGet]
    [Authorize(Roles = "Accounts,Administrator")]
    public async Task<ActionResult<IReadOnlyList<InvoiceResponseDto>>> GetAll(CancellationToken ct) => Ok(await invoices.GetGeneratedAsync(null, ct));

    [HttpGet("me")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<IReadOnlyList<InvoiceResponseDto>>> GetMine(CancellationToken ct)
    {
        var customerId = GetCustomerId();
        return customerId is null ? Forbid() : Ok(await invoices.GetGeneratedAsync(customerId, ct));
    }

    [HttpGet("{invoiceId:int}")]
    public async Task<ActionResult<InvoiceResponseDto>> GetById(int invoiceId, CancellationToken ct)
    {
        var invoice = await invoices.GetByIdAsync(invoiceId, ct);
        if (invoice is null) return NotFound(new { message = "Invoice not found." });
        if (User.IsInRole("Customer") && GetCustomerId() != invoice.CustomerId) return Forbid();
        if (!User.IsInRole("Customer") && !User.IsInRole("Accounts") && !User.IsInRole("Administrator")) return Forbid();
        return Ok(invoice);
    }

    private int? GetCustomerId() => int.TryParse(User.FindFirstValue("customerId"), out var id) ? id : null;
}
