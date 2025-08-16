using Application.Domain;
using MassTransit;
using Microsoft.EntityFrameworkCore;
namespace Application.Infrastructure.Persistence.Contexts.ApplicationDbContext;
public sealed class AppDbContext : DbContext {
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {  } 
    protected override void OnModelCreating(ModelBuilder modelBuilder) 
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("write");
        modelBuilder.AddInboxStateEntity(); 
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity(); 
    } 
}