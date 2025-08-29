using Application.Categories.GetById;
using Application.Categories.Responses;
using Cortex.Mediator;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace YourApp.Api.EndPoints.Categories;

public class GetCategoryByIdController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("/api/categories/{id:guid}")]
    public async Task<IActionResult> Create([FromRoute] Guid id)
    {
        var result = await mediator.SendQueryAsync<GetCategoryByIdQuery, ErrorOr<CategoryResponse>>(new GetCategoryByIdQuery(id));
        return result.Match(Ok, Problem);
     
    }
}



