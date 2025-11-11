using Microsoft.AspNetCore.Mvc;
using Carpet.Models.DTOs;
using Carpet.Models.Entities;
using Carpet.Repositories;
using Carpet.Mappers;
using CarpetEntity = Carpet.Models.Entities.Carpet;

namespace Carpet.Controllers;

public class InvoiceController : Controller
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICarpetRepository _carpetRepository;
    private readonly ILogger<InvoiceController> _logger;

    public InvoiceController(
        IInvoiceRepository invoiceRepository,
        ICarpetRepository carpetRepository,
        ILogger<InvoiceController> logger)
    {
        _invoiceRepository = invoiceRepository;
        _carpetRepository = carpetRepository;
        _logger = logger;
    }

    // GET: Invoice
    public async Task<IActionResult> Index()
    {
        var invoices = await _invoiceRepository.GetAllAsync();
        var invoiceDtos = invoices.Select(InvoiceMapper.ToDto).ToList();
        return View(invoiceDtos);
    }

    // GET: Invoice/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Invoice/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateInvoiceDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
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

            await _invoiceRepository.CreateAsync(invoice);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating invoice");
            ModelState.AddModelError("", "An error occurred while creating the invoice.");
            return View(dto);
        }
    }

    // GET: Invoice/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        var invoiceDto = InvoiceMapper.ToDto(invoice);
        return View(invoiceDto);
    }

    // GET: Invoice/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        var invoiceDto = InvoiceMapper.ToDto(invoice);
        return View(invoiceDto);
    }

    // POST: Invoice/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _invoiceRepository.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}

