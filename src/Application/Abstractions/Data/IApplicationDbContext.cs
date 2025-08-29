
using Domain.Categories;
using Microsoft.EntityFrameworkCore;

using ProductEntity = Domain.Products.Product;

namespace Application.Abstractions.Data;


public interface IApplicationDbContext
{
    public DbSet<ProductEntity> Products { get; }
    public DbSet<Category> Categories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
