using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {

            var category = new Category
            {
                UserId = request.UserId,
                Title = request.Title,
                Description = request.Description
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();
            return new Response<Category?>(category, 201, "Criado com sucesso");
        }
        catch (Exception ex)
        {
            return new Response<Category?>(null, code: 500, "Não foi possível criar a categoria");
        }

    }
    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category == null)
                return new Response<Category?>(null, code: 404, "Categoria Não Encontrada");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria Excluída com sucesso");
        }
        catch (Exception ex)
        {
            return new Response<Category?>(null, code: 500, "Não foi possível excluir a categoria");
        }


    }

    public async Task<PagedResponse<List<Category?>?>> GetAllAsync(GetAllCategoriesRequest request)
    {
        try
        {
            var query = context.Categories.AsNoTracking().Where(x => x.UserId == request.UserId);

            var categories = await query
                .Skip(request.PageSize * request.PageNumber)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query
                .CountAsync();

            return new PagedResponse<List<Category?>?>(categories, count, request.PageNumber, request.PageSize);

        }
        catch (Exception ex)
        {
            return new PagedResponse<List<Category?>?>(null, code: 500, "Não foi possível obter as categorias");
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return category != null
                ? new Response<Category?>(category, message: "Categoria encontrada")
                : new Response<Category?>(null, code: 404, "Categoria não encontrada");

        }
        catch (Exception ex)
        {
            return new Response<Category?>(null, code: 500, "Não foi possível obter a categoria");
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {

            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (category == null)
                return new Response<Category?>(null, code: 404, "Categoria Não Encontrada");


            category.Title = request.Title;
            category.Description = request.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, message: "Categoria Atualizada com sucesso");
        }
        catch (Exception)
        {
            return new Response<Category?>(null, code: 500, "Não foi possível alterar a categoria");
        }


    }
}
