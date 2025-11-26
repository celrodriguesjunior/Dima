using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Reports;

public class GetFinancialSummaryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/summary", HandleAsync)
        .Produces<Response<FinancialSummary>>();

    private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IReportHandler handler)
    {
        GetFinancialSummaryRequest request = new()
        {
            UserId = user.Identity?.Name ?? string.Empty
        };
        var response = await handler.GetFinancialSummaryReportAsync(request);
        return response.IsSuccess
            ? Results.Ok(response)
            : TypedResults.BadRequest(response);
    }

}
