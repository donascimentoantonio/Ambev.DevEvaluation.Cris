namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleResponse
    {
        /// <summary>
        /// The sale number
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// The consumer of the sale
        /// </summary>
        public string Consumer { get; set; } = string.Empty;

        /// <summary>
        /// Total value of the sale
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Total discounts applied to the sale
        /// </summary>
        public decimal Discounts { get; set; }
    }
}
