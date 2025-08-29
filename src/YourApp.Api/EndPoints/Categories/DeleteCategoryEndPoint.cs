using Application.Categories.Delete;
using Cortex.Mediator;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace YourApp.Api.EndPoints.Categories;

public class DeleteCategoryController(IMediator mediator) : ApiControllerBase
{

    [HttpDelete("/api/categories/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await mediator.SendCommandAsync<DeleteCategoryCommand, ErrorOr<Guid>>(new DeleteCategoryCommand(id));

        return result.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}


