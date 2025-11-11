using Microsoft.EntityFrameworkCore;
using Carpet.Data;
using Carpet.Models.Entities;

namespace Carpet.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly CarpetDbContext _context;

    public InvoiceRepository(CarpetDbContext context)
    {
        _context = context;
    }

    public async Task<Invoice?> GetByIdAsync(int id)
    {
        return await _context.Invoices
            .Include(i => i.Items)
                .ThenInclude(item => item.Carpet)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<Invoice>> GetAllAsync()
    {
        return await _context.Invoices
            .Include(i => i.Items)
                .ThenInclude(item => item.Carpet)
            .OrderByDescending(i => i.CreatedDate)
            .ToListAsync();
    }

    public async Task<Invoice> CreateAsync(Invoice invoice)
    {
        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();
        return invoice;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice == null)
            return false;

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
        return true;
    }
}

