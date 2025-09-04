using Dima.Api.Common.Api;
using Dima.Api.Endpoints.Categories;
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

        app.MapGroup("v1/identity").WithTags("Identity").MapIdentityApi<User>();
        app.MapGroup("v1/identity").WithTags("Identity").MapPost("/logout", async (SignInManager<User> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return Results.Ok();
        }).RequireAuthorization();

        app.MapGroup("v1/identity").WithTags("Identity").MapGet("/roles", async (ClaimsPrincipal user) =>
        {
            if (user.Identity is null || user.Identity?.IsAuthenticated != true)
                return Results.Unauthorized();

            var identity = user.Identity as ClaimsIdentity;
            var roles = identity?.FindAll(identity.RoleClaimType).Select(c => new { c.Issuer, c.OriginalIssuer, c.Type, c.Value, c.ValueType });

            return TypedResults.Json(roles);
        }).RequireAuthorization();

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
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : class, IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }

}
