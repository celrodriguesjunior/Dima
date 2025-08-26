using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Transactions;

public class UpdateTransactionEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapPut(pattern: "/{id}", handler: HandleAsync)
          .WithName("Transaction: Update")
          .WithSummary("Atualiza uma transação")
          .WithDescription("")
          .WithOrder(2)
          .Produces<Response<Transaction?>>();

    public static async Task<IResult> HandleAsync(ITransactionHandler handler, UpdateTransactionRequest request, long id)
    {
        request.Id = id;
        request.UserId = "teste@teste.com";
        var response = await handler.UpdateAsync(request);

        return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}
