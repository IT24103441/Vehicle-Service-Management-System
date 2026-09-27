namespace InventoryService.Models;

public class PartRequest
{
    public int Id { get; set; }
    public int JobCardId { get; set; }
    public int SparePartId { get; set; }
    public int RequestedQuantity { get; set; }
    public string RequestingMechanicId { get; set; } = string.Empty;
    public string RequestingMechanicName { get; set; } = string.Empty;
    public string Status { get; set; } = PartRequestStatus.Pending;
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? IssuedAt { get; set; }
    public SparePart? SparePart { get; set; }
    public PartIssue? PartIssue { get; set; }
}

public static class PartRequestStatus
{
    public const string Pending = "Pending";
    public const string Issued = "Issued";
}
