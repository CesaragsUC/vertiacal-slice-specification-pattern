using Application.Categories.Responses;
using Cortex.Mediator.Queries;
using ErrorOr;

namespace Application.Categories.GetAll;

public sealed record GetCategoryListQuery : IQuery<ErrorOr<List<CategoryResponse>>>;