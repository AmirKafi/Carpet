namespace Carpet.Models.Entities;

public class Invoice
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public string InvoiceNo { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal Tax { get; set; }
    public decimal FinalTotal { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
}

