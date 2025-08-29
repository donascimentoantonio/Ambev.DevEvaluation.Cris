namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Request model for getting paginated, filtered, and sorted sales
/// </summary>
using Microsoft.AspNetCore.Mvc;

public class GetSalesRequest
{
    [FromQuery(Name = "_page")]
    public int Page { get; set; } = 1;

    [FromQuery(Name = "_size")]
    public int Size { get; set; } = 10;

    [FromQuery(Name = "filter")]
    public string? Filter { get; set; }

    [FromQuery(Name = "_order")]
    public string? Order { get; set; }
}
