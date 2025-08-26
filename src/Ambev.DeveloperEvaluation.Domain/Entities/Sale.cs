using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sales transaction in the system.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Unique order identifier.
    /// </summary>
    public string OrderId { get; set;  }

    /// <summary>
    /// Date of the sale.
    /// </summary>
    public DateTime SaleDate { get; }

    /// <summary>
    /// Name of the customer.
    /// </summary>
    public string Consumer { get; }

    /// <summary>
    /// Name of the agency where the sale occurred.
    /// </summary>
    public string Agency { get; }

    /// <summary>
    /// Internal list of items in the sale. Used for encapsulation and business logic.
    /// </summary>
    private readonly List<SaleItem> _items = [];

    /// <summary>
    /// List of items in the sale (read-only).
    /// </summary>
    public IReadOnlyList<SaleItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Total value of the sale.
    /// </summary>
    public decimal TotalValue => _items.Sum(item => item.TotalValue);

    /// <summary>
    /// Total discounts applied to the sale.
    /// </summary>
    public decimal Discounts => CalculateTotalDiscount();
    /// <summary>
    /// Calculates the total discount for the sale based on all items and discount rules.
    /// </summary>
    /// <returns>Total discount value.</returns>
    private decimal CalculateTotalDiscount()
    {
        decimal totalDiscount = 0;
        foreach (var item in _items)
        {
            totalDiscount += CalculateDiscountForItem(item);
        }
        return totalDiscount;
    }

    /// <summary>
    /// Calculates the discount for a single item based on quantity and price.
    /// </summary>
    /// <param name="item">The sale item.</param>
    /// <returns>Discount value for the item.</returns>
    private decimal CalculateDiscountForItem(SaleItem item)
    {
        if (item.Quantity <= 3)
            return 0;
        if (item.Quantity <= 9)
            return item.Quantity * item.Price * 0.10m;
        if (item.Quantity <= 20)
            return item.Quantity * item.Price * 0.20m;
        return 0;
    }

    /// <summary>
    /// Indicates if the sale has been canceled.
    /// </summary>
    public bool IsCanceled { get; private set; }

    /// <summary>
    /// Creates a new Sale instance.
    /// </summary>
    /// <param name="saleNumber">Unique sale number.</param>
    /// <param name="consumer">Name of the customer.</param>
    /// <param name="agency">Name of the agency where the sale occurred.</param>
    /// <param name="saleDate">Date of the sale (optional, defaults to now).</param>
    /// <exception cref="ArgumentException">Thrown if required fields are missing.</exception>
    public Sale(OrderId orderId, string consumer, string agency, DateTime? saleDate = null)
    {
        if (orderId.Equals(default))
            throw new ArgumentException("OrderId must be provided.", nameof(orderId));
        if (string.IsNullOrWhiteSpace(consumer))
            throw new ArgumentException("Consumer must be provided.", nameof(consumer));
        if (string.IsNullOrWhiteSpace(agency))
            throw new ArgumentException("Agency must be provided.", nameof(agency));

        OrderId = orderId.Value;
        Consumer = consumer;
        Agency = agency;
        SaleDate = saleDate ?? DateTime.UtcNow;
        IsCanceled = false;
    }

    /// <summary>
    /// Adds an item to the sale, applying discount rules and business validation.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void AddItem(SaleItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        if (item.Quantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 identical items.");

        var existingItem = _items.FirstOrDefault(i => i.Product == item.Product);

        if (existingItem != null)
        {
            int newQuantity = existingItem.Quantity + item.Quantity;
            if (newQuantity > 20)
                throw new InvalidOperationException("The sum of items cannot exceed 20 units.");

            existingItem.UpdateQuantity(newQuantity);
        }
        else
        {
            _items.Add(item);
        }

        // Ensure discount calculation is always up to date after adding an item
        CalculateTotalDiscount();
    }

    /// <summary>
    /// Removes all items from the sale.
    /// </summary>
    public void ClearItems() => _items.Clear();

    /// <summary>
    /// Cancels the sale.
    /// </summary>
    public void Cancel() => IsCanceled = true;

    /// <summary>
    /// Reverses the cancelation of the sale.
    /// </summary>
    public void Reactivate() => IsCanceled = false;
}

