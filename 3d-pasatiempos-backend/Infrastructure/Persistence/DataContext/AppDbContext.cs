using _3d_pasatiempos_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<Printer> Printer { get; set; }
        public DbSet<Quote> Quote { get; set; }
        public DbSet<QuoteItem> QuoteItem { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Production> Production { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Expense> Expense { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer", schema: "app");
                entity.HasKey(e => e.Id);

                // Relations
                entity.HasMany(c => c.Projects)
                      .WithOne(p => p.Customer)
                      .HasForeignKey(p => p.CustomerId);

                entity.HasMany(e => e.Quotes)
                      .WithOne(q => q.Customer)
                      .HasForeignKey(q => q.CustomerId);

                // Columns
                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(150)
                      .HasColumnName("name");

                entity.Property(e => e.Email)
                      .HasMaxLength(150)
                      .HasColumnName("email");

                entity.Property(e => e.Phone)
                      .HasMaxLength(50)
                      .HasColumnName("phone");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            #endregion

            #region Project
            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("project", schema: "app");
                entity.HasKey(e => e.Id);

                // Relations
                entity.HasOne(p => p.Customer)
                      .WithMany(c => c.Projects)
                      .HasForeignKey(p => p.CustomerId);

                entity.HasMany(p => p.Quotes)
                      .WithOne(q => q.Project)
                      .HasForeignKey(q => q.ProjectId);

                // Columns
                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.CustomerId)
                      .HasColumnName("customer_id")
                      .IsRequired();

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(150)
                      .HasColumnName("name");

                entity.Property(e => e.Description)
                      .HasColumnName("description");

                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasColumnName("status");

                entity.Property(e => e.ImageUrl)
                      .HasColumnName("image_url");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            #endregion

            #region Material
            modelBuilder.Entity<Material>(entity =>
            {
                entity.ToTable("material", schema: "app");
                entity.HasKey(e => e.Id);

                // Columns
                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasColumnName("name");

                entity.Property(e => e.PricePerGram)
                      .IsRequired()
                      .HasColumnType("decimal(10,2)")
                      .HasColumnName("price_per_gram");
            });
            #endregion

            #region Printer
            modelBuilder.Entity<Printer>(entity =>
            {
                entity.ToTable("printer", schema: "app");
                entity.HasKey(e => e.Id);

                // Columns
                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasColumnName("name");

                entity.Property(e => e.PowerConsumptionKwh)
                      .IsRequired()
                      .HasColumnType("decimal(10,4)")
                      .HasColumnName("power_consumption_kwh");

                entity.Property(e => e.CostPerMinute)
                      .IsRequired()
                      .HasColumnType("decimal(12,2)")
                      .HasColumnName("cost_per_minute");

                entity.Property(e => e.UsefulLifeHours)
                      .IsRequired()
                      .HasColumnName("useful_life_hours");

                entity.Property(e => e.PowerConsumptionWh)
                      .IsRequired()
                      .HasColumnType("decimal(10,4)")
                      .HasColumnName("power_consumption_wh");

                entity.Property(e => e.FailPercentage)
                      .IsRequired()
                      .HasColumnType("decimal(5,2)")
                      .HasColumnName("fail_percentage");
            });
            #endregion

            #region Quote
            modelBuilder.Entity<Quote>(entity =>
            {
                entity.ToTable("quote", schema: "app");
                entity.HasKey(e => e.Id);

                // Relations
                entity.HasOne(q => q.Customer)
                      .WithMany(c => c.Quotes)
                      .HasForeignKey(q => q.CustomerId);

                entity.HasOne(q => q.Project)
                      .WithMany(p => p.Quotes)
                      .HasForeignKey(q => q.ProjectId);

                entity.HasMany(q => q.Items)
                      .WithOne(i => i.Quote)
                      .HasForeignKey(i => i.QuoteId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(q => q.Order)
                      .WithOne(o => o.Quote)
                      .HasForeignKey<Order>(o => o.QuoteId);

                // Columns
                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.CustomerId)
                      .HasColumnName("customer_id")
                      .IsRequired();

                entity.Property(e => e.ProjectId)
                      .HasColumnName("project_id");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasConversion<string>()
                      .HasColumnName("status");

                entity.Property(e => e.Total)
                      .HasColumnType("decimal(12,2)")
                      .HasColumnName("total");
            });
            #endregion

            #region QuoteItem
            modelBuilder.Entity<QuoteItem>(entity =>
            {
                entity.ToTable("quote_item", schema: "app");
                entity.HasKey(e => e.Id);

                // Relations
                entity.HasOne(i => i.Quote)
                      .WithMany(q => q.Items)
                      .HasForeignKey(i => i.QuoteId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Columns
                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.QuoteId)
                      .HasColumnName("quote_id")
                      .IsRequired();

                entity.Property(e => e.ProductName)
                      .HasMaxLength(150)
                      .HasColumnName("product_name");

                entity.Property(e => e.EstimatedGrams)
                      .HasColumnType("decimal(10,2)")
                      .HasColumnName("estimated_grams");

                entity.Property(e => e.EstimatedHours)
                      .HasColumnType("decimal(10,2)")
                      .HasColumnName("estimated_hours");

                entity.Property(e => e.CalculatedPrice)
                      .HasColumnType("decimal(12,2)")
                      .HasColumnName("calculated_price");

                entity.Property(e => e.PricePerGramUsed)
                      .HasColumnType("decimal(10,2)")
                      .HasColumnName("price_per_gram_used");

                entity.Property(e => e.CostPerKwhUsed)
                      .HasColumnType("decimal(10,2)")
                      .HasColumnName("cost_per_kwh_used");

                entity.Property(e => e.MachineWearCostUsed)
                      .HasColumnType("decimal(10,2)")
                      .HasColumnName("machine_wear_cost_used");

                entity.Property(e => e.CostOverrunFailture)
                      .HasColumnType("decimal(10,2)")
                      .HasColumnName("cost_overrun_failure");

                entity.Property(e => e.ProfitPercentage)
                      .HasColumnType("decimal(5,2)")
                      .HasColumnName("profit_percentage");
            });
            #endregion

            #region Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders", schema: "app");
                entity.HasKey(e => e.Id);

                // Relations
                entity.HasOne(o => o.Quote)
                      .WithOne(q => q.Order)
                      .HasForeignKey<Order>(o => o.QuoteId);

                entity.HasOne(o => o.Production)
                      .WithOne(p => p.Order)
                      .HasForeignKey<Production>(p => p.OrderId);

                entity.HasMany(o => o.Payments)
                      .WithOne(p => p.Order)
                      .HasForeignKey(p => p.OrderId);

                entity.HasMany(o => o.Expenses)
                      .WithOne(e => e.Order)
                      .HasForeignKey(e => e.OrderId);

                // Columns
                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.QuoteId)
                      .HasColumnName("quote_id")
                      .IsRequired();

                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasColumnName("status");

                entity.Property(e => e.StartDate)
                      .HasColumnName("start_date");

                entity.Property(e => e.EndDate)
                      .HasColumnName("end_date");
            });
            #endregion

            #region Production
            modelBuilder.Entity<Production>(entity =>
            {
                entity.ToTable("production", schema: "app");
                entity.HasKey(e => e.Id);

                // Relations
                entity.HasOne(p => p.Order)
                      .WithOne(o => o.Production)
                      .HasForeignKey<Production>(p => p.OrderId);

                entity.HasOne(p => p.Printer)
                      .WithMany()
                      .HasForeignKey(p => p.PrinterId);

                entity.HasOne(p => p.Material)
                      .WithMany()
                      .HasForeignKey(p => p.MaterialId);

                // Columns
                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.OrderId)
                      .HasColumnName("order_id")
                      .IsRequired();

                entity.Property(e => e.PrinterId)
                      .HasColumnName("printer_id")
                      .IsRequired();

                entity.Property(e => e.MaterialId)
                      .HasColumnName("material_id")
                      .IsRequired();

                entity.Property(e => e.GramsUsed)
                      .HasColumnType("decimal(10,2)")
                      .HasColumnName("grams_used");

                entity.Property(e => e.EstimatedHours)
                      .HasColumnType("decimal(10,2)")
                      .HasColumnName("estimated_hours");

                entity.Property(e => e.ActualHours)
                      .HasColumnType("decimal(10,2)")
                      .HasColumnName("actual_hours");

                entity.Property(e => e.Status)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasColumnName("status");
            });
            #endregion

            #region Payment
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payment", schema: "app");
                entity.HasKey(e => e.Id);

                // Relations
                entity.HasOne(p => p.Order)
                      .WithMany(o => o.Payments)
                      .HasForeignKey(p => p.OrderId);

                // Columns
                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.OrderId)
                      .HasColumnName("order_id")
                      .IsRequired();

                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(12,2)")
                      .IsRequired()
                      .HasColumnName("amount");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Method)
                      .HasMaxLength(50)
                      .HasColumnName("method");

                entity.Property(e => e.Type)
                      .HasMaxLength(50)
                      .HasColumnName("type");
            });
            #endregion

            #region Expense
            modelBuilder.Entity<Expense>(entity =>
            {
                entity.ToTable("expense", schema: "app");
                entity.HasKey(e => e.Id);

                // Relaciones
                entity.HasOne(e => e.Order)
                      .WithMany(o => o.Expenses)
                      .HasForeignKey(e => e.OrderId);

                // Columnas
                entity.Property(e => e.Id)
                      .HasColumnName("id");

                entity.Property(e => e.Description)
                      .HasMaxLength(150)
                      .HasColumnName("description");

                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(12,2)")
                      .IsRequired()
                      .HasColumnName("amount");

                entity.Property(e => e.CreatedAt)
                      .HasColumnName("created_at")
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Type)
                      .HasMaxLength(50)
                      .HasColumnName("type");

                entity.Property(e => e.OrderId)
                      .HasColumnName("order_id");
            });
            #endregion
        }
    }
}
