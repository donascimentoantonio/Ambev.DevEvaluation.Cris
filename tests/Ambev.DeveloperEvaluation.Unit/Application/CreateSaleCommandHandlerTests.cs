using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using MediatR;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateSaleCommandHandlerTests
{
    [Fact(DisplayName = "Should dispatch SaleCreatedEvent when sale is created")]
    public async Task Handle_ShouldDispatchSaleCreatedEvent()
    {
        // Arrange
    var saleRepository = Substitute.For<Ambev.DeveloperEvaluation.Domain.Repositories.ISaleRepository>();
        var mapper = Substitute.For<IMapper>();
        var mediator = Substitute.For<IMediator>();
        var eventDispatcher = Substitute.For<IEventDispatcher>();
        var handler = new CreateSaleCommandHandler(saleRepository, mapper, mediator, eventDispatcher);
        var sale = SaleTestData.GenerateSale();
        var faker = new Bogus.Faker();
        var items = new List<SaleItem> {
            new SaleItem {
                Product = faker.Commerce.ProductName(),
                Quantity = faker.Random.Int(1, 5),
                Price = faker.Random.Decimal(1, 100)
            }
        };
        var command = new CreateSaleCommand {
            Consumer = faker.Name.FullName(),
            Agency = faker.Company.CompanyName(),
            Items = items
        };
        // Setup repository and mapper mocks as needed
        saleRepository.AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        mapper.Map<Sale>(command).Returns(sale);
        mapper.Map<List<SaleItem>>(command.Items).Returns(items);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await eventDispatcher.Received(1).DispatchAsync(
            Arg.Is<Ambev.DeveloperEvaluation.Domain.Events.SaleCreatedEvent>(e => e.Sale.Id == sale.Id),
            Arg.Any<CancellationToken>());
    }
}
