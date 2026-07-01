using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Orders;

public class RefoundOrderEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapPost("/{id:long}/refound", HandleAsync)
        .WithName("Orders: Refound an order")
        .WithSummary("Reembolsa um pedido")
        .WithDescription("Reembolsa um pedido existente")
        .WithOrder(4)
        .Produces<Response<Order?>>();

    private static async Task<IResult> HandleAsync(IOrderHandler handler, long id, ClaimsPrincipal user)
    {
        var request = new RefoundOrderRequest
        {
            Id = id,
            UserId = user.Identity!.Name ?? string.Empty
        };

        var result = await handler.RefundAsync(request);

        return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
    }

}
