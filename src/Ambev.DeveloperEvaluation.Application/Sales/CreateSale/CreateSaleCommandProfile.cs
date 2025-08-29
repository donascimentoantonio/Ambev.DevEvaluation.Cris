using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommandProfile : Profile
{
    public CreateSaleCommandProfile()
    {
        CreateMap<CreateSaleCommand, Sale>()
            .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber.Value))
            .ForMember(dest => dest.Items, opt => opt.Ignore());


        CreateMap<CreateSaleItemDto, SaleItem>();
        CreateMap<Sale, CreateSaleResult>();
    }
}
