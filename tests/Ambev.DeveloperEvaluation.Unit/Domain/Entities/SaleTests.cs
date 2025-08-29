using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact(DisplayName = "Should create sale with valid data")]
    public void CreateSale_WithValidData_ShouldSucceed()
    {
        // Arrange
        var sale = SaleTestData.GenerateSale();

        // Assert
        sale.SaleNumber.Should().NotBeNullOrWhiteSpace();
        sale.Consumer.Should().NotBeNullOrWhiteSpace();
        sale.Agency.Should().NotBeNullOrWhiteSpace();
        sale.Items.Should().NotBeNull();
    }

    [Fact(DisplayName = "Should add item to sale")]
    public void AddItem_ShouldAddSaleItem()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithItems(0);
        var item = SaleTestData.GenerateSaleWithItems(1).Items[0];

        // Act
        sale.AddItem(item);

        // Assert
        sale.Items.Should().Contain(item);
    }

    [Fact(DisplayName = "Should remove item from sale")]
    public void RemoveItem_ShouldRemoveSaleItem()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithItems(1);
        var item = sale.Items[0];

        // Act
        sale.RemoveItem(item);

        // Assert
        sale.Items.Should().BeEmpty();
    }

    [Fact(DisplayName = "Should not allow negative quantity")]
    public void AddItem_WithNegativeQuantity_ShouldThrow()
    {
        // Arrange
        var sale = new Sale { SaleNumber = new SaleNumber().Value };
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
        var sale = new Sale { SaleNumber = new SaleNumber().Value };
        var item = new SaleItem { Product = null, Quantity = 1, Price = 10.0m };

        // Act
        Action act = () => sale.AddItem(item);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
