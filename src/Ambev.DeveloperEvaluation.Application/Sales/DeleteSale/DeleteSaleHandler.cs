using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Ambev.DeveloperEvaluation.Application.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, bool>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMediator _mediator;

    public DeleteSaleHandler(ISaleRepository saleRepository, IMediator mediator)
    {
        _saleRepository = saleRepository;
        _mediator = mediator;
    }

    public async Task<bool> Handle(DeleteSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber, cancellationToken);
        var result = await _saleRepository.DeleteAsync(new Domain.ValueObjects.SaleNumber(request.SaleNumber), cancellationToken);

        if (result && sale != null)
        {
            await _mediator.Publish(new SaleCancelledIntegrationEvent(sale), cancellationToken);
        }
        return result;
    }
}
