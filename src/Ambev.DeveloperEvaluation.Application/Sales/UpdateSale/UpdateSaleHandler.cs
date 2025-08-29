using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Ambev.DeveloperEvaluation.Application.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, bool>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMediator _mediator;

    public UpdateSaleHandler(ISaleRepository saleRepository, IMediator mediator)
    {
        _saleRepository = saleRepository;
        _mediator = mediator;
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

        await _mediator.Publish(new SaleUpdatedIntegrationEvent(sale), cancellationToken);

        return true;
    }
}
