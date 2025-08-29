using Application.Categories.Responses;
using Cortex.Mediator.Queries;
using ErrorOr;



namespace Application.Categories.GetByName;

public sealed record CategoryByNameQuery(string Name) : IQuery<ErrorOr<CategoryResponse>>;
