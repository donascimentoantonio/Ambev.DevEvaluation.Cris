using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered after a sale is updated.
/// </summary>
public class SaleUpdatedEvent : IDomainEvent
{
    public Sale Sale { get; }
    public SaleUpdatedEvent(Sale sale)
    {
        Sale = sale;
    }
}
