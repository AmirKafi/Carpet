using Carpet.Models.Entities;
using Carpet.Models.DTOs;
using CarpetEntity = Carpet.Models.Entities.Carpet;

namespace Carpet.Mappers;

public static class CarpetMapper
{
    public static CarpetDto ToDto(CarpetEntity carpet)
    {
        return new CarpetDto
        {
            Id = carpet.Id,
            Name = carpet.Name,
            Price = carpet.Price
        };
    }

    public static CarpetEntity ToEntity(CarpetDto dto)
    {
        return new CarpetEntity
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            CreatedDate = DateTime.UtcNow
        };
    }
}

