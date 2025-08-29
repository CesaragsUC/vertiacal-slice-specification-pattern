using Application.Categories.GetByName;
using Application.Categories.Responses;
using Cortex.Mediator;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;



namespace YourApp.Api.EndPoints.Categories;

public class GetCategoryByNameController(IMediator mediator) : ApiControllerBase
{

    [HttpGet("/api/categories/by-name/{name}")]
    public async Task<IActionResult> Get([FromRoute] string name)
    {
        var result = await mediator.SendQueryAsync<CategoryByNameQuery, ErrorOr<CategoryResponse>>(new CategoryByNameQuery(name));
        return result.Match(Ok, Problem);
 
    }
}



