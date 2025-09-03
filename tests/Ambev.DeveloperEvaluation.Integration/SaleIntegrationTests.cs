using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Ambev.DeveloperEvaluation.TestUtils;
using Ambev.DeveloperEvaluation.Application.Events;
using NSubstitute;

namespace Ambev.DeveloperEvaluation.Integration;

public class SaleIntegrationTests
{
    [Fact(DisplayName = "Should return sales ordered by Consumer desc, SaleDate asc")]
    public async Task GetAllAsync_OrdersByMultipleFields()
    {

        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(databaseName: "SaleOrderTestDb")
            .Options;

        using var context = new DefaultContext(options);
        
        var sales = new List<Sale>
        {
            SaleTestData.GenerateCustomSale("Carlos", new DateTime(2024, 1, 1)),
            SaleTestData.GenerateCustomSale("Ana", new DateTime(2024, 1, 2)),
            SaleTestData.GenerateCustomSale("Bruno", new DateTime(2024, 1, 3)),
            SaleTestData.GenerateCustomSale("Carlos", new DateTime(2024, 1, 2)),
            SaleTestData.GenerateCustomSale("Ana", new DateTime(2024, 1, 1)),
        };
        
        context.Sales.AddRange(sales);
        context.SaveChanges();
        
        var saleRepository = new SaleRepository(context);
        var result = await saleRepository.GetAllAsync(
            pageNumber: 1,
            pageSize: 10,
            filter: null,
            sortBy: "Consumer desc,SaleDate asc",
            consumer: null,
            agency: null,
            cancellationToken: default
        );
        
        Assert.Equal(5, result.Count);
        Assert.Equal("Carlos", result[0].Consumer);
        Assert.Equal(new DateTime(2024, 1, 1), result[0].SaleDate);
        Assert.Equal("Carlos", result[1].Consumer);
        Assert.Equal(new DateTime(2024, 1, 2), result[1].SaleDate);
        Assert.Equal("Bruno", result[2].Consumer);
        Assert.Equal("Ana", result[3].Consumer);
        Assert.Equal(new DateTime(2024, 1, 1), result[3].SaleDate);
        Assert.Equal("Ana", result[4].Consumer);
        Assert.Equal(new DateTime(2024, 1, 2), result[4].SaleDate);
    }

    [Fact(DisplayName = "Should create sale and persist to database")]
    public async Task CreateSale_FullFlow_PersistsSale()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(databaseName: "SaleIntegrationTestDb")
            .Options;

        using var context = new DefaultContext(options);
        var saleRepository = new SaleRepository(context);

        var mapper = new AutoMapper.Mapper(new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateSaleCommand, Sale>()
                .ForMember(dest => dest.Items, opt => opt.Ignore());
            cfg.CreateMap<SaleItem, SaleItem>();
            cfg.CreateMap<Sale, CreateSaleResult>();
        }));

        var mediator = Substitute.For<MediatR.IMediator>();
        var eventDispatcher = Substitute.For<IEventDispatcher>();
        var handler = new CreateSaleCommandHandler(saleRepository, mapper, mediator, eventDispatcher);
        var items = new List<SaleItem> {
            SaleItemBuilder.New()
                .WithProductName("Product X")
                .WithQuantity(2)
                .WithPrice(50m)
                .Build()
        };

        //Act
        var command = CreateSaleCommandBuilder.New()
            .WithConsumer("John Doe")
            .WithAgency("Agency Y")
            .WithItems(items)
            .Build();

        //Assert
        var result = await handler.Handle(command, default);
        Assert.NotNull(result);
        Assert.Equal("John Doe", result.Consumer);
        var persistedSale = await context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.SaleNumber == result.SaleNumber);
        Assert.NotNull(persistedSale);
        Assert.Equal("John Doe", persistedSale.Consumer);
        Assert.Equal("Agency Y", persistedSale.Agency);
        Assert.Single(persistedSale.Items);
        Assert.Equal("Product X", persistedSale.Items.First().ProductName);
    }
}
