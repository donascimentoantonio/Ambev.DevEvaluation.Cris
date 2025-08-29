using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core
/// </summary>

public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    // Método privado reutilizável para buscar Sale por SaleNumber
    private async Task<Sale?> GetSaleBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);
    }

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<bool> DeleteAsync(SaleNumber saleNumber, CancellationToken cancellationToken = default)
    {
        var sale = await GetSaleBySaleNumberAsync(saleNumber.Value, cancellationToken);
        if (sale == null)
            return false;
        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<int> CountAsync(string? filter, string? saleNumber = null, string? consumer = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Sales.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(s =>
                (s.Consumer != null && EF.Functions.ILike(s.Consumer, "%" + filter + "%")) ||
                (s.Agency != null && EF.Functions.ILike(s.Agency, "%" + filter + "%")) ||
                (s.SaleNumber != null && EF.Functions.ILike(s.SaleNumber, "%" + filter + "%"))
            );
        }
        if (!string.IsNullOrWhiteSpace(saleNumber))
            query = query.Where(s => s.SaleNumber == saleNumber);
        if (!string.IsNullOrWhiteSpace(consumer))
            query = query.Where(s => s.Consumer != null && EF.Functions.ILike(s.Consumer, "%" + consumer + "%"));
        return await query.CountAsync(cancellationToken);
    }

    public async Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return await GetSaleBySaleNumberAsync(saleNumber, cancellationToken);
    }

    public async Task<Sale?> CancelAsync(SaleNumber SaleNumber, CancellationToken cancellationToken = default)
    {
        var sale = await GetSaleBySaleNumberAsync(SaleNumber.Value, cancellationToken);
        if (sale == null)
            return null;
        sale.Cancel();
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<List<Sale>> GetAllAsync(int pageNumber, int pageSize, string? filter, string? sortBy, string? saleNumber = null, string? consumer = null, CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var query = _context.Sales
            .Include(v => v.Items)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(s =>
                (s.Consumer != null && EF.Functions.ILike(s.Consumer, "%" + filter + "%")) ||
                (s.Agency != null && EF.Functions.ILike(s.Agency, "%" + filter + "%")) ||
                (s.SaleNumber != null && EF.Functions.ILike(s.SaleNumber, "%" + filter + "%"))
            );
        }
        if (!string.IsNullOrWhiteSpace(saleNumber))
            query = query.Where(s => s.SaleNumber == saleNumber);
        if (!string.IsNullOrWhiteSpace(consumer))
            query = query.Where(s => s.Consumer != null && EF.Functions.ILike(s.Consumer, "%" + consumer + "%"));

        query = sortBy?.ToLower() switch
        {
            "consumer" => query.OrderBy(s => s.Consumer),
            "consumer_desc" => query.OrderByDescending(s => s.Consumer),
            "agency" => query.OrderBy(s => s.Agency),
            "agency_desc" => query.OrderByDescending(s => s.Agency),
            "salenumber" => query.OrderBy(s => s.SaleNumber),
            "salenumber_desc" => query.OrderByDescending(s => s.SaleNumber),
            "saledate" => query.OrderBy(s => s.SaleDate),
            "saledate_desc" => query.OrderByDescending(s => s.SaleDate),
            _ => query.OrderByDescending(s => s.SaleDate)
        };

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return await query.ToListAsync(cancellationToken);
    }
}
