using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Orders;

public class GetVoucherByNumberEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapGet("/{number}", HandleAsync)
       .WithName("Orders: Get voucher by number")
       .WithSummary("Obtém um voucher pelo número")
       .WithDescription("Obtém um voucher pelo número")
       .WithOrder(1)
       .Produces<Response<Voucher>>();

    public static async Task<IResult> HandleAsync(IVoucherHandler handler, string number)
    {

        var request = new GetVoucherByNumberRequest()
        {
            Number = number
        };

        var result = await handler.GetByCodeAsync(request);

        return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
    }

}
