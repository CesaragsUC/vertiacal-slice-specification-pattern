using Application.Categories.Responses;
using Cortex.Mediator.Queries;
using ErrorOr;

namespace Application.Categories.GetById;

public sealed record GetCategoryByIdQuery(Guid Id) : IQuery<ErrorOr<CategoryResponse>>;
