using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class DeleteCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
            => app.MapDelete(pattern: "/{id}", handler: HandleAsync)
                  .WithName("Categories: Delete")
                  .WithSummary("Exclui uma categoria")
                  .WithDescription("")
                  .WithOrder(3)
                  .Produces<Response<Category?>>();

    public static async Task<IResult> HandleAsync(ICategoryHandler handler, long id)
    {
        var response = await handler.DeleteAsync(new DeleteCategoryRequest() { Id = id, UserId = "teste@teste.com" });

        return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}
