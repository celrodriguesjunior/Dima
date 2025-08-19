using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class GetCategoryByIdEndpoint : IEndpoint
{

    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet(pattern: "/{id}", handler: HandleAsync)
              .WithName("Categories: GetById")
              .WithSummary("Obtém uma categoria pelo Id")
              .WithDescription("")
              .WithOrder(4)
              .Produces<Response<Category?>>()
              .Produces(StatusCodes.Status404NotFound);
    public static async Task<IResult> HandleAsync(ICategoryHandler handler, long id)
    {
        var response = await handler.GetByIdAsync(new Core.Requests.Categories.GetCategoryByIdRequest() { Id = id, UserId = "teste@teste.com" });
        return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.NotFound(response);
    }
}
