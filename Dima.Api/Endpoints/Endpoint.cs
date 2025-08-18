using Dima.Api.Common.Api;
using Dima.Api.Endpoints.Categories;

namespace Dima.Api.Endpoints;

public static class Endpoint
{

    public static void MapEndpoints(this WebApplication app)
    {
        // Map your endpoints here
        // Example:
        // app.MapGet("/example", () => "Hello World!");

        // You can also call other endpoint classes to map their endpoints
        // CreateCategoryEndpoint.Map(app);
        // Add other endpoint mappings as needed


        var endpoints = app.MapGroup("v1/categories")
            .WithTags("Categories")
            .RequireAuthorization()
            .MapEndpoint<CreateCategoryEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : class, IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }

}
