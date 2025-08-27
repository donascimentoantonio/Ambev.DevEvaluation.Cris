using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    public OrderId OrderId { get; private set; }

    public string Consumer { get; set; } = string.Empty;
    public string Agency { get; set; } = string.Empty;
    public List<SaleItem> Items { get; set; } = [];
}

public class CreateSaleItemDto
{
    public string Product { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
