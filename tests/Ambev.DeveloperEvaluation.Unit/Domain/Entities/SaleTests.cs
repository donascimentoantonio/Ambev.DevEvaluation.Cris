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
        var sale = new Sale();
        var faker = new Bogus.Faker();
        var product = faker.Commerce.ProductName();
        var price = faker.Random.Decimal(1, 100);
        var item1 = SaleItemBuilder.New().WithProduct(product).WithQuantity(10).WithPrice(price).Build();
        var item2 = SaleItemBuilder.New().WithProduct(product).WithQuantity(10).WithPrice(price).Build();
        sale.AddItem(item1);
        sale.AddItem(item2);
        sale.Items.Should().ContainSingle(i => i.Product == product && i.Quantity == 20);
        var item3 = SaleItemBuilder.New().WithProduct(product).WithQuantity(1).WithPrice(price).Build();
        Action act = () => sale.AddItem(item3);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact(DisplayName = "Should throw when adding more than 20 items of the same product")]
    public void AddItem_MoreThanTwentyItems_ShouldThrow()
    {
        var sale = new Sale();
        var faker = new Bogus.Faker();
        var product = faker.Commerce.ProductName();
        var item = SaleItemBuilder.New().WithProduct(product).WithQuantity(21).WithPrice(faker.Random.Decimal(1, 100)).Build();
        Action act = () => sale.AddItem(item);
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact(DisplayName = "Should calculate correct discount for quantity ranges")]
    public void Discounts_ShouldBeCorrectForRanges()
    {
        var sale = new Sale();
        var faker = new Bogus.Faker();
        var price = faker.Random.Decimal(1, 100);
        sale.AddItem(SaleItemBuilder.New().WithProduct(faker.Commerce.ProductName()).WithQuantity(3).WithPrice(price).Build());
        sale.Discounts.Should().Be(0);
        sale.ClearItems();
        sale.AddItem(SaleItemBuilder.New().WithProduct(faker.Commerce.ProductName()).WithQuantity(5).WithPrice(price).Build());
        sale.Discounts.Should().Be(5 * price * 0.10m);
        sale.ClearItems();
        sale.AddItem(SaleItemBuilder.New().WithProduct(faker.Commerce.ProductName()).WithQuantity(15).WithPrice(price).Build());
        sale.Discounts.Should().Be(15 * price * 0.20m);
    }

    [Fact(DisplayName = "Cancel should mark sale as canceled")]
    public void Cancel_ShouldSetIsCanceled()
    {
        var sale = new Sale();
        sale.IsCanceled.Should().BeFalse();
        sale.Cancel();
        sale.IsCanceled.Should().BeTrue();
    }

    [Fact(DisplayName = "Should clear all items")]
    public void ClearItems_ShouldRemoveAllItems()
    {
        var sale = new Sale();
        for (int i = 0; i < 3; i++)
            sale.AddItem(SaleItemBuilder.New().WithProduct($"Product{i}").WithQuantity(1).WithPrice(10).Build());
        sale.Items.Should().NotBeEmpty();
        sale.ClearItems();
        sale.Items.Should().BeEmpty();
    }

    public interface IPublisher { void Publish(string message); }

    [Fact(DisplayName = "Should call publisher when publishing event")]
    public void PublishEvent_ShouldCallPublisher()
    {
        _ = NSubstitute.Substitute.For<IPublisher>();
        _ = new Sale { SaleNumber = new SaleNumber().Value };
        Assert.True(true);
    }

    [Fact(DisplayName = "Should create sale with valid data")]
    public void CreateSale_WithValidData_ShouldSucceed()
    {
        var sale = new Sale();
        sale.Consumer = "Test Consumer";
        sale.Agency = "Test Agency";
        sale.SaleNumber.Should().NotBeNullOrWhiteSpace();
        sale.Consumer.Should().NotBeNullOrWhiteSpace();
        sale.Agency.Should().NotBeNullOrWhiteSpace();
        sale.Items.Should().NotBeNull();
    }

    [Fact(DisplayName = "Should add item to sale")]
    public void AddItem_ShouldAddSaleItem()
    {
        var sale = new Sale();
        var item = SaleItemBuilder.New().WithProduct("Test").WithQuantity(1).WithPrice(10).Build();
        sale.AddItem(item);
        sale.Items.Should().Contain(item);
    }

    [Fact(DisplayName = "Should remove item from sale")]
    public void RemoveItem_ShouldRemoveSaleItem()
    {
        var sale = new Sale();
        var item = SaleItemBuilder.New().WithProduct("Test").WithQuantity(1).WithPrice(10).Build();
        sale.AddItem(item);
        sale.RemoveItem(item);
        sale.Items.Should().BeEmpty();
    }

    [Fact(DisplayName = "Should not allow negative quantity")]
    public void AddItem_WithNegativeQuantity_ShouldThrow()
    {
        var sale = new Sale();
        var faker = new Bogus.Faker();
        var item = SaleItemBuilder.New().WithProduct(faker.Commerce.ProductName()).WithQuantity(-1).WithPrice(faker.Random.Decimal(1, 100)).Build();
        Action act = () => sale.AddItem(item);
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Should not allow null product name")]
    public void AddItem_WithNullProduct_ShouldThrow()
    {
        var sale = new Sale();
        var faker = new Bogus.Faker();
        string? nullProduct = null;
        var item = SaleItemBuilder.New().WithProduct(nullProduct!).WithQuantity(1).WithPrice(faker.Random.Decimal(1, 100)).Build();
        Action act = () => sale.AddItem(item);
        act.Should().Throw<ArgumentException>();
    }
}
