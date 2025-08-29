using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateSaleCommandHandlerTests
{
    [Fact(DisplayName = "Should create sale and return result")]
    public async Task Handle_ShouldCreateSaleAndReturnResult()
    {
        // Arrange
        var saleRepository = Substitute.For<ISaleRepository>();
        var mapper = Substitute.For<IMapper>();
        var mediator = Substitute.For<MediatR.IMediator>();
    var eventDispatcher = Substitute.For<Ambev.DeveloperEvaluation.Application.Events.IEventDispatcher>();
        var handler = new CreateSaleCommandHandler(saleRepository, mapper, mediator, eventDispatcher);
        var fixedSaleNumber = "TEST123";
        var sale = new Sale { SaleNumber = fixedSaleNumber };
        var faker = new Bogus.Faker();
        var items = new List<SaleItem> {
            new() {
                Product = faker.Commerce.ProductName(),
                Quantity = faker.Random.Int(1, 5),
                Price = faker.Random.Decimal(1, 100)
            }
        };
        var command = new CreateSaleCommand
        {
            Consumer = faker.Name.FullName(),
            Agency = faker.Company.CompanyName(),
            Items = items
        };
        sale.Consumer = command.Consumer;
        sale.Agency = command.Agency;
        saleRepository.AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(sale);
        mapper.Map<Sale>(command).Returns(sale);
        mapper.Map<List<SaleItem>>(command.Items).Returns(items);
        var expectedResult = new CreateSaleResult { SaleNumber = fixedSaleNumber, Consumer = sale.Consumer ?? string.Empty, TotalValue = sale.TotalValue, Discounts = sale.Discounts };
        mapper.Map<CreateSaleResult>(sale).Returns(expectedResult);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Consumer, result.Consumer);
    }
}
