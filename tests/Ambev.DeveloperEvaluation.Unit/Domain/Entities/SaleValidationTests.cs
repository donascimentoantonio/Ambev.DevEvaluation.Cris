using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleValidationTests
{
    [Fact(DisplayName = "Should not allow negative quantity")]
    public void AddItem_WithNegativeQuantity_ShouldThrow()
    {
        var sale = SaleTestData.GenerateSaleWithItems(0);
        var faker = new Bogus.Faker();
        var item = SaleItemBuilder.New().WithProduct(faker.Commerce.ProductName()).WithQuantity(-1).WithPrice(faker.Random.Decimal(1, 100)).Build();
        Action act = () => sale.AddItem(item);
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Should not allow null product name")]
    public void AddItem_WithNullProduct_ShouldThrow()
    {
        var sale = SaleTestData.GenerateSaleWithItems(0);
        var faker = new Bogus.Faker();
        string? nullProduct = null;
        var item = SaleItemBuilder.New().WithProduct(nullProduct!).WithQuantity(1).WithPrice(faker.Random.Decimal(1, 100)).Build();
        Action act = () => sale.AddItem(item);
        act.Should().Throw<ArgumentException>();
    }
}
