using Ambev.DeveloperEvaluation.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    /// <summary>
    /// Removes an item from the sale.
    /// </summary>
    /// <summary>
    /// Unique order identifier.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;
    /// <summary>
    /// Removes an item from the sale.
    /// </summary>
    public void RemoveItem(SaleItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Remove(item);

    }


    /// <summary>
    /// Date of the sale.
    /// </summary>
    public DateTime SaleDate { get; private set; }



    /// <summary>
    /// Name of the customer.
    /// </summary>
    public string? Consumer { get; set; }


    /// <summary>
    /// Name of the agency where the sale occurred.
    /// </summary>
    public string? Agency { get; set; }

    /// <summary>
    /// Internal list of items in the sale. Used for encapsulation and business logic.
    /// </summary>
    private readonly List<SaleItem> _items = new();


    /// <summary>
    /// List of items in the sale (read-only).
    /// </summary>
    public IReadOnlyList<SaleItem> Items => _items.AsReadOnly();
    /// <summary>
    /// Parameterless constructor required by Entity Framework Core.
    /// </summary>

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
    /// Adds an item to the sale, applying discount rules and business validation.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void AddItem(SaleItem item)
    {

        if (item == null)
            throw new ArgumentNullException(nameof(item));
        if (item.Quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(item.Quantity));
        if (string.IsNullOrWhiteSpace(item.ProductId))
            throw new ArgumentException("Product name cannot be null or empty.", nameof(item.ProductId));
        if (item.Quantity > 20)
            throw new InvalidOperationException("Cannot sell more than 20 identical items.");

        var existingItem = _items.FirstOrDefault(i => i.ProductId == item.ProductId);

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
}

