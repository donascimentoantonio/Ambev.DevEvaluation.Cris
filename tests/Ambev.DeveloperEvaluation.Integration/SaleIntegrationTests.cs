using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Ambev.DeveloperEvaluation.TestUtils;

namespace Ambev.DeveloperEvaluation.Integration;

/// <summary>
/// Integration tests for the complete Sale creation flow, including persistence and dependencies.
/// </summary>
public class SaleIntegrationTests
{
    [Fact(DisplayName = "Should create sale and persist to database")]
    public async Task CreateSale_FullFlow_PersistsSale()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(databaseName: "SaleIntegrationTestDb")
            .Options;
        using var context = new DefaultContext(options);

        // Arrange: create command and handler with real context
        var saleRepository = new ORM.Repositories.SaleRepository(context);
        var mapper = new AutoMapper.Mapper(new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateSaleCommand, Sale>()
                .ForMember(dest => dest.Items, opt => opt.Ignore());
            cfg.CreateMap<SaleItem, SaleItem>();
            cfg.CreateMap<Sale, CreateSaleResult>();
        }));
        var mediator = NSubstitute.Substitute.For<MediatR.IMediator>();
        var eventDispatcher = NSubstitute.Substitute.For<Application.Events.IEventDispatcher>();
        var handler = new CreateSaleCommandHandler(saleRepository, mapper, mediator, eventDispatcher);

        var items = new List<SaleItem> {
            SaleItemBuilder.New()
                .WithProduct("Product X")
                .WithQuantity(2)
                .WithPrice(50m)
                .Build()
        };
        var command = CreateSaleCommandBuilder.New()
            .WithConsumer("John Doe")
            .WithAgency("Agency Y")
            .WithItems(items)
            .Build();

        // Act
        var result = await handler.Handle(command, default);

        // Assert: check result and database
        Assert.NotNull(result);
        Assert.Equal("John Doe", result.Consumer);
        var persistedSale = await context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.SaleNumber == result.SaleNumber);
        Assert.NotNull(persistedSale);
        Assert.Equal("John Doe", persistedSale.Consumer);
        Assert.Equal("Agency Y", persistedSale.Agency);
        Assert.Single(persistedSale.Items);
        Assert.Equal("Product X", persistedSale.Items.First().Product);
    }
}
