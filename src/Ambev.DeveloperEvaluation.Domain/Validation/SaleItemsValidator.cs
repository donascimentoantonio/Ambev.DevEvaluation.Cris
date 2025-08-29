using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SaleItemsValidator : AbstractValidator<SaleItem>
    {
        public SaleItemsValidator()
        {
            RuleFor(item => item.Product)
                .NotEmpty()
                .WithMessage("O nome do produto é obrigatório.");

            RuleFor(item => item.Quantity)
                .GreaterThan(0)
                .WithMessage("A quantidade deve ser maior que 0.")
                .LessThanOrEqualTo(20)
                .WithMessage("A quantidade não pode exceder 20 unidades.");

            RuleFor(item => item.Price)
                .GreaterThan(0)
                .WithMessage("O preço unitário deve ser maior que 0.");

            RuleFor(item => item.Discount)
                .Must(ValidateDiscount)
                .WithMessage("O desconto aplicado é inválido para a quantidade informada.");

            RuleFor(item => item.TotalValue)
                .Must(ValidarTotalValue)
                .WithMessage("O valor total do item está incorreto.");
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

        private bool ValidarTotalValue(SaleItem item, decimal totalValue)
        {
            decimal descontoCalculado = item.Quantity switch
            {
                <= 3 => 0, // no discount
                <= 9 => item.Quantity * item.Price * 0.10m, // 10%
                <= 20 => item.Quantity * item.Price * 0.20m, // 20%
                _ => 0
            };

            return totalValue == (item.Quantity * item.Price) - descontoCalculado;
        }
    }
}
