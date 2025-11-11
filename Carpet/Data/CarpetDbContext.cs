using Microsoft.EntityFrameworkCore;
using CarpetEntity = Carpet.Models.Entities.Carpet;
using Carpet.Models.Entities;

namespace Carpet.Data;

public class CarpetDbContext : DbContext
{
    public CarpetDbContext(DbContextOptions<CarpetDbContext> options) : base(options)
    {
    }

    public DbSet<CarpetEntity> Carpets { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Carpet entity
        modelBuilder.Entity<CarpetEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreatedDate).IsRequired();
        });

        // Configure Invoice entity
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.InvoiceNo).IsRequired().HasMaxLength(50);
            entity.Property(e => e.InvoiceDate).IsRequired();
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Tax).HasColumnType("decimal(18,2)");
            entity.Property(e => e.FinalTotal).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreatedDate).IsRequired();
        });

        // Configure InvoiceItem entity
        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Invoice)
                .WithMany(i => i.Items)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Carpet)
                .WithMany(c => c.InvoiceItems)
                .HasForeignKey(e => e.CarpetId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

