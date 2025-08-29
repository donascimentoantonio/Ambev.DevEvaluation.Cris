using Ambev.DeveloperEvaluation.Application.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for SaleCreatedEvent that only logs the event.
/// </summary>
public class SaleCreatedEventHandler : INotificationHandler<SaleCreatedIntegrationEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;
    public SaleCreatedEventHandler(ILogger<SaleCreatedEventHandler> logger)
    {
        _logger = logger;
    }
    public Task Handle(SaleCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"SaleCreatedIntegrationEvent: Sale created with SaleNumber={notification.Sale.SaleNumber}");
        return Task.CompletedTask;
    }
}
