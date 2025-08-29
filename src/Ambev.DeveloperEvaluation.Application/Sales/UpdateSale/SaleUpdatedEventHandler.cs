using Ambev.DeveloperEvaluation.Application.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for SaleUpdatedIntegrationEvent that only logs the event.
/// </summary>
public class SaleUpdatedEventHandler : INotificationHandler<SaleUpdatedIntegrationEvent>
{
    private readonly ILogger<SaleUpdatedEventHandler> _logger;
    public SaleUpdatedEventHandler(ILogger<SaleUpdatedEventHandler> logger)
    {
        _logger = logger;
    }
    public Task Handle(SaleUpdatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
    _logger.LogInformation("SaleUpdatedIntegrationEvent: Sale updated. Payload: {@Sale}", notification.Sale);
        return Task.CompletedTask;
    }
}
