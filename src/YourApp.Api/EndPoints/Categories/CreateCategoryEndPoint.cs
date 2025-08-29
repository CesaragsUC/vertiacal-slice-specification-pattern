using Application.Categories.Create;
using Application.Domain.Events.Category;
using Cortex.Mediator;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace YourApp.Api.EndPoints.Categories;


public class CreateCategoryController(IMediator mediator) : ApiControllerBase
{
    [HttpPost("/api/categories")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken ct)
    {
        var result = await mediator.SendCommandAsync<CreateCategoryCommand, ErrorOr<Guid>>(command, ct);
        return result.Match(
            id => Ok(id),
            errors => Problem(errors)
        );
    }
}