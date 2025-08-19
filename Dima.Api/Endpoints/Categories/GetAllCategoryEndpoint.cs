using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dima.Api.Endpoints.Categories;

public class GetAllCategoryEndpoint : IEndpoint
{

    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet(pattern: "/", handler: HandleAsync)
              .WithName("Categories: GetAll")
              .WithSummary("Obtém todas as categorias")
              .WithDescription("")
              .WithOrder(5)
              .Produces<PagedResponse<List<Category>>>();
    public static async Task<IResult> HandleAsync(ICategoryHandler handler, [FromQuery] int pageNumber = Configuration.DefaultPageNumber, [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var response = await handler.GetAllAsync(new GetAllCategoriesRequest()
        {
            UserId = "",
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return response.IsSuccess ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}

