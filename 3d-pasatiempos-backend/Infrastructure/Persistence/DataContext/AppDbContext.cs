using _3d_pasatiempos_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace _3d_pasatiempos_backend.Infrastructure.Persistence.DataContext;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customer { get; set; }
    public virtual DbSet<Expense> Expense { get; set; }
    public virtual DbSet<Material> Material { get; set; }
    public virtual DbSet<Order> Order { get; set; }
    public virtual DbSet<Payment> Payment { get; set; }
    public virtual DbSet<Printer> Printer { get; set; }
    public virtual DbSet<Production> Production { get; set; }
    public virtual DbSet<Project> Project { get; set; }
    public virtual DbSet<Quote> Quote { get; set; }
    public virtual DbSet<QuoteItem> QuoteItem { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customer_pkey");

            entity.ToTable("customer", "app");

            entity.HasIndex(e => e.Name, "idx_customer_name");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("expense_pkey");

            entity.ToTable("expense", "app");

            entity.HasIndex(e => e.OrderId, "idx_expense_order_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(12, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .HasColumnName("description");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");

            entity.HasOne(d => d.Order).WithMany(p => p.Expense)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_expense_order");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("material_pkey");

            entity.ToTable("material", "app");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.PricePerGram)
                .HasPrecision(10, 2)
                .HasColumnName("price_per_gram");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders", "app");

            entity.HasIndex(e => e.QuoteId, "idx_orders_quote_id");

            entity.HasIndex(e => e.QuoteId, "orders_quote_id_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_date");
            entity.Property(e => e.QuoteId).HasColumnName("quote_id");
            entity.Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_date");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Quote).WithOne(p => p.Order)
                .HasForeignKey<Order>(d => d.QuoteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orders_quote");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_pkey");

            entity.ToTable("payment", "app");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasPrecision(12, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Method)
                .HasMaxLength(50)
                .HasColumnName("method");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");

            entity.HasOne(d => d.Order).WithMany(p => p.Payment)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payment_order");
        });

        modelBuilder.Entity<Printer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("printer_pkey");

            entity.ToTable("printer", "app");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CostPerMinute)
                .HasPrecision(12, 2)
                .HasColumnName("cost_per_minute");
            entity.Property(e => e.FailPercentage)
                .HasPrecision(5, 2)
                .HasColumnName("fail_percentage");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PowerConsumptionKwh)
                .HasPrecision(10, 4)
                .HasColumnName("power_consumption_kwh");
            entity.Property(e => e.PowerConsumptionWh)
                .HasPrecision(10, 4)
                .HasColumnName("power_consumption_wh");
            entity.Property(e => e.UsefulLifeHours).HasColumnName("useful_life_hours");
        });

        modelBuilder.Entity<Production>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("production_pkey");

            entity.ToTable("production", "app");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EstimatedEndTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("estimated_end_time");
            entity.Property(e => e.GramsUsed)
                .HasPrecision(10, 2)
                .HasColumnName("grams_used");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.StartTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_time");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TimeUsed)
                .HasPrecision(10, 2)
                .HasColumnName("time_used");

            entity.HasOne(d => d.Order).WithMany(p => p.Production)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_production_order");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("project_pkey");

            entity.ToTable("project", "app");

            entity.HasIndex(e => e.CustomerId, "idx_project_customer_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Customer).WithMany(p => p.Project)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_project_customer");
        });

        modelBuilder.Entity<Quote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quote_pkey");

            entity.ToTable("quote", "app");

            entity.HasIndex(e => e.CustomerId, "idx_quote_customer_id");

            entity.HasIndex(e => e.ProjectId, "idx_quote_project_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.ProjectId).HasColumnName("project_id");
            entity.Property(e => e.RejectReason).HasColumnName("reject_reason");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Total)
                .HasPrecision(12, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.Customer).WithMany(p => p.Quote)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_quote_customer");

            entity.HasOne(d => d.Project).WithMany(p => p.Quote)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("fk_quote_project");
        });

        modelBuilder.Entity<QuoteItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quote_item_pkey");

            entity.ToTable("quote_item", "app");

            entity.HasIndex(e => e.QuoteId, "idx_quote_item_quote_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CalculatedPrice)
                .HasPrecision(12, 2)
                .HasColumnName("calculated_price");
            entity.Property(e => e.CostOverrunFailure)
                .HasPrecision(10, 2)
                .HasDefaultValue(0m)
                .HasColumnName("cost_overrun_failure");
            entity.Property(e => e.CostPerKwhUsed)
                .HasPrecision(10, 2)
                .HasColumnName("cost_per_kwh_used");
            entity.Property(e => e.EstimatedGrams)
                .HasPrecision(10, 2)
                .HasColumnName("estimated_grams");
            entity.Property(e => e.EstimatedHours)
                .HasPrecision(10, 2)
                .HasColumnName("estimated_hours");
            entity.Property(e => e.MachineWearCostUsed)
                .HasPrecision(10, 2)
                .HasColumnName("machine_wear_cost_used");
            entity.Property(e => e.PricePerGramUsed)
                .HasPrecision(10, 2)
                .HasColumnName("price_per_gram_used");
            entity.Property(e => e.ProductName)
                .HasMaxLength(150)
                .HasColumnName("product_name");
            entity.Property(e => e.ProfitPercentage)
                .HasPrecision(5, 2)
                .HasDefaultValue(0m)
                .HasColumnName("profit_percentage");
            entity.Property(e => e.QuoteId).HasColumnName("quote_id");

            entity.HasOne(d => d.Quote).WithMany(p => p.QuoteItem)
                .HasForeignKey(d => d.QuoteId)
                .HasConstraintName("fk_quote_item_quote");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
