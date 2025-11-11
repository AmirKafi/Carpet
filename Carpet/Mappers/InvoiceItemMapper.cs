using Carpet.Models.Entities;
using Carpet.Models.DTOs;

namespace Carpet.Mappers;

public static class InvoiceItemMapper
{
    public static InvoiceItemDto ToDto(InvoiceItem item)
    {
        return new InvoiceItemDto
        {
            Id = item.Id,
            InvoiceId = item.InvoiceId,
            CarpetId = item.CarpetId,
            CarpetName = item.Carpet.Name,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            TotalPrice = item.TotalPrice
        };
    }
}

