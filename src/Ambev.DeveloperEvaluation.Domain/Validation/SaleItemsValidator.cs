using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleItemsValidator : AbstractValidator<SaleItem>
    {
        public SaleItemsValidator()
        {
            RuleFor(item => item.ProductId)
                .NotEmpty()
                .WithMessage("Product name is required.");

            RuleFor(item => item.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.")
                .LessThanOrEqualTo(20)
                .WithMessage("Quantity cannot exceed 20 units.");

            RuleFor(item => item.Price)
                .GreaterThan(0)
                .WithMessage("Unit price must be greater than 0.");

            RuleFor(item => item.Discount)
                .Must(ValidateDiscount)
                .WithMessage("The applied discount is invalid for the given quantity.");

            RuleFor(item => item.TotalValue)
                .Must(ValidateTotalValue)
                .WithMessage("The item total value is incorrect.");
        }

        private bool ValidateDiscount(SaleItem item, decimal desconto)
        {
            decimal descontoEsperado = item.Quantity switch
            {
                <= 3 => 0, // no discount
                <= 9 => item.Quantity * item.Price * 0.10m, // 10%
                <= 20 => item.Quantity * item.Price * 0.20m, // 20%
                _ => 0
            };

            return desconto == descontoEsperado;
        }

        private bool ValidateTotalValue(SaleItem item, decimal totalValue)
        {
            decimal calculatedDiscount = item.Quantity switch
            {
                <= 3 => 0, // no discount
                <= 9 => item.Quantity * item.Price * 0.10m, // 10%
                <= 20 => item.Quantity * item.Price * 0.20m, // 20%
                _ => 0
            };

            return totalValue == (item.Quantity * item.Price) - calculatedDiscount;
        }
    }
}
