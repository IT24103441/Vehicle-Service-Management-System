using System.ComponentModel.DataAnnotations;
using BillingService.Models;
namespace BillingService.DTOs;
public class AddManualChargeDto { [Required, MaxLength(300)] public string Description { get; set; } = string.Empty; [Range(typeof(decimal), "0.0001", "999999999")] public decimal Quantity { get; set; } [Range(typeof(decimal), "0", "999999999")] public decimal UnitPrice { get; set; } }
public class InvoiceResponseDto { public int Id { get; set; } public int JobCardId { get; set; } public decimal TotalAmount { get; set; } public List<ChargeLineResponseDto> ChargeLines { get; set; } = []; }
public class ChargeLineResponseDto { public int Id { get; set; } public string ChargeType { get; set; } = string.Empty; public string Description { get; set; } = string.Empty; public decimal Quantity { get; set; } public decimal UnitPrice { get; set; } public decimal LineTotal { get; set; } }
