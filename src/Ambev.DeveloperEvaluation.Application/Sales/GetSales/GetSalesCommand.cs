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
    public string? Order { get; set; }

    public GetSalesCommand(
        int pageNumber = 1,
        int pageSize = 10,
        string? filter = null,
        string? order = null)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        Filter = filter;
        Order = order;
    }
}
