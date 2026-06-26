using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Orders;

public class CancelOrderEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/{id}/cancel", HandleAsync)
        .WithName("Orders: Cancel order")
        .WithSummary("Cancela um pedido")
        .WithDescription("Cancela um pedido")
        .WithOrder(2)
        .Produces<Response<Order?>>();

    private static async Task<IResult> HandleAsync(IOrderHandler orderHandler, long id, ClaimsPrincipal user)
    {
        var request = new Dima.Core.Requests.Orders.CancelOrderRequest
        {
            Id = id,
            UserId = user.Identity!.Name ?? string.Empty
        };
        var response = await orderHandler.CancelAsync(request);
        return response.IsSuccess
            ? Results.Ok(response)
            : Results.BadRequest(response);
    }

}
