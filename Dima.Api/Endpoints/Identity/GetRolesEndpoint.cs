using Dima.Api.Common.Api;
using Dima.Api.Models;
using Dima.Core.Models.Account;
using Microsoft.AspNetCore.Identity;
using System;
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
        var roles = identity?.FindAll(identity.RoleClaimType).Select(c => new RoleClaim()
        {
            Issuer = c.Issuer,
            OriginalIssuer = c.OriginalIssuer,
            Type = c.Type,
            Value = c.Value,
            ValueType = c.ValueType
        });

        return TypedResults.Json(roles);
    }
}
