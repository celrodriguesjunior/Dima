using Dima.Api.Common.Api;
using Dima.Api.Endpoints.Categories;
using Dima.Api.Endpoints.Identity;
using Dima.Api.Endpoints.Transactions;
using Dima.Api.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Dima.Api.Endpoints;

public static class Endpoint
{

    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGroup("/").WithTags("Health Check").MapGet("/", () => "Ok!");

        app.MapGroup("v1/categories")
            .WithTags("Categories")
            .RequireAuthorization()
            .MapEndpoint<CreateCategoryEndpoint>()
            .MapEndpoint<UpdateCategoryEndpoint>()
            .MapEndpoint<DeleteCategoryEndpoint>()
            .MapEndpoint<GetAllCategoryEndpoint>()
            .MapEndpoint<GetCategoryByIdEndpoint>();

        app.MapGroup("v1/transactions")
            .WithTags("Transactions")
            .RequireAuthorization()
            .MapEndpoint<CreateTransactionEndpoint>()
            .MapEndpoint<UpdateTransactionEndpoint>()
            .MapEndpoint<DeleteTransactionEndpoint>()
            .MapEndpoint<GetTransactionByPeriodEndpoint>()
            .MapEndpoint<GetTransactionByIdEndpoint>();

        app.MapGroup("v1/identity")
            .WithTags("Identity")
            .MapIdentityApi<User>();
        
        app.MapGroup("v1/identity")
            .WithTags("Transactions")
            .MapEndpoint<LogoutEndpoint>()
            .MapEndpoint<GetRolesEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : class, IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }

}
