using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess.Models
{
    public partial class FalconTraderContext : DbContext
    {
        public FalconTraderContext()
        {
        }

        public FalconTraderContext(DbContextOptions<FalconTraderContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<PurchaseInvoice> PurchaseInvoice { get; set; }
        public virtual DbSet<PurchaseInvoiceDetail> PurchaseInvoiceDetail { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<StockIn> StockIn { get; set; }
        public virtual DbSet<StockOut> StockOut { get; set; }
        public virtual DbSet<StockProducts> StockProducts { get; set; }
        public virtual DbSet<StockReturn> StockReturn { get; set; }
        public virtual DbSet<Tax> Tax { get; set; }
        public virtual DbSet<TblDiscount> TblDiscount { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=CW-PC;Database=FalconTrader;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.Itemid);

                entity.Property(e => e.Itemid).HasColumnName("itemid");

                entity.Property(e => e.Itemdescp).HasMaxLength(50);

                entity.Property(e => e.ProductCode).HasMaxLength(50);
            });

            modelBuilder.Entity<PurchaseInvoice>(entity =>
            {
                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Invoicetotal).HasColumnName("invoicetotal");

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<PurchaseInvoiceDetail>(entity =>
            {
                entity.HasKey(e => e.PurchaseDetailId)
                    .HasName("PK_SaleInvoiceDetail");

                entity.Property(e => e.DiscountPercentage).HasColumnName("Discount_Percentage");

                entity.Property(e => e.FkPurchaseInvoiceId).HasColumnName("FK_PurchaseInvoice_Id");

                entity.Property(e => e.FkStockId).HasColumnName("Fk_StockId");

                entity.Property(e => e.Itemid).HasColumnName("itemid");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.TaxPercentage)
                    .HasColumnName("Tax_Percentage")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");

                entity.Property(e => e.Total).HasColumnName("total");

                entity.Property(e => e.Unitcost).HasColumnName("unitcost");

                entity.HasOne(d => d.FkPurchaseInvoice)
                    .WithMany(p => p.PurchaseInvoiceDetail)
                    .HasForeignKey(d => d.FkPurchaseInvoiceId)
                    .HasConstraintName("FK_PurchaseInvoiceDetail_PurchaseInvoiceDetail");

                entity.HasOne(d => d.FkStock)
                    .WithMany(p => p.PurchaseInvoiceDetail)
                    .HasForeignKey(d => d.FkStockId)
                    .HasConstraintName("FK_PurchaseInvoiceDetail_Stock");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.PurchaseInvoiceDetail)
                    .HasForeignKey(d => d.Itemid)
                    .HasConstraintName("FK_PurchaseInvoiceDetail_Products");
            });

            modelBuilder.Entity<Route>(entity =>
            {
                entity.Property(e => e.RouteId).HasColumnName("Route_Id");

                entity.Property(e => e.RouteDescp)
                    .HasColumnName("Route_Descp")
                    .HasMaxLength(50);

                entity.Property(e => e.RouteName)
                    .HasColumnName("Route_Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("status");

                entity.Property(e => e.StatusDescp)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Descp)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Symbol)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StockIn>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FkStockId).HasColumnName("Fk_Stock_Id");

                entity.Property(e => e.StockInDate)
                    .HasColumnName("StockIn_Date")
                    .HasColumnType("date");

                entity.HasOne(d => d.FkStock)
                    .WithMany(p => p.StockIn)
                    .HasForeignKey(d => d.FkStockId)
                    .HasConstraintName("FK_StockIn_Stock");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.StockIn)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_StockIn_Products");
            });

            modelBuilder.Entity<StockOut>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FkStockId).HasColumnName("Fk_Stock_Id");

                entity.Property(e => e.OutQuantity).HasColumnName("Out_Quantity");

                entity.Property(e => e.StockOutDate)
                    .HasColumnName("StockOut_Date")
                    .HasColumnType("date");

                entity.HasOne(d => d.FkStock)
                    .WithMany(p => p.StockOut)
                    .HasForeignKey(d => d.FkStockId)
                    .HasConstraintName("FK_StockOut_Stock1");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.StockOut)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_StockOut_Stock");
            });

            modelBuilder.Entity<StockProducts>(entity =>
            {
                entity.ToTable("Stock_Products");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FkItemId).HasColumnName("Fk_ItemId");

                entity.Property(e => e.FkStockId).HasColumnName("Fk_Stock_Id");

                entity.HasOne(d => d.FkItem)
                    .WithMany(p => p.StockProducts)
                    .HasForeignKey(d => d.FkItemId)
                    .HasConstraintName("FK_Stock_Products_Products");

                entity.HasOne(d => d.FkStock)
                    .WithMany(p => p.StockProducts)
                    .HasForeignKey(d => d.FkStockId)
                    .HasConstraintName("FK_Stock_Products_Stock");
            });

            modelBuilder.Entity<StockReturn>(entity =>
            {
                entity.HasKey(e => e.ReturnId);

                entity.Property(e => e.ReturnId).HasColumnName("Return_Id");

                entity.Property(e => e.FkItemId).HasColumnName("Fk_ItemId");

                entity.Property(e => e.FkPurcharseInvoiceId).HasColumnName("Fk_Purcharse_Invoice_Id");

                entity.Property(e => e.FkStockId).HasColumnName("FK_Stock_Id");

                entity.HasOne(d => d.FkItem)
                    .WithMany(p => p.StockReturn)
                    .HasForeignKey(d => d.FkItemId)
                    .HasConstraintName("FK_StockReturn_Products");

                entity.HasOne(d => d.FkPurcharseInvoice)
                    .WithMany(p => p.StockReturn)
                    .HasForeignKey(d => d.FkPurcharseInvoiceId)
                    .HasConstraintName("FK_StockReturn_PurchaseInvoice");

                entity.HasOne(d => d.FkStock)
                    .WithMany(p => p.StockReturn)
                    .HasForeignKey(d => d.FkStockId)
                    .HasConstraintName("FK_StockReturn_Stock");
            });

            modelBuilder.Entity<Tax>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnType("decimal(18, 0)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Percentage).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxDescp)
                    .HasColumnName("Tax_Descp")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblDiscount>(entity =>
            {
                entity.ToTable("tblDiscount");

                entity.Property(e => e.Descp)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Percentage).HasColumnType("decimal(18, 2)");
            });
        }
    }
}
