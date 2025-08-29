using Cortex.Mediator;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace YourApp.Api.EndPoints.Categories;

public class UpdateCategoryController(IMediator mediator) : ApiControllerBase
{
    [HttpPut("/api/categories")]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand command)
    {
        var result = await mediator.SendCommandAsync<UpdateCategoryCommand, ErrorOr<Guid>>(command);
        return result.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}


