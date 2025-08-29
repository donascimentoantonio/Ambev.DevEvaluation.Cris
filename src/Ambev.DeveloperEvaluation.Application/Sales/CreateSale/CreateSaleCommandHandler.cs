using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler responsible for processing the CreateSaleCommand and orchestrating the business logic for sale creation.
/// </summary>
public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public CreateSaleCommandHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var sale = _mapper.Map<Sale>(command);
        sale.SaleNumber = new SaleNumber().Value;

        var saleItems = _mapper.Map<List<SaleItem>>(command.Items);
        foreach (var saleItem in saleItems)
        {
            sale.AddItem(saleItem);
        }

        await _saleRepository.AddAsync(sale, cancellationToken);

        return _mapper.Map<CreateSaleResult>(sale);
    }
}
