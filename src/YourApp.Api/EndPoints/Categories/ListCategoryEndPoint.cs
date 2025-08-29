using Application.Categories.GetAll;
using Application.Categories.Responses;
using Cortex.Mediator;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace YourApp.Api.EndPoints.Categories;

public class ListCategoryController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("/api/categories/all")]
    public async Task<IActionResult> Get()
    {
        var result = await mediator.SendQueryAsync<GetCategoryListQuery, ErrorOr<List<CategoryResponse>>>(new GetCategoryListQuery());
        return result.Match(
            categories => Ok(categories),
            errors => Problem(errors)
        );
    }
}




