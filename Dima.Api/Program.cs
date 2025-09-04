using Dima.Api.Common.Api;
using Dima.Api.Data;
using Dima.Api.Endpoints;
using Dima.Api.Handlers;
using Dima.Api.Models;
using Dima.Core;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.AddConfiguration();
builder.AddSecturity();

builder.AddDataContext();
builder.AddCrossOrigin();
builder.AddDocumentation();
builder.AddHandlers();

//builder.Services.AddTransient<Handler>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ConfigureDevEnvironment();
}

app.UseSecurity();

app.UseHttpsRedirection();

app.MapControllers();

app.MapEndpoints();




app.Run();


//Comandos do ef migration
//dotnet ef migrations add v3
//dotnet ef database update