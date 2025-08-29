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
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.Page))
            .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.Size))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
            .ForMember(dest => dest.Consumer, opt => opt.MapFrom(src => src.Consumer))
            .ForMember(dest => dest.Agency, opt => opt.MapFrom(src => src.Agency));
        CreateMap<GetSalesResult, GetSalesResponse>();
        CreateMap<SaleDto, SaleItemResponse>()
            .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
            .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.DataSale))
            .ForMember(dest => dest.Canceled, opt => opt.MapFrom(src => src.Cancelado));
    }
}
