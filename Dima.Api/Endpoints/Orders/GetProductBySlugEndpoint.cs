using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Orders;

public class GetProductBySlugEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/{slug}", HandleAsync)
        .WithName("Orders: Get product by slug")
        .WithSummary("Obtém um produto pelo slug")
        .WithDescription("Obtém um produto pelo slug")
        .WithOrder(2)
        .Produces<Response<Product>>();

    private static async Task<IResult> HandleAsync(IProductHandler handler, string slug)
    {
        var request = new GetProductBySlugRequest
        {
            Slug = slug
        };

        var result = await handler.GetBySlugAsync(request);

        return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
    }

}
