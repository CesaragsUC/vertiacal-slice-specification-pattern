using Application.Product.GetByName;
using Application.Product.Response;
using Cortex.Mediator;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace YourApp.Api.Features.Products;

public class ProductByNameController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("/api/products/{name}")]
    public async Task<IActionResult> Get([FromRoute] string name)
    {
        var result = await mediator.SendQueryAsync<ProductByNameQuery, ErrorOr<IEnumerable<ProductResponse>>>(new ProductByNameQuery(name));

        return result.Match(
            products => Ok(products),
            errors => Problem(errors)
        );
    }
}

