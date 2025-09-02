using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Transactions;

public class CreateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapPost(pattern: "/", handler: HandleAsync)
          .WithName("Transaction: Create")
          .WithSummary("Cria uma nova transação")
          .WithDescription("Cria uma nova transação")
          .WithOrder(1)
          .Produces<Response<Transaction?>>();

    public static async Task<IResult> HandleAsync(ClaimsPrincipal user, ITransactionHandler handler, CreateTransactionRequest request)
    {
        request.UserId = user.Identity.Name;
        var response = await handler.CreateAsync(request);

        return response.IsSuccess ? TypedResults.Created($"/{response.Data?.Id}", response) : TypedResults.BadRequest(response.Data);
    }
}
