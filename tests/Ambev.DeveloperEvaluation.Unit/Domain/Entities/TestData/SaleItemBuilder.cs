using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides a fluent builder for creating SaleItem instances for tests.
/// Allows customization of product name, quantity, and price to generate valid or edge-case SaleItem objects.
/// </summary>
public class SaleItemBuilder
{
    private string _product = "Default Product";
    private int _quantity = 1;
    private decimal _price = 1m;

    public static SaleItemBuilder New()
    {
        return new SaleItemBuilder();
    }

    public SaleItemBuilder WithProduct(string product)
    {
        _product = product;
        return this;
    }

    public SaleItemBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public SaleItemBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public SaleItem Build()
    {
        return new SaleItem
        {
            Product = _product,
            Quantity = _quantity,
            Price = _price
        };
    }
}
