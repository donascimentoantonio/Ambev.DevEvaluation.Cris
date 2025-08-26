namespace Ambev.DeveloperEvaluation.Domain.Entities.Sales;

/// <summary>
/// Represents an item in a sale transaction.
/// </summary>
public class SaleItem
{
    /// <summary>
    /// Product name or identifier.
    /// </summary>
    public string Product { get; }

    /// <summary>
    /// Quantity of the product being sold.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Price per unit of the product.
    /// </summary>
    public decimal Price { get; }

    /// <summary>
    /// Discount applied to this item.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Total value for this item (after discount).
    /// </summary>
    public decimal TotalValue => (Price * Quantity) - Discount;

    /// <summary>
    /// Creates a new SaleItem instance.
    /// </summary>
    /// <param name="product">Product name or identifier.</param>
    /// <param name="quantity">Quantity of the product (must be > 0).</param>
    /// <param name="price">Price per unit (must be >= 0).</param>
    /// <exception cref="ArgumentException">Thrown if quantity or price are invalid.</exception>
    public SaleItem(string product, int quantity, decimal price)
    {
        if (string.IsNullOrWhiteSpace(product))
            throw new ArgumentException("Product name must be provided.", nameof(product));
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.", nameof(price));

        Product = product;
        Quantity = quantity;
        Price = price;
        RecalculateValues();
    }

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
