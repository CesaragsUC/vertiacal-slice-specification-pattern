using Application.Product.GetById;
using Application.Product.Response;
using Cortex.Mediator;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace YourApp.Api.Features.Products;

public class ProductByIdController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("/api/products/{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await mediator.SendQueryAsync<ProductByIdQuery, ErrorOr<ProductResponse>>(new ProductByIdQuery(id));

        return result.Match(
            products => Ok(products),
            errors => Problem(errors)
        );
    }
}

