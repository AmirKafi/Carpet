using Microsoft.EntityFrameworkCore;
using Carpet.Data;
using Carpet.Models.Entities;
using CarpetEntity = Carpet.Models.Entities.Carpet;

namespace Carpet.Repositories;

public class CarpetRepository : ICarpetRepository
{
    private readonly CarpetDbContext _context;

    public CarpetRepository(CarpetDbContext context)
    {
        _context = context;
    }

    public async Task<CarpetEntity?> GetByIdAsync(int id)
    {
        return await _context.Carpets.FindAsync(id);
    }

    public async Task<IEnumerable<CarpetEntity>> GetAllAsync()
    {
        return await _context.Carpets
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<CarpetEntity>> SearchByNameAsync(string searchTerm)
    {
        return await _context.Carpets
            .Where(c => c.Name.Contains(searchTerm))
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<CarpetEntity> CreateAsync(CarpetEntity carpet)
    {
        _context.Carpets.Add(carpet);
        await _context.SaveChangesAsync();
        return carpet;
    }

    public async Task<CarpetEntity?> GetByNameAsync(string name)
    {
        return await _context.Carpets
            .FirstOrDefaultAsync(c => c.Name == name);
    }
}

