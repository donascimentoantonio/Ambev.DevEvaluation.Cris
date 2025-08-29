using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Integration event for MediatR pipeline, triggered after a sale is cancelled.
/// </summary>
public class SaleCancelledIntegrationEvent : INotification
{
    public Sale Sale { get; }
    public SaleCancelledIntegrationEvent(Sale sale)
    {
        Sale = sale;
    }
}
