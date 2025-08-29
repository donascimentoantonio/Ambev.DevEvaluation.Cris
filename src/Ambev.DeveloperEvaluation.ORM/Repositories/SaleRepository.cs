using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

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

    public async Task<int> CountAsync(string? filter, CancellationToken cancellationToken = default)
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

    public async Task<List<Sale>> GetAllAsync(int pageNumber, int pageSize, string? filter, string? sortBy, CancellationToken cancellationToken = default)
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

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            var orderString = string.Join(",",
                sortBy.Split(',')
                    .Select(o => {
                        var parts = o.Trim().Split(' ');
                        var field = parts[0];
                        var direction = parts.Length > 1 ? parts[1] : "asc";
                        switch (field.ToLower())
                        {
                            case "consumer": field = "Consumer"; break;
                            case "agency": field = "Agency"; break;
                            case "salenumber": field = "SaleNumber"; break;
                            case "saledate": field = "SaleDate"; break;
                            default: field = "SaleDate"; direction = "desc"; break;
                        }
                        return $"{field} {direction}";
                    })
            );
            query = query.OrderBy(orderString);
        }
        else
        {
            query = query.OrderByDescending(s => s.SaleDate);
        }

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return await query.ToListAsync(cancellationToken);
    }
}
