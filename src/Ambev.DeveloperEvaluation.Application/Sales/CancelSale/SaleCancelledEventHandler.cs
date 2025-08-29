using Ambev.DeveloperEvaluation.Application.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for SaleCancelledIntegrationEvent that only logs the event.
/// </summary>
public class SaleCancelledEventHandler : INotificationHandler<SaleCancelledIntegrationEvent>
{
    private readonly ILogger<SaleCancelledEventHandler> _logger;
    public SaleCancelledEventHandler(ILogger<SaleCancelledEventHandler> logger)
    {
        _logger = logger;
    }
    public Task Handle(SaleCancelledIntegrationEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"SaleCancelledIntegrationEvent: Sale cancelled with SaleNumber={notification.Sale.SaleNumber}");
        return Task.CompletedTask;
    }
}
