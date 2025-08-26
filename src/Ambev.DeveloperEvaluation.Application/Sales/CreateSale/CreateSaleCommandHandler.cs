using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Entities.Sales;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler responsible for processing the CreateSaleCommand and orchestrating the business logic for sale creation.
/// </summary>
public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleResponse>
{
    private readonly AutoMapper.IMapper _mapper;

    public CreateSaleCommandHandler(AutoMapper.IMapper mapper)
    {
        //_saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<CreateSaleResponse> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = _mapper.Map<Sale>(command);
        sale.OrderId = new OrderId().Value;

        var saleItems = _mapper.Map<List<SaleItem>>(command.Items);
        foreach (var saleItem in saleItems)
        {
            sale.AddItem(saleItem);
        }

        //await _saleRepository.AddAsync(sale, cancellationToken);

        return _mapper.Map<CreateSaleResponse>(sale);
    }
}
