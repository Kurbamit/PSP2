using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<TestModel> TestModels { get; set; }

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
            
            entity.Property(e => e.ReceiveTime)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
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
        });

        modelBuilder.Entity<FullOrder>(entity =>
        {
            entity.HasOne(fo => fo.Order)
                .WithMany(o => o.FullOrders)
                .HasForeignKey(fo => fo.OrderId);
            
            entity.HasOne(fo => fo.Item)
                .WithMany(i => i.FullOrders)
                .HasForeignKey(fo => fo.ItemId);

            entity.Property(e => e.FullOrderId)
                .IsRequired()
                .ValueGeneratedOnAdd();
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
            
            entity.HasOne(g => g.Payment)
                .WithMany(p => p.GiftCards)
                .HasForeignKey(g => g.PaymentId);
        });
    }
}