using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, bool>
{
    private readonly ISaleRepository _saleRepository;

    public UpdateSaleHandler(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<bool> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber, cancellationToken);
        if (sale == null)
            return false;

        if (request.Consumer != null)
            sale.Consumer = request.Consumer;
        if (request.Agency != null)
            sale.Agency = request.Agency;

        await _saleRepository.UpdateAsync(sale, cancellationToken);
        return true;
    }
}
