using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the <see cref="Sale"/> entity validation logic.
/// </summary>
public class SaleValidationTests
{
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
        const string? nullProduct = null;
        var item = SaleItemBuilder.New().WithProduct(nullProduct!).WithQuantity(1).WithPrice(faker.Random.Decimal(1, 100)).Build();
        Action act = () => sale.AddItem(item);
        act.Should().Throw<ArgumentException>();
    }
}
