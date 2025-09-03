using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.TestUtils;

/// <summary>
/// Provides a fluent builder for creating CreateSaleCommand instances for tests.
/// Allows customization of consumer, agency, and items to generate valid or edge-case commands for handler tests.
/// </summary>
public class CreateSaleCommandBuilder
{
    private string _consumer = new Faker().Name.FullName();
    private string _agency = new Faker().Company.CompanyName();
    private List<SaleItem> _items = new List<SaleItem> {
        SaleItemBuilder.New().Build()
    };

    public static CreateSaleCommandBuilder New()
    {
        return new CreateSaleCommandBuilder();
    }

    public CreateSaleCommandBuilder WithConsumer(string consumer)
    {
        _consumer = consumer;
        return this;
    }

    public CreateSaleCommandBuilder WithAgency(string agency)
    {
        _agency = agency;
        return this;
    }

    public CreateSaleCommandBuilder WithItems(List<SaleItem> items)
    {
        _items = items;
        return this;
    }

    public CreateSaleCommand Build()
    {
        return new CreateSaleCommand
        {
            Consumer = _consumer,
            Agency = _agency,
            Items = _items
        };
    }
}
