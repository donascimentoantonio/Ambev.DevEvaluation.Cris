using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommandProfile : Profile
{
    public CreateSaleCommandProfile()
    {
        CreateMap<CreateSaleCommand, Sale>()
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId.Value))
            .ForMember(dest => dest.Items, opt => opt.Ignore());


        CreateMap<CreateSaleItemDto, SaleItem>();
        CreateMap<Sale, CreateSaleResult>();
    }
}
