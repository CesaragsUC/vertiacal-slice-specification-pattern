using Application.Features.Categories.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.Features.MinimalApiDemo;

public static class MinimalApiDemoEndpoints
{
    public static IEndpointRouteBuilder MapMinimalApiDemoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/minimal-api-demo",
            async (CancellationToken ct) =>
            {
                await Task.Delay(1000, ct);
                return Results.Ok("Hello from minimal API");
            })
            .WithName("opa")
            .Produces<CategoryDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

        app.MapGet("/api/your-name/{name}",
            async (string name,CancellationToken ct) =>
            {
                await Task.Delay(1000, ct);
                return Results.Ok($"Hello {name}");
            })
            .WithName("opa2")
            .Produces<CategoryDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);


        return app;
    }
}