using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Bogus;

namespace Ambev.DeveloperEvaluation.TestUtils;

/// <summary>
/// Provides methods for generating test data for Sale entities using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and custom data scenarios for Sale and SaleItem.
/// </summary>
public static class SaleTestData
{
    /// <summary>
    /// Gera uma venda customizada para testes, permitindo definir consumidor e data.
    /// </summary>
    /// <param name="consumer">Nome do consumidor</param>
    /// <param name="saleDate">Data da venda</param>
    /// <param name="agency">Agência (opcional)</param>
    /// <returns>Instância de Sale</returns>
    public static Sale GenerateCustomSale(string consumer, DateTime saleDate, string? agency = null)
    {
        var sale = GenerateSale();
        sale.Consumer = consumer;
        typeof(Sale).GetProperty("SaleDate")?.SetValue(sale, saleDate);
        if (agency != null) sale.Agency = agency;
        return sale;
    }
    private static readonly Faker<SaleItem> itemFaker = new Faker<SaleItem>()
        .RuleFor(i => i.Product, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(i => i.Price, f => f.Random.Decimal(1, 100));
    private static readonly Faker<Sale> saleFaker = new Faker<Sale>()
        .CustomInstantiator(f =>
        {
            var sale = new Sale();
            sale.SaleNumber = new SaleNumber().Value;
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

    /// <summary>
    /// Generates a valid Sale entity with randomized data using Bogus.
    /// The generated sale will have:
    /// - A unique SaleNumber
    /// - Randomized SaleDate (past date)
    /// - Random Consumer and Agency names
    /// - Between 1 and 5 SaleItems, each with valid product, quantity, and price
    /// </summary>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static Sale GenerateSale()
    {
        return saleFaker.Generate();
    }

    /// <summary>
    /// Generates a valid Sale entity with a custom number of items.
    /// The generated sale will have:
    /// - A unique SaleNumber
    /// - Randomized SaleDate (past date)
    /// - Random Consumer and Agency names
    /// - Exactly <paramref name="itemCount"/> SaleItems, each with valid product, quantity, and price
    /// </summary>
    /// <param name="itemCount">The number of SaleItems to generate for the sale.</param>
    /// <returns>A valid Sale entity with the specified number of items.</returns>
    public static Sale GenerateSaleWithItems(int itemCount)
    {
        var sale = saleFaker.Generate();
        sale.ClearItems();
        for (int i = 0; i < itemCount; i++)
            sale.AddItem(itemFaker.Generate());
        return sale;
    }
}
