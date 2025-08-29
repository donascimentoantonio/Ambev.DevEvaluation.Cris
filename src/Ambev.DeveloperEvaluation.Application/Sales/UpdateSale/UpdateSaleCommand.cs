using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleCommand : IRequest<bool>
{
    public string SaleNumber { get; set; } = null!;
    public string? Consumer { get; set; }
    public string? Agency { get; set; }
}
