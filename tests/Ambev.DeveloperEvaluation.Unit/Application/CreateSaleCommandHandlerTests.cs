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
        var items = new List<SaleItem> {
            Domain.Entities.TestData.SaleItemBuilder.New()
                .WithProduct("Product X")
                .WithQuantity(2)
                .WithPrice(50m)
                .Build()
        };
        var command = Application.TestData.CreateSaleCommandBuilder.New()
            .WithConsumer("John Doe")
            .WithAgency("Agency Y")
            .WithItems(items)
            .Build();
        var sale = new Sale { SaleNumber = fixedSaleNumber, Consumer = command.Consumer, Agency = command.Agency };
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
