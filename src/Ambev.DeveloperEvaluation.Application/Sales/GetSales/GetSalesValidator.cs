using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Validator for the GetSalesCommand.
/// </summary>
public class GetSalesValidator : AbstractValidator<GetSalesCommand>
{
    public GetSalesValidator()
    {
        RuleFor(v => v.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
        RuleFor(v => v.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.");
    }
}
