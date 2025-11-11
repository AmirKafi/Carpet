using Carpet.Models.Entities;

namespace Carpet.Repositories;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(int id);
    Task<IEnumerable<Invoice>> GetAllAsync();
    Task<Invoice> CreateAsync(Invoice invoice);
    Task<bool> DeleteAsync(int id);
}

