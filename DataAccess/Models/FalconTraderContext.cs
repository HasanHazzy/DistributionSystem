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

        public virtual DbSet<LoadInvoice> LoadInvoice { get; set; }
        public virtual DbSet<LoadInvoiceDetail> LoadInvoiceDetail { get; set; }
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
                optionsBuilder.UseSqlServer("Data Source=bankr-analyze.database.windows.net;Initial Catalog=Bankr;User ID=usr_bank_analyze;Password=ddS9JaHrhYG0c0x;");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<LoadInvoice>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeliveryMan)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DiscountFoc).HasColumnName("Discount_Foc");

                entity.Property(e => e.DiscountHth).HasColumnName("Discount_HTH");

                entity.Property(e => e.DiscountRegular).HasColumnName("Discount_Regular");

                entity.Property(e => e.FkRouteId).HasColumnName("Fk_RouteId");

                entity.Property(e => e.FkTaxId).HasColumnName("Fk_Tax_Id");

                entity.Property(e => e.InvoiceTotal).HasColumnName("Invoice_Total");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.VehicleName)
                    .HasColumnName("Vehicle_Name")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleNo)
                    .HasColumnName("Vehicle_No")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkRoute)
                    .WithMany(p => p.LoadInvoice)
                    .HasForeignKey(d => d.FkRouteId)
                    .HasConstraintName("FK_LoadInvoice_Route");

                entity.HasOne(d => d.FkTax)
                    .WithMany(p => p.LoadInvoice)
                    .HasForeignKey(d => d.FkTaxId)
                    .HasConstraintName("FK_LoadInvoice_LoadInvoice");
            });

            modelBuilder.Entity<LoadInvoiceDetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.FkItemId).HasColumnName("FK_ItemId");

                entity.Property(e => e.FkLoadInvoiceId).HasColumnName("FK_LoadInvoiceId");

                entity.Property(e => e.FkStockId).HasColumnName("Fk_StockId");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Total).HasColumnName("total");

                entity.Property(e => e.Unitcost).HasColumnName("unitcost");

                entity.HasOne(d => d.FkItem)
                    .WithMany(p => p.LoadInvoiceDetail)
                    .HasForeignKey(d => d.FkItemId)
                    .HasConstraintName("FK_LoadInvoiceDetail_Products");

                entity.HasOne(d => d.FkLoadInvoice)
                    .WithMany(p => p.LoadInvoiceDetail)
                    .HasForeignKey(d => d.FkLoadInvoiceId)
                    .HasConstraintName("FK_LoadInvoiceDetail_LoadInvoice");

                entity.HasOne(d => d.FkStock)
                    .WithMany(p => p.LoadInvoiceDetail)
                    .HasForeignKey(d => d.FkStockId)
                    .HasConstraintName("FK_LoadInvoiceDetail_Stock");
            });

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

                entity.Property(e => e.DiscountAmount).HasColumnName("Discount_Amount");

                entity.Property(e => e.FkPurchaseInvoiceId).HasColumnName("FK_PurchaseInvoice_Id");

                entity.Property(e => e.FkStockId).HasColumnName("Fk_StockId");

                entity.Property(e => e.Itemid).HasColumnName("itemid");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.TaxAmount).HasColumnName("Tax_Amount");

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

                entity.Property(e => e.FkPuchaseInvoiceId).HasColumnName("Fk_PuchaseInvoice_Id");

                entity.Property(e => e.FkStockId).HasColumnName("Fk_Stock_Id");

                entity.Property(e => e.StockInDate)
                    .HasColumnName("StockIn_Date")
                    .HasColumnType("date");

                entity.HasOne(d => d.FkPuchaseInvoice)
                    .WithMany(p => p.StockIn)
                    .HasForeignKey(d => d.FkPuchaseInvoiceId)
                    .HasConstraintName("FK_StockIn_PurchaseInvoice");

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

                entity.Property(e => e.FkLoadInvoiceId).HasColumnName("FK_LoadInvoice_Id");

                entity.Property(e => e.FkStockId).HasColumnName("Fk_Stock_Id");

                entity.Property(e => e.OutQuantity).HasColumnName("Out_Quantity");

                entity.Property(e => e.StockOutDate)
                    .HasColumnName("StockOut_Date")
                    .HasColumnType("date");

                entity.HasOne(d => d.FkLoadInvoice)
                    .WithMany(p => p.StockOut)
                    .HasForeignKey(d => d.FkLoadInvoiceId)
                    .HasConstraintName("FK_StockOut_LoadInvoice");

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

                entity.Property(e => e.FkLoadInvoiceId).HasColumnName("Fk_Load_Invoice_Id");

                entity.Property(e => e.FkStockId).HasColumnName("FK_Stock_Id");

                entity.HasOne(d => d.FkItem)
                    .WithMany(p => p.StockReturn)
                    .HasForeignKey(d => d.FkItemId)
                    .HasConstraintName("FK_StockReturn_Products");

                entity.HasOne(d => d.FkLoadInvoice)
                    .WithMany(p => p.StockReturn)
                    .HasForeignKey(d => d.FkLoadInvoiceId)
                    .HasConstraintName("FK_StockReturn_LoadInvoice");

                entity.HasOne(d => d.FkStock)
                    .WithMany(p => p.StockReturn)
                    .HasForeignKey(d => d.FkStockId)
                    .HasConstraintName("FK_StockReturn_Stock");
            });

            modelBuilder.Entity<Tax>(entity =>
            {
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
