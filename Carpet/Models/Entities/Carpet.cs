namespace Carpet.Models.Entities;

public class Carpet
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
}

