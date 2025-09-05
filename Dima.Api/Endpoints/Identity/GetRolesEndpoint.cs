using Dima.Api.Common.Api;
using Dima.Api.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Identity;

public class GetRolesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet(pattern: "/roles", handler: HandleAsync)
                .WithName("Identity: Roles")
                .WithOrder(1)
                .RequireAuthorization();

    public static async Task<IResult> HandleAsync(ClaimsPrincipal user)
    {
        if (user.Identity is null || user.Identity?.IsAuthenticated != true)
            return Results.Unauthorized();

        var identity = user.Identity as ClaimsIdentity;
        var roles = identity?.FindAll(identity.RoleClaimType).Select(c => new { c.Issuer, c.OriginalIssuer, c.Type, c.Value, c.ValueType });

        return TypedResults.Json(roles);
    }
}
