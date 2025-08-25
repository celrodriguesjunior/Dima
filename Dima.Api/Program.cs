using Dima.Api.Data;
using Dima.Api.Endpoints;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string connStr = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => { x.CustomSchemaIds(n => n.FullName); });
//builder.Services.AddTransient<Handler>();
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "Ok!");

//app.MapPost(
//    pattern: "/v1/categories",
//    handler: async (ICategoryHandler handler, CreateCategoryRequest request) => await handler.CreateAsync(request))
//    .WithName("Categories: Create")
//    .WithSummary("Cria uma nova categoria")
//    .Produces<Response<Category>>();

//app.MapPut(
//    pattern: "/v1/categories/{id}",
//    handler: async (ICategoryHandler handler, long id, UpdateCategoryRequest request) =>
//    {
//        request.Id = id;
//        return await handler.UpdateAsync(request);
//    })
//    .WithName("Categories: Update")
//    .WithSummary("Atualiza uma categoria")
//    .Produces<Response<Category>>();

//app.MapDelete(
//    pattern: "/v1/categories/{id}",
//    handler: async (ICategoryHandler handler,long id) =>
//    {
//        var request = new DeleteCategoryRequest
//        {
//            Id = id
//        };
//        return await handler.DeleteAsync(request);
//    })
//    .WithName("Categories: Delete")
//    .WithSummary("Deleta uma categoria")
//    .Produces<Response<Category>>();

//app.MapGet(
//    pattern: "/v1/categories/{id}",
//    handler: async (ICategoryHandler handler,long id) =>
//    {
//        var request = new GetCategoryByIdRequest
//        {
//            Id = id
//        };
//        return await handler.GetByIdAsync(request);
//    })
//    .WithName("Categories: Get by Id")
//    .WithSummary("Retorna uma categoria")
//    .Produces<Response<Category>>();

//app.MapGet(
//    pattern: "/v1/categories",
//    handler: async (ICategoryHandler handler) =>
//    {
//        var request = new GetAllCategoriesRequest
//        {
//            UserId = ""
//        };
//        return await handler.GetAllAsync(request);
//    })
//    .WithName("Categories: Get All Categories")
//    .WithSummary("Retorna todas uma categoria")
//    .Produces<Response<Category>>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

app.MapEndpoints();

//public class Request
//{
//    public string Title { get; set; } = string.Empty;
//    public string Description { get; set; } = string.Empty;
//}

//public class Response
//{
//    public long Id { get; set; }
//    public string Title { get; set; }
//}

//public class Handler(AppDbContext context)
//{
//    public Response Handle(Request request)
//    {

//        // Faz todo o processo de criação
//        // Persiste no banco...

//        var category = new Category
//        {
//            Title = request.Title,
//            Description = request.Description,
//        };

//        context.Categories.Add(category);
//        context.SaveChanges();

//        return new Response { Id = category.Id, Title = category.Title };

//    }
//}