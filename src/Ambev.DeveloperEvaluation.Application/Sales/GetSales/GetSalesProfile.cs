using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// AutoMapper profile for GetSales mapping
/// </summary>
public class GetSalesProfile : Profile
{
    public GetSalesProfile()
    {
        CreateMap<Sale, SaleDto>()
            .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
            .ForMember(dest => dest.DataSale, opt => opt.MapFrom(src => src.SaleDate))
            .ForMember(dest => dest.Cancelado, opt => opt.MapFrom(src => src.IsCanceled));
    }
}
