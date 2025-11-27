using Domain.Categories;
using SharedKernel;


namespace Domain.Products;

public sealed class Product : BaseEntity
{
    public string Name { get; private set; } = default!;
    public decimal Price { get; private set; }
    public bool IsActive { get; private set; }
    private Product() { }
    public Product(string name, decimal price, bool isActive, Guid categoryId, Category? category)
    {
        Name = name;
        Price = price;
        IsActive = isActive;
        Category = category;
        CategoryId = categoryId;
    }
    public void Deactivate() => IsActive = false;
    public void Rename(string name) => Name = name;
    public void Reprice(decimal price) => Price = price;

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    public void Update(string name, decimal price, bool isActive, Guid categoryId)
    {
        Name = name;
        Price = price;
        IsActive = isActive;
        CategoryId = categoryId;
    }
}