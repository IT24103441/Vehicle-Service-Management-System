namespace BillingService.Models;

public class Invoice
{
    public int Id { get; set; }
    public int JobCardId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public List<ChargeLine> ChargeLines { get; set; } = [];
}
