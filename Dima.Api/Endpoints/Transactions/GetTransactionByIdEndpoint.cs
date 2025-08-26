using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;

public class GetTransactionByIdEndpoint : IEndpoint
{

    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet(pattern: "/{id}", handler: HandleAsync)
          .WithName("Transaction: GetById")
          .WithSummary("Obtém uma transação pelo Id")
          .WithDescription("")
          .WithOrder(4)
          .Produces<Response<Transaction?>>()
          .Produces(StatusCodes.Status404NotFound);
    public static async Task<IResult> HandleAsync(ITransactionHandler handler, long id)
    {
        var response = await handler.GetByIdAsync(new GetTransactionByIdRequest() { Id = id, UserId = "teste@teste.com" });
        return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.NotFound(response);
    }
}
