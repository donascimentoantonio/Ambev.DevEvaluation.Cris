using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Validator for the GetSaleCommand.
/// </summary>
public class GetSaleValidator : AbstractValidator<GetSaleCommand>
{
    public GetSaleValidator()
    {
        RuleFor(v => v.SaleNumber)
            .NotEmpty().WithMessage("The sale number is required for the search.")
            .NotEqual(string.Empty).WithMessage("The sale number is required for the search.");
    }
}
