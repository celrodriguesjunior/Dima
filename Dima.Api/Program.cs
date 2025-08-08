using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
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

//app.MapPost(
//    pattern: "/v1/categories",
//    handler: (CreateCategoryRequest request, Handler handler) => handler.Handle(request))
//    .WithName("Categories: Create")
//    .WithSummary("Cria uma nova categoria")
//    .Produces<Response<Category>>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


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