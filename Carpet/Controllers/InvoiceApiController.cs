using Microsoft.AspNetCore.Mvc;
using Carpet.Models.DTOs;
using Carpet.Models.Entities;
using Carpet.Repositories;
using Carpet.Mappers;
using CarpetEntity = Carpet.Models.Entities.Carpet;

namespace Carpet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceApiController : ControllerBase
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICarpetRepository _carpetRepository;
    private readonly ILogger<InvoiceApiController> _logger;

    public InvoiceApiController(
        IInvoiceRepository invoiceRepository,
        ICarpetRepository carpetRepository,
        ILogger<InvoiceApiController> logger)
    {
        _invoiceRepository = invoiceRepository;
        _carpetRepository = carpetRepository;
        _logger = logger;
    }

    // POST: api/InvoiceApi
    [HttpPost]
    public async Task<ActionResult<InvoiceDto>> Create([FromBody] CreateInvoiceDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
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
                // Get or create carpet
                var carpet = await _carpetRepository.GetByNameAsync(itemDto.CarpetName);
                if (carpet == null)
                {
                    carpet = new CarpetEntity
                    {
                        Name = itemDto.CarpetName,
                        Price = itemDto.UnitPrice,
                        CreatedDate = DateTime.UtcNow
                    };
                    carpet = await _carpetRepository.CreateAsync(carpet);
                }

                var totalPrice = itemDto.Quantity * itemDto.UnitPrice;
                totalAmount += totalPrice;

                invoice.Items.Add(new InvoiceItem
                {
                    Carpet = carpet,
                    CarpetId = carpet.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    TotalPrice = totalPrice
                });
            }

            invoice.TotalAmount = totalAmount;
            invoice.Tax = totalAmount * 0.09m; // 9% tax
            invoice.FinalTotal = totalAmount + invoice.Tax;

            var createdInvoice = await _invoiceRepository.CreateAsync(invoice);
            var invoiceDto = InvoiceMapper.ToDto(createdInvoice);

            return CreatedAtAction(nameof(Get), new { id = invoiceDto.Id }, invoiceDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating invoice");
            return StatusCode(500, "An error occurred while creating the invoice.");
        }
    }

    // GET: api/InvoiceApi/5
    [HttpGet("{id}")]
    public async Task<ActionResult<InvoiceDto>> Get(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        return Ok(InvoiceMapper.ToDto(invoice));
    }

    // GET: api/InvoiceApi
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetAll()
    {
        var invoices = await _invoiceRepository.GetAllAsync();
        return Ok(invoices.Select(InvoiceMapper.ToDto));
    }

    // DELETE: api/InvoiceApi/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _invoiceRepository.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}

