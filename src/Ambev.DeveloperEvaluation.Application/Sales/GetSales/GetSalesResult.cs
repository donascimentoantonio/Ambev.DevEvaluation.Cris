using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Result of the GetSalesCommand (paginated sales list)
/// </summary>
public class GetSalesResult
{
    public List<SaleDto> Sales { get; set; } = [];
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}

public class SaleDto
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime DataSale { get; set; }
    public string Consumer { get; set; } = string.Empty;
    public string Agency { get; set; } = string.Empty;
    public List<SaleItem> Items { get; set; } = [];
    public decimal TotalValue { get; set; }
    public bool Cancelado { get; set; }
}
