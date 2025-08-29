using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Collections.Generic;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public class CreateSaleCommandBuilder
{
    private string _consumer = new Faker().Name.FullName();
    private string _agency = new Faker().Company.CompanyName();
    private List<SaleItem> _items = new List<SaleItem> { 
        Domain.Entities.TestData.SaleItemBuilder.New().Build() 
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
