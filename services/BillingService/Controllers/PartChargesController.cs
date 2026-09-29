using BillingService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Controllers;
[ApiController, Route("api/part-charges")]
public class PartChargesController(BillingDbContext db) : ControllerBase
{
    [HttpGet("job/{jobCardId:int}")]
    public async Task<IActionResult> GetByJob(int jobCardId, CancellationToken ct) => Ok(await db.PartCharges.AsNoTracking().Where(x => x.JobCardId == jobCardId).OrderByDescending(x => x.CreatedAt).ToListAsync(ct));
}
