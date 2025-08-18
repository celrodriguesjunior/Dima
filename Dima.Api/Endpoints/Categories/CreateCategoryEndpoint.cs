using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    => app.MapPost(pattern: "/", handler: HandleAsync)
          .WithName("Categories: Create")
          .WithSummary("Cria uma nova categoria")
          .WithDescription("")
          .WithOrder(1)
          .Produces<Response<Category?>>();

    public static async Task<IResult> HandleAsync(ICategoryHandler handler, CreateCategoryRequest request)
    {
        var response = await handler.CreateAsync(request);

        return response.IsSuccess ? TypedResults.Created($"/{response.Data?.Id}", response) : TypedResults.BadRequest(response.Data);
    }
}
