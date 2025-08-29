using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Profile for mapping GetSales feature requests and responses
/// </summary>
public class GetSalesProfile : Profile
{
    public GetSalesProfile()
    {
    CreateMap<GetSalesRequest, GetSalesCommand>()
        .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src._order));
        CreateMap<GetSalesResult, GetSalesResponse>();
        CreateMap<SaleDto, SaleItemResponse>()
            .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
            .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.DataSale))
            .ForMember(dest => dest.Canceled, opt => opt.MapFrom(src => src.Cancelado));
    }
}
