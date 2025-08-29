using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
{
    public CreateSaleRequestValidator()
    {

        RuleFor(sale => sale.Consumer)
            .NotEmpty()
            .WithMessage("Customer name is required.");

        RuleFor(sale => sale.Agency)
            .NotEmpty()
            .WithMessage("Agency name is required.");
    }
}