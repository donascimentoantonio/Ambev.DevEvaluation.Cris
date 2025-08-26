namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Response object for the result of creating a sale (Application layer).
    /// </summary>
    public class CreateSaleResponse
    {
        /// <summary>
        /// Unique sale number.
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// Name of the consumer.
        /// </summary>
        public string Consumer { get; set; } = string.Empty;

        /// <summary>
        /// Total value of the sale.
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Total discounts applied to the sale.
        /// </summary>
        public decimal Discounts { get; set; }
    }
}
