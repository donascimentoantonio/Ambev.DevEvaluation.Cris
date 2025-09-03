using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// API response model for GetSales operation (paginated sales)
/// </summary>
public class GetSalesResponse
{
    public List<SaleItemResponse> Data { get; set; } = [];
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}

public class SaleItemResponse
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string Consumer { get; set; } = string.Empty;
    public string Agency { get; set; } = string.Empty;
    public List<SaleItem> Items { get; set; } = [];
    public decimal TotalValue { get; set; }
    public bool Canceled { get; set; }
}
