using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Command for retrieving a paginated, filterable, and sortable list of sales
/// </summary>
public record GetSalesCommand : IRequest<GetSalesResult>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Filter { get; set; }
    public string? SortBy { get; set; }
    public string? SaleNumber { get; set; }
    public string? Consumer { get; set; }

    public GetSalesCommand(
        int pageNumber = 1,
        int pageSize = 10,
        string? filter = null,
        string? sortBy = null,
        string? saleNumber = null,
        string? consumer = null)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Filter = filter;
        SortBy = sortBy;
        SaleNumber = saleNumber;
        Consumer = consumer;
    }
}
