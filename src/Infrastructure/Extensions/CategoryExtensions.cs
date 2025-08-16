using Application.Domain;
using Application.Features.Categories.Dtos;
using Application.Features.Products.Dtos;

namespace Application.Extensions;

public static class CategoryExtensions
{
    public static CategoryDto ToDto(this Category category)
    {
        if (category == null) return null;
        return new CategoryDto(category.Id, category.Name, category.IsActive);
    }

    public static IEnumerable<CategoryDto> ToDto(this IEnumerable<Category> categories)
    {
        if (categories == null) return Enumerable.Empty<CategoryDto>();
        return categories.Select(c => c.ToDto());
    }

    public static Category ToDomain(this CategoryDto category)
    {
        if (category == null) return null;
        return new Category(category.Id, category.Name,category.IsActive);
    }

    public static IEnumerable<Category> ToDomain(this IEnumerable<CategoryDto> categories)
    {
        if (categories == null) return Enumerable.Empty<Category>();
        return categories.Select(c => c.ToDomain());
    }

    public static CategoryReadModel ToReadModel(this CategoryDto category)
    {
        if (category == null) return null;
        return new CategoryReadModel(category.Id, category.Name, category.IsActive);
    }
}
