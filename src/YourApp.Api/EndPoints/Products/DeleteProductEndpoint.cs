using Application.Product.Delete;
using Cortex.Mediator;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace YourApp.Api.Features.Products;

public class DeleteProductController(IMediator mediator) : ApiControllerBase
{

    [HttpDelete("/api/products/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await mediator.SendCommandAsync<DeleteProductCommand, ErrorOr<Guid>>(new DeleteProductCommand(id), ct);

        return result.Match(id => Ok(id), Problem);
    }

}

