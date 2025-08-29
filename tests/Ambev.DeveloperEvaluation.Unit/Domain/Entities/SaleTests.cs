using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact(DisplayName = "Should create sale with valid data")]
    public void CreateSale_WithValidData_ShouldSucceed()
    {
        // Arrange
        var sale = new Sale
        {
            SaleNumber = new SaleNumber().Value,
            SaleDate = DateTime.UtcNow,
            Consumer = "John Doe",
            Agency = "Main Branch"
        };

        // Assert
        sale.SaleNumber.Should().NotBeNullOrWhiteSpace();
        sale.Consumer.Should().Be("John Doe");
        sale.Agency.Should().Be("Main Branch");
    }

    [Fact(DisplayName = "Should add item to sale")]
    public void AddItem_ShouldAddSaleItem()
    {
        // Arrange
        var sale = new Sale { SaleNumber = new SaleNumber().Value, SaleDate = DateTime.UtcNow };
        var item = new SaleItem { Product = "Beer", Quantity = 5, Price = 10.0m };

        // Act
        sale.AddItem(item);

        // Assert
        sale.Items.Should().ContainSingle(i => i.Product == "Beer" && i.Quantity == 5 && i.Price == 10.0m);
    }

    [Fact(DisplayName = "Should remove item from sale")]
    public void RemoveItem_ShouldRemoveSaleItem()
    {
        // Arrange
        var sale = new Sale { SaleNumber = new SaleNumber().Value, SaleDate = DateTime.UtcNow };
        var item = new SaleItem { Product = "Beer", Quantity = 5, Price = 10.0m };
        sale.AddItem(item);

        // Act
        sale.RemoveItem(item);

        // Assert
        sale.Items.Should().BeEmpty();
    }

    [Fact(DisplayName = "Should not allow negative quantity")]
    public void AddItem_WithNegativeQuantity_ShouldThrow()
    {
        // Arrange
        var sale = new Sale { SaleNumber = new SaleNumber().Value, SaleDate = DateTime.UtcNow };
        var item = new SaleItem { Product = "Beer", Quantity = -1, Price = 10.0m };

        // Act
        Action act = () => sale.AddItem(item);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Should not allow null product name")]
    public void AddItem_WithNullProduct_ShouldThrow()
    {
        // Arrange
        var sale = new Sale { SaleNumber = new SaleNumber().Value, SaleDate = DateTime.UtcNow };
        var item = new SaleItem { Product = null, Quantity = 1, Price = 10.0m };

        // Act
        Action act = () => sale.AddItem(item);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
