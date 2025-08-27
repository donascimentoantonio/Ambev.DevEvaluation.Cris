using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Mappings
{
    public class CreateSaleRequestProfile : Profile
    {
        public CreateSaleRequestProfile()
        {
            CreateMap<CreateSaleRequest, CreateSaleCommand>();
            CreateMap<ItemSaleRequest, CreateSaleItemDto>();
            CreateMap<ItemSaleRequest, SaleItem>();
            //CreateMap<CreateSaleResult, CreateSaleResponse>();
        }
    }
}
