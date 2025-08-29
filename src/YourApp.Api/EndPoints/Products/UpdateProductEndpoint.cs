using Application.Product.Update;
using Cortex.Mediator;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace YourApp.Api.Features.Products;

public class UpdateProductController(IMediator mediator) : ApiControllerBase
{

    [HttpPut("/api/products")]
    public async Task<IActionResult> Update(UpdateProductCommand command)
    {
        var result = await mediator.SendCommandAsync<UpdateProductCommand, ErrorOr<Guid>>(command);

        return result.Match(id => Ok(id), Problem);
    }

}



