using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Validator for GetSalesRequest
/// </summary>
public class GetSalesRequestValidator : AbstractValidator<GetSalesRequest>
{
    public GetSalesRequestValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0");
        RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0");
    }
}
