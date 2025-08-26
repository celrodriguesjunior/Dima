using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;

public class DeleteTransactionEndpoint : IEndpoint
{

    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete(pattern: "/{id}", handler: HandleAsync)
              .WithName("Transaction: Delete")
              .WithSummary("Exclui uma transação")
              .WithDescription("")
              .WithOrder(3)
              .Produces<Response<Transaction?>>();

    public static async Task<IResult> HandleAsync(ITransactionHandler handler, long id)
    {
        var response = await handler.DeleteAsync(new DeleteTransactionRequest() { Id = id, UserId = "teste@teste.com" });

        return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }

}
