using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Reports;

public class GetExpensesByCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/expenses", HandleAsync)
        .Produces<Response<List<ExpensesByCategory>?>>();

    private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IReportHandler handler)
    {
        GetExpensesByCategoryRequest request = new GetExpensesByCategoryRequest()
        {
            UserId = user.Identity?.Name ?? string.Empty
        };
        var response = await handler.GetExpensesByCategoryReport(request);
        return response.IsSuccess
            ? Results.Ok(response)
            : TypedResults.BadRequest(response);
    }

}
