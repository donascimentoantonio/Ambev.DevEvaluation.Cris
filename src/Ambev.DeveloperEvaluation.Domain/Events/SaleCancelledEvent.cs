using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Domain event triggered after a sale is cancelled.
/// </summary>
public class SaleCancelledEvent : IDomainEvent
{
    public Sale Sale { get; }
    public SaleCancelledEvent(Sale sale)
    {
        Sale = sale;
    }
}
