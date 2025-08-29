using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales;

/// <summary>
/// Validator for GetSalesRequest
/// </summary>
public class GetSalesRequestValidator : AbstractValidator<GetSalesRequest>
{
    public GetSalesRequestValidator()
    {
    RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page must be greater than 0");
    RuleFor(x => x.Size).GreaterThan(0).WithMessage("Size must be greater than 0");
    }
}
