namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item in a sale transaction.
/// </summary>
public class SaleItem
{
    /// <summary>
    /// Product name or identifier.
    /// </summary>
    public string? Product { get; set; }

    /// <summary>
    /// Quantity of the product being sold.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Price per unit of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Discount applied to this item.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Total value for this item (after discount).
    /// </summary>
    public decimal TotalValue => Price * Quantity - Discount;
    
    /// <summary>
    /// Updates the quantity and recalculates discount.
    /// </summary>
    /// <param name="newQuantity">The new quantity (must be > 0).</param>
    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));
        Quantity = newQuantity;
        RecalculateValues();
    }

    /// <summary>
    /// Recalculates discount based on quantity.
    /// </summary>
    private void RecalculateValues()
    {
        Discount = Quantity switch
        {
            <= 3 => 0, // No discount
            <= 9 => Quantity * Price * 0.10m, // 10%
            <= 20 => Quantity * Price * 0.20m, // 20%
            _ => 0
        };
    }
}
