using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered after a sale is created.
/// </summary>
public class SaleCreatedEvent : IDomainEvent
{
    public Sale Sale { get; }
    public SaleCreatedEvent(Sale sale)
    {
        Sale = sale;
    }
}
