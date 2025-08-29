namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;

public class UpdateSaleRequest
{
    public string SaleNumber { get; set; } = null!;
    public string? Consumer { get; set; }
    public string? Agency { get; set; }
}
