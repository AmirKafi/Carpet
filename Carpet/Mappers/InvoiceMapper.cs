using Carpet.Models.Entities;
using Carpet.Models.DTOs;
using CarpetEntity = Carpet.Models.Entities.Carpet;

namespace Carpet.Mappers;

public static class InvoiceMapper
{
    public static InvoiceDto ToDto(Invoice invoice)
    {
        return new InvoiceDto
        {
            Id = invoice.Id,
            CustomerName = invoice.CustomerName,
            InvoiceDate = invoice.InvoiceDate,
            InvoiceNo = invoice.InvoiceNo,
            TotalAmount = invoice.TotalAmount,
            Tax = invoice.Tax,
            FinalTotal = invoice.FinalTotal,
            CreatedDate = invoice.CreatedDate,
            Items = invoice.Items.Select(InvoiceItemMapper.ToDto).ToList()
        };
    }

    public static Invoice ToEntity(CreateInvoiceDto dto, List<CarpetEntity> carpets)
    {
        var invoice = new Invoice
        {
            CustomerName = dto.CustomerName,
            InvoiceDate = dto.InvoiceDate,
            InvoiceNo = dto.InvoiceNo,
            CreatedDate = DateTime.UtcNow
        };

        decimal totalAmount = 0;

        foreach (var itemDto in dto.Items)
        {
            var carpet = carpets.FirstOrDefault(c => c.Name == itemDto.CarpetName);
            if (carpet == null)
            {
                // Create new carpet if it doesn't exist
                carpet = new CarpetEntity
                {
                    Name = itemDto.CarpetName,
                    Price = itemDto.UnitPrice,
                    CreatedDate = DateTime.UtcNow
                };
            }

            var totalPrice = itemDto.Quantity * itemDto.UnitPrice;
            totalAmount += totalPrice;

            invoice.Items.Add(new InvoiceItem
            {
                Carpet = carpet,
                Quantity = itemDto.Quantity,
                UnitPrice = itemDto.UnitPrice,
                TotalPrice = totalPrice
            });
        }

        invoice.TotalAmount = totalAmount;
        invoice.Tax = totalAmount * 0.09m; // 9% tax
        invoice.FinalTotal = totalAmount + invoice.Tax;

        return invoice;
    }
}

