using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class SaleTestData
{
    public static string GenerateProductId() => Guid.NewGuid().ToString();
    private static readonly Faker<SaleItem> itemFaker = new Faker<SaleItem>()
        .RuleFor(i => i.ProductId, _ => GenerateProductId())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(i => i.Price, f => f.Random.Decimal(1, 100));

    private static readonly Faker<Sale> saleFaker = new Faker<Sale>()
        .CustomInstantiator(f =>
        {
            var sale = new Sale
            {
                SaleNumber = new SaleNumber().Value
            };
            typeof(Sale).GetProperty("SaleDate")?.SetValue(sale, f.Date.PastOffset(1).UtcDateTime);
            sale.Consumer = f.Name.FullName();
            sale.Agency = f.Company.CompanyName();
            return sale;
        })
        .FinishWith((f, sale) =>
        {
            var items = itemFaker.Generate(f.Random.Int(1, 5));
            foreach (var item in items)
                sale.AddItem(item);
        });

    public static Sale GenerateSale()
    {
        return saleFaker.Generate();
    }

    public static Sale GenerateSaleWithItems(int itemCount)
    {
        var sale = saleFaker.Generate();
        sale.ClearItems();
        for (int i = 0; i < itemCount; i++)
            sale.AddItem(itemFaker.Generate());
        return sale;
    }
}
