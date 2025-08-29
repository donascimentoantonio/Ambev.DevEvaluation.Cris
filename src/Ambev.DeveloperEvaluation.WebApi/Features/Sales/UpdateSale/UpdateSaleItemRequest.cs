namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleItemRequest
{
    public Guid Id { get; set; }
    public string? ProductName { get; set; }
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }
}
