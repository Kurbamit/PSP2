using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TestModel> TestModels { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeAddress> EmployeeAddresses { get; set; }
    public DbSet<Establishment> Establishments { get; set; }
    public DbSet<EstablishmentAddress> EstablishmentAddresses { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Storage> Storages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<FullOrder> FullOrders { get; set; }
    public DbSet<FullOrderService> FullOrderServices { get; set; }
    public DbSet<FullOrderServiceTax> FullOrderServiceTaxes { get; set; }
    public DbSet<FullOrderTax> FullOrderTaxes { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<GiftCard> GiftCards { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<WorkingHours> WorkingHours { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Tax> Taxes { get; set; }
    public DbSet<ServiceTax> ServiceTaxes { get; set; }
    public DbSet<ItemTax> ItemTaxes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestModel>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Table for testing purposes"));

            entity.Property(e => e.IntId)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasComment("Integer identification");

            entity.Property(e => e.GuidId)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("gen_random_uuid()")
                .HasComment("Guid identification");

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Timestamp when the record was created");
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.Property(e => e.ReceiveTime)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.ItemId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.HasOne(i => i.Storage)
                .WithOne(s => s.Item)
                .HasForeignKey<Storage>(s => s.ItemId);
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasIndex(i => i.ItemId)
                .IsUnique();

            entity.Property(p => p.StorageId)
                .IsRequired()
                .ValueGeneratedOnAdd();
        });


        modelBuilder.Entity<Establishment>(entity =>
        {
            entity.Property(e => e.EstablishmentId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.HasOne(e => e.EstablishmentAddress)
                .WithOne(a => a.Establishment)
                .HasForeignKey<EstablishmentAddress>(a => a.EstablishmentId);

            entity.HasOne(e => e.EstablishmentAddress)
                .WithOne(a => a.Establishment)
                .HasForeignKey<EstablishmentAddress>(a => a.EstablishmentId) // EstablishmentAddress.EstablishmentId is the foreign key
                .IsRequired();

            entity.HasMany(e => e.Storages)
                .WithOne(s => s.Establishment)
                .HasForeignKey(s => s.EstablishmentId)
                .IsRequired();

            entity.HasMany(e => e.Employees)
                .WithOne(e => e.Establishment)
                .HasForeignKey(e => e.EstablishmentId)
                .IsRequired();

            entity.HasMany(e => e.Items)
                .WithOne(e => e.Establishment)
                .HasForeignKey(e => e.EstablishmentId);

            entity.HasMany(e => e.Orders)
                .WithOne(e => e.Establishment)
                .HasForeignKey(e => e.EstablishmentId);

            entity.Property(e => e.ReceiveTime)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasMany(d => d.Discounts)
                .WithOne(d => d.Establishment)
                .HasForeignKey(d => d.EstablishmentId);
        });

        modelBuilder.Entity<EstablishmentAddress>(entity =>
        {
            entity.Property(e => e.ReceiveTime)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .IsRequired();

            entity.Property(p => p.AddressId)
                .ValueGeneratedOnAdd()
                .IsRequired();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.ReceiveTime)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.EmployeeId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.HasOne(e => e.EmployeeAddress)
                .WithOne(a => a.Employee)
                .HasForeignKey<EmployeeAddress>(a => a.EmployeeId);

            entity.HasMany(e => e.Orders) // One Employee has many Orders
                .WithOne(o => o.CreatedByEmployee) // Each Order has one Employee
                .HasForeignKey(o => o.CreatedByEmployeeId);

            entity.HasMany(e => e.Items)
                .WithOne(i => i.CreatedByEmployee)
                .HasForeignKey(i => i.CreatedByEmployeeId);
        });

        modelBuilder.Entity<EmployeeAddress>(entity =>
        {
            entity.Property(p => p.AddressId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            entity.Property(e => e.ReceiveTime)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.HasOne(o => o.Reservation)
                .WithOne(r => r.Order)
                .HasForeignKey<Order>(o => o.ReservationId)
                .IsRequired(false);

            entity.Property(e => e.ReceiveTime)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Discount)
                .WithMany(d => d.Orders)
                .HasForeignKey(d => d.DiscountId);
        });

        modelBuilder.Entity<FullOrder>(entity =>
        {
            entity.HasOne(fo => fo.Order)
                .WithMany(o => o.FullOrders)
                .HasForeignKey(fo => fo.OrderId);

            entity.Property(e => e.FullOrderId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.HasOne(e => e.CreatedByEmployee)
                .WithMany(f => f.FullOrders)
                .HasForeignKey(f => f.CreatedByEmployeeId);

            entity.HasOne(d => d.Discount)
                .WithMany(d => d.Items)
                .HasForeignKey(d => d.DiscountId);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(p => p.PaymentId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId);

            entity.Property(e => e.ReceiveTime)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<GiftCard>(entity =>
        {
            entity.Property(p => p.GiftCardId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ReceiveTime)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(e => e.Code)
                .IsUnique();
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.Property(p => p.ReservationId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ReceiveTime)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.Property(p => p.ServiceId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ReceiveTime)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(n => n.EstablishmentId)
                .IsRequired();
        });

        modelBuilder.Entity<WorkingHours>(entity =>
        {
            entity.Property(p => p.WorkingHoursId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ReceiveTime)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Tax>(entity =>
        {
            entity.Property(t => t.TaxId)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(t => t.Percentage).IsRequired();

            entity.Property(t => t.Description).HasMaxLength(255);
        });

        modelBuilder.Entity<ServiceTax>()
            .HasKey(it => new { it.ServiceId, it.TaxId });

        modelBuilder.Entity<ServiceTax>()
            .HasOne<Service>(st => st.Service)
            .WithMany(s => s.ServiceTax)
            .HasForeignKey(st => st.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ServiceTax>()
            .HasOne<Tax>(st => st.Tax)
            .WithMany(t => t.ServiceTax)
            .HasForeignKey(st => st.TaxId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ItemTax>()
            .HasKey(it => new { it.ItemId, it.TaxId });

        modelBuilder.Entity<ItemTax>()
            .HasOne<Item>(it => it.Item)
            .WithMany(i => i.ItemTax)
            .HasForeignKey(it => it.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ItemTax>()
            .HasOne<Tax>(it => it.Tax)
            .WithMany(t => t.ItemTax)
            .HasForeignKey(it => it.TaxId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}