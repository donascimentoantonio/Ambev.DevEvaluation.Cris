using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Domain.Events;
using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Application.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler responsible for processing the CreateSaleCommand and orchestrating the business logic for sale creation.
/// </summary>
public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IEventDispatcher _eventDispatcher;

    public CreateSaleCommandHandler(ISaleRepository saleRepository, IMapper mapper, IMediator mediator, IEventDispatcher eventDispatcher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _mediator = mediator;
        _eventDispatcher = eventDispatcher;
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


        var saleCreatedEvent = new SaleCreatedEvent(sale);
        var integrationEvent = new SaleCreatedIntegrationEvent(sale);
        await _mediator.Publish(integrationEvent, cancellationToken);
        await _eventDispatcher.DispatchAsync(saleCreatedEvent, cancellationToken);

        return _mapper.Map<CreateSaleResult>(sale);
    }
}
