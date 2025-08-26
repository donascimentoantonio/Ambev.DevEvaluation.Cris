using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for Sale entity operations
/// </summary>
public interface ISaleRepository
{

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
    /// <param name="orderId">The unique OrderId of the sale.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The sale if found, null otherwise.</returns>
    Task<Sale?> GetByOrderIdAsync(OrderId orderId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves all sales from the repository.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A list of all sales.</returns>
    Task<List<Sale>> GetAllAsync(CancellationToken cancellationToken = default);


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
    /// <param name="orderId">The unique OrderId of the sale to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the sale was deleted, false if not found.</returns>
    Task<bool> DeleteAsync(OrderId orderId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Cancels a sale in the repository.
    /// </summary>
    /// <param name="orderId">The unique OrderId of the sale to cancel.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The canceled sale if found, null otherwise.</returns>
    Task<Sale?> CancelAsync(OrderId orderId, CancellationToken cancellationToken = default);
}
