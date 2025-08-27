
namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Request object for creating a new sale
/// </summary>
public class CreateSaleRequest
{
    /// <summary>
    /// The consumer who is making the purchase
    /// </summary>
    public string Consumer { get; set; } = string.Empty;

    /// <summary>
    /// The agency responsible for the sale
    /// </summary>
    public string Agency { get; set; } = string.Empty;

    /// <summary>
    /// List of items included in the sale
    /// </summary>
    public List<ItemSaleRequest> Items { get; set; } = [];
}

/// <summary>
/// Represents an item in the sale
/// </summary>
public class ItemSaleRequest
{
    /// <summary>
    /// The product name or identifier
    /// </summary>
    public string Product { get; set; } = string.Empty;

    /// <summary>
    /// The quantity of the product being sold
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The price per unit of the product
    /// </summary>
    public decimal Price { get; set; }
}
