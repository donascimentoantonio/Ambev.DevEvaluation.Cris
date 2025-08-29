using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Integration event for MediatR pipeline, triggered after a sale is updated.
/// </summary>
public class SaleUpdatedIntegrationEvent : INotification
{
    public Sale Sale { get; }
    public SaleUpdatedIntegrationEvent(Sale sale)
    {
        Sale = sale;
    }
}
