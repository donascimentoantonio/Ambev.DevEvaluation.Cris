using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Integration event for MediatR pipeline, triggered after a sale is created.
/// </summary>
public class SaleCreatedIntegrationEvent : INotification
{
    public Sale Sale { get; }
    public SaleCreatedIntegrationEvent(Sale sale)
    {
        Sale = sale;
    }
}
