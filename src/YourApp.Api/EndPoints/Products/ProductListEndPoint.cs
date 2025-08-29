using Application.Product.GetAll;
using Application.Product.Response;
using Cortex.Mediator;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace YourApp.Api.Features.Products;

public class ProductListController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("/api/products")]
    public async Task<IActionResult> Get([FromQuery] GetProductsQuery query)
    {
        var result = await mediator.SendQueryAsync<GetProductsQuery, ErrorOr<IEnumerable<ProductResponse>>>(query);

        return result.Match(
            products => Ok(products),
            errors => Problem(errors)
        );
    }
}

