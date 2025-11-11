namespace Carpet.Models.Entities;

public class InvoiceItem
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public int CarpetId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    
    // Navigation properties
    public Invoice Invoice { get; set; } = null!;
    public Carpet Carpet { get; set; } = null!;
}

