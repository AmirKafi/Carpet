using Carpet.Models.Entities;
using CarpetEntity = Carpet.Models.Entities.Carpet;

namespace Carpet.Repositories;

public interface ICarpetRepository
{
    Task<CarpetEntity?> GetByIdAsync(int id);
    Task<IEnumerable<CarpetEntity>> GetAllAsync();
    Task<IEnumerable<CarpetEntity>> SearchByNameAsync(string searchTerm);
    Task<CarpetEntity> CreateAsync(CarpetEntity carpet);
    Task<CarpetEntity?> GetByNameAsync(string name);
}

