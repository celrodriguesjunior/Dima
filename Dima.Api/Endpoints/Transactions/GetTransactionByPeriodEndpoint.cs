using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Transactions;

public class GetTransactionByPeriodEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet(pattern: "/", handler: HandleAsync)
          .WithName("Transactions: GetAll")
          .WithSummary("Obtém todas as transações")
          .WithDescription("")
          .WithOrder(5)
          .Produces<PagedResponse<List<Transaction>>>();
    public static async Task<IResult> HandleAsync(ITransactionHandler handler,
                                                  [FromQuery] DateTime? starDate = null,
                                                  [FromQuery] DateTime? endDate = null,
                                                  [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
                                                  [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var response = await handler.GetByPeriodAsync(new GetTransactionByPeriodRequest()
        {
            UserId = "",
            PageNumber = pageNumber,
            PageSize = pageSize,
            StartDate = starDate,
            EndDate = endDate
        });

        return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }

}
