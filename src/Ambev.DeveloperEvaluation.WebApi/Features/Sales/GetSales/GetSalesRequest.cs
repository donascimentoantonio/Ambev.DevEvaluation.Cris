namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Request model for getting paginated, filtered, and sorted sales
/// </summary>
public record GetSalesRequest(
    int PageNumber = 1,
    int PageSize = 10,
    string? Filter = null,
    string? SortBy = null,
    string? SaleNumber = null,
    string? Consumer = null
);
