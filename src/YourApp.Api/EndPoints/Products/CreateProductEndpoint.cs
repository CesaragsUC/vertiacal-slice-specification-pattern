using Cortex.Mediator;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace YourApp.Api.Features.Products;

public class CreateProductController(IMediator mediator) : ApiControllerBase
{

    [HttpPost("/api/products")]
    public async Task<IActionResult> Create(CreateProductCommand command, CancellationToken ct)
    {
        var result = await mediator.SendCommandAsync<CreateProductCommand, ErrorOr<Guid>>(command, ct);

        return result.Match(id => Ok(id), Problem);
    }

}

