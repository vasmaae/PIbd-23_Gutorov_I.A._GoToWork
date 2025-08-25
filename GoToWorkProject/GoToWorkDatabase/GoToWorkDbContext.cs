using GoToWorkContracts.Infrastructure;
using GoToWorkDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace GoToWorkDatabase;

internal class GoToWorkDbContext : DbContext
{
    private readonly IConfigurationDatabase? _configurationDatabase;

    public GoToWorkDbContext(IConfigurationDatabase? configurationDatabase)
    {
        _configurationDatabase = configurationDatabase;
    }

    public DbSet<Detail> Details { get; set; }
    public DbSet<DetailProduct> DetailProducts { get; set; }
    public DbSet<DetailProduction> DetailProductions { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeMachine> EmployeeMachines { get; set; }
    public DbSet<EmployeeWorkshop> EmployeeWorkshops { get; set; }
    public DbSet<Machine> Machines { get; set; }
    public DbSet<Production> Productions { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Workshop> Workshops { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configurationDatabase?.ConnectionString, o => o.SetPostgresVersion(12, 2));
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<Workshop>()
            .HasIndex(w => w.Address)
            .IsUnique();
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name)
            .IsUnique();
        modelBuilder.Entity<Production>()
            .HasIndex(p => p.Name)
            .IsUnique();
        modelBuilder.Entity<Machine>()
            .HasIndex(m => m.Model)
            .IsUnique();

        modelBuilder.Entity<Machine>()
            .HasMany(m => m.Products)
            .WithOne(p => p.Machine)
            .HasForeignKey(m => m.MachineId);
        modelBuilder.Entity<Production>()
            .HasMany(p => p.Workshops)
            .WithOne(w => w.Production)
            .HasForeignKey(w => w.ProductionId);

        modelBuilder.Entity<DetailProduct>()
            .HasKey(dp => new { dp.DetailId, dp.ProductId });
        modelBuilder.Entity<DetailProduction>()
            .HasKey(dp => new { dp.DetailId, dp.ProductionId });
        modelBuilder.Entity<EmployeeMachine>()
            .HasKey(em => new { em.EmployeeId, em.MachineId });
        modelBuilder.Entity<EmployeeWorkshop>()
            .HasKey(ew => new { ew.EmployeeId, ew.WorkshopId });
    }
}