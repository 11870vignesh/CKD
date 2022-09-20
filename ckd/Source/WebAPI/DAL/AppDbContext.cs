namespace WebAPI.DAL;

using Models.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
public class AppDbContext : DbContext
{
    public AppDbContext()
    {


    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<User>? Users { get; set; }
    public DbSet<Business>? Business { get; set; }
    public DbSet<Country>? Country { get; set; }
    public DbSet<Department>? Department { get; set; }
    public DbSet<Roles>? Roles { get; set; }
    public DbSet<Access>? Access { get; set; }
    public DbSet<BusinessCountryMapping>? BusinessCountryMapping { get; set; }
    public DbSet<Module>? Module { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Country>()
        // .HasOne(p => p.Business)
        // .WithMany(b => b.Country)
        // .HasForeignKey(p => p.BusinessId);

        modelBuilder.Entity<BusinessCountryMapping>()
        .HasOne(p => p.Business)
        .WithMany(b => b.BusinessCountryMapping)
        .HasForeignKey(p => p.BusinessId);

        modelBuilder.Entity<BusinessCountryMapping>()
        .HasOne(p => p.Country)
        .WithMany(d => d.BusinessCountryMapping)
        .HasForeignKey(p => p.CountryId);
    }


}