using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Result of the GetSaleCommand.
/// </summary>
public class GetSaleResult
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
