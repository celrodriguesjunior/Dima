using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class ProductHandler(AppDbContext context) : IProductHandler
{
    public async Task<PagedResponse<List<Product>?>> GetAllAsync(GetAllProductsRequest request)
    {
        try
        {
            var query = context.Products
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Title);

            var products = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var totalCount = await query.CountAsync();

            return new PagedResponse<List<Product>?>
            (
                products,
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        }
        catch (Exception ex)
        {
            return new PagedResponse<List<Product>?>(null, 500, "Não foi possível obter os produtos");
        }
    }

    public async Task<Response<Product?>> GetBySlugAsync(GetProductBySlugRequest request)
    {
        try
        {
            var product = await context.Products.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Slug == request.Slug && x.IsActive);

            return product is null ? new Response<Product?>(null, 404, "Produto não encontrado") : new Response<Product?>(product);
        }
        catch (Exception e)
        {
            return new Response<Product?>(null, 500, "Não foi possível obter o produto");
        }
    }
}
