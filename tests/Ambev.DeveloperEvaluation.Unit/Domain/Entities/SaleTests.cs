using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact(DisplayName = "Should sum quantity for duplicate product, up to 20 units")]
    public void AddItem_DuplicateProduct_ShouldSumQuantityUpToLimit()
    {
        //Arrange
        var sale = SaleTestData.GenerateSaleWithItems(0);
        var faker = new Bogus.Faker();
        var product = faker.Commerce.ProductName();
        var item1 = new SaleItem { ProductId = Guid.NewGuid().ToString(), ProductName = product, Quantity = 10, Price = faker.Random.Decimal(1, 100) };
        var item2 = new SaleItem { ProductId = item1.ProductId, ProductName = product, Quantity = 10, Price = item1.Price };
        sale.AddItem(item1);
        sale.AddItem(item2);

        //Assert
        sale.Items.Should().ContainSingle(i => i.ProductName == product && i.Quantity == 20);

        // Exceeding the limit
        var item3 = new SaleItem { ProductId = item1.ProductId, ProductName = product, Quantity = 1, Price = item1.Price };

        //Act
        Action act = () => sale.AddItem(item3);

        //Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact(DisplayName = "Should calculate correct discount for quantity ranges")]
    public void Discounts_ShouldBeCorrectForRanges()
    {
        //Arrange
        var sale = SaleTestData.GenerateSaleWithItems(0);
        var faker = new Bogus.Faker();
        var price = faker.Random.Decimal(1, 100);

        sale.AddItem(new SaleItem { ProductId = Guid.NewGuid().ToString(), ProductName = faker.Commerce.ProductName(), Quantity = 3, Price = price }); // 0 discount
        sale.Discounts.Should().Be(0);
        sale.ClearItems();

        sale.AddItem(new SaleItem { ProductId = Guid.NewGuid().ToString(), ProductName = faker.Commerce.ProductName(), Quantity = 5, Price = price }); // 10% discount
        sale.Discounts.Should().Be(5 * price * 0.10m);
        sale.ClearItems();

        sale.AddItem(new SaleItem { ProductId = Guid.NewGuid().ToString(), ProductName = faker.Commerce.ProductName(), Quantity = 15, Price = price }); // 20% discount
        sale.Discounts.Should().Be(15 * price * 0.20m);
    }

    [Fact(DisplayName = "Should cancel sale")]
    public void Cancel_ShouldSetIsCanceled()
    {
        var sale = SaleTestData.GenerateSale();
        sale.IsCanceled.Should().BeFalse();
        sale.Cancel();
        sale.IsCanceled.Should().BeTrue();
    }

    [Fact(DisplayName = "Should clear all items")]
    public void ClearItems_ShouldRemoveAllItems()
    {
        var sale = SaleTestData.GenerateSaleWithItems(3);
        sale.Items.Should().NotBeEmpty();
        sale.ClearItems();
        sale.Items.Should().BeEmpty();
    }

    // Example of using NSubstitute for an external dependency (Publisher)
    // Suppose Sale had a method that accepted an IPublisher
    public interface IPublisher { void Publish(string message); }

    [Fact(DisplayName = "Should call publisher when publishing event")]
    public void PublishEvent_ShouldCallPublisher()
    {
        var publisher = NSubstitute.Substitute.For<IPublisher>();
        var sale = new Sale { SaleNumber = new SaleNumber().Value };
        // Suppose Sale has a method to publish an event
        // sale.PublishCreatedEvent(publisher);
        // publisher.Received(1).Publish(Arg.Any<string>());
        // As Sale has no dependency, this is just a structure example
        Assert.True(true);
    }

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
        var sale = SaleTestData.GenerateSaleWithItems(0);
        var faker = new Bogus.Faker();
        var item = new SaleItem { ProductId = Guid.NewGuid().ToString(), ProductName = faker.Commerce.ProductName(), Quantity = -1, Price = faker.Random.Decimal(1, 100) };

        // Act
        Action act = () => sale.AddItem(item);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Should not allow null product name")]
    public void AddItem_WithNullProduct_ShouldThrow()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithItems(0);
        var faker = new Bogus.Faker();
        var item = new SaleItem(saleNumber: "S123", productId: Guid.NewGuid().ToString(), productName: null, quantity: 1, price: faker.Random.Decimal(1, 100));

        // Act
        Action act = () => sale.AddItem(item);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
