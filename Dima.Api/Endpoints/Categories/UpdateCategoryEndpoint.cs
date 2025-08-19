using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class UpdateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut(pattern: "/{id}", handler: HandleAsync)
              .WithName("Categories: Update")
              .WithSummary("Atualiza uma categoria")
              .WithDescription("")
              .WithOrder(2)
              .Produces<Response<Category?>>();

    public static async Task<IResult> HandleAsync(ICategoryHandler handler, UpdateCategoryRequest request, long id)
    {
        request.Id = id;
        request.UserId = "teste@teste.com";
        var response = await handler.UpdateAsync(request);

        return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}
