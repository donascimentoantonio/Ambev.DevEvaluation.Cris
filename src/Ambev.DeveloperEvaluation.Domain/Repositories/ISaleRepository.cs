using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sale entity operations
/// </summary>
public interface ISaleRepository
{
    /// <summary>
    /// Gets the total count of sales matching the filter (for pagination).
    /// </summary>
    /// <param name="filter">Filter string (optional).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Total count of sales matching the filter.</returns>
    Task<int> CountAsync(string? filter, string? saleNumber = null, string? consumer = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new sale in the repository.
    /// </summary>
    /// <param name="sale">The sale to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created sale.</returns>
    Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves a sale by its unique order id.
    /// </summary>
    /// <param name="SaleNumber">The unique SaleNumber of the sale.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale if found, null otherwise.</returns>
    Task<Sale?> GetBySaleNumberAsync(string SaleNumber, CancellationToken cancellationToken = default);



    /// <summary>
    /// Retrieves all sales from the repository with pagination, filtering and sorting.
    /// </summary>
    /// <param name="pageNumber">Page number (1-based).</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="filter">Filter string (optional).</param>
    /// <param name="sortBy">Sort by property (optional).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paginated, filtered and sorted list of sales.</returns>
    Task<List<Sale>> GetAllAsync(int pageNumber, int pageSize, string? filter, string? sortBy, string? saleNumber = null, string? consumer = null, CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates an existing sale in the repository.
    /// </summary>
    /// <param name="sale">The sale to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated sale.</returns>
    Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default);


    /// <summary>
    /// Deletes a sale from the repository.
    /// </summary>
    /// <param name="SaleNumber">The unique SaleNumber of the sale to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the sale was deleted, false if not found.</returns>
    Task<bool> DeleteAsync(SaleNumber SaleNumber, CancellationToken cancellationToken = default);


    /// <summary>
    /// Cancels a sale in the repository.
    /// </summary>
    /// <param name="SaleNumber">The unique SaleNumber of the sale to cancel.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The canceled sale if found, null otherwise.</returns>
    Task<Sale?> CancelAsync(SaleNumber SaleNumber, CancellationToken cancellationToken = default);
}
