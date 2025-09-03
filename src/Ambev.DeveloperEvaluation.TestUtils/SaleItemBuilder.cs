using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.TestUtils;

/// <summary>
/// Provides a fluent builder for creating SaleItem instances for tests.
/// Allows customization of product name, quantity, and price to generate valid or edge-case SaleItem objects.
/// </summary>
public class SaleItemBuilder
{
    private string _productName = "Default Product";
    private int _quantity = 1;
    private decimal _price = 1m;

    public static SaleItemBuilder New()
    {
        return new SaleItemBuilder();
    }

    public SaleItemBuilder WithProductName(string productName)
    {
        _productName = productName;
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
            ProductName = _productName,
            Quantity = _quantity,
            Price = _price
        };
    }
}
