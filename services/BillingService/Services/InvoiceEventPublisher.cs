using System.Text.Json;
using BillingService.Events;
using BillingService.Models;
using Confluent.Kafka;

namespace BillingService.Services;

public interface IInvoiceEventPublisher
{
    Task PublishAsync(Invoice invoice, CancellationToken ct = default);
}

public class InvoiceEventPublisher(IConfiguration configuration, ILogger<InvoiceEventPublisher> logger) : IInvoiceEventPublisher
{
    public async Task PublishAsync(Invoice invoice, CancellationToken ct = default)
    {
        var bootstrapServers = configuration["Kafka:BootstrapServers"];
        if (string.IsNullOrWhiteSpace(bootstrapServers))
            throw new InvalidOperationException("Kafka:BootstrapServers is missing; InvoiceGenerated could not be published.");

        var evt = new InvoiceGeneratedEvent
        {
            CorrelationId = invoice.JobCardNumber,
            Data = new InvoiceGeneratedData
            {
                InvoiceId = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber ?? string.Empty,
                JobCardId = invoice.JobCardId,
                CustomerId = invoice.CustomerId,
                InvoiceTotal = invoice.TotalAmount
            }
        };
        using var producer = new ProducerBuilder<string, string>(new ProducerConfig { BootstrapServers = bootstrapServers, Acks = Acks.All, MessageTimeoutMs = 5000 }).Build();
        await producer.ProduceAsync("vsc.invoice.generated", new Message<string, string>
        {
            Key = evt.EventId.ToString(),
            Value = JsonSerializer.Serialize(evt, new JsonSerializerOptions(JsonSerializerDefaults.Web))
        }, ct);
        logger.LogInformation("Published InvoiceGenerated event {EventId} for invoice {InvoiceId}.", evt.EventId, invoice.Id);
    }
}
