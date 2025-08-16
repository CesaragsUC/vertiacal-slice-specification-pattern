namespace Application.Specifications.Product;

public sealed class ActiveProductsByCategorySpec : Specification<Domain.Product>
{
    public ActiveProductsByCategorySpec(string category)
    {
        Criteria = p => p.IsActive && p.Category.Name == category;
        ApplyOrderBy(p => p.Name);
    }
}
