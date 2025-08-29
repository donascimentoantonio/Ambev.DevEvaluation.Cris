using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// API response model for GetSale operation
/// </summary>
public class GetSaleResponse
{
    /// <summary>
    /// The unique identifier of the sale
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The sale number
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The sale date
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// The name of the client
    /// </summary>
    public string Consumer { get; set; } = string.Empty;

    /// <summary>
    /// The agency where the sale was made
    /// </summary>
    public string Agency { get; set; } = string.Empty;

    /// <summary>
    /// List of items in the sale
    /// </summary>
    public List<SaleItem> Items { get; set; } = [];

    /// <summary>
    /// The total value of the sale
    /// </summary>
    public decimal TotalValue { get; set; }

    /// <summary>
    /// Indicates whether the sale is canceled
    /// </summary>
    public bool Canceled { get; set; }
}
