using Dima.Api.Data;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class OrdeHandler(AppDbContext context) : IOrderHandler
{
    public async Task<Response<Order?>> CancelAsync(CancelOrderRequest request)
    {
        Order? order;
        try
        {
            order = await context.Orders
                .Include(x => x.Product)
                .Include(x => x.Voucher)
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (order is null)
                return new Response<Order?>(null, 404, "Pedido não encontrado");


        }
        catch (Exception)
        {
            return new Response<Order?>(null, 500, "Ocorreu um erro ao cancelar o pedido");
        }


        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(order, 400, "O pedido já está cancelado");
            case EOrderStatus.WaitingPayment:
                break;
            case EOrderStatus.Paid:
                return new Response<Order?>(order, 400, "O pedido já foi pago e não pode ser cancelado");
            case EOrderStatus.Refunded:
                return new Response<Order?>(order, 400, "O pedido já foi reembolsado e não pode ser cancelado");
            default:
                return new Response<Order?>(order, 400, "O pedido não pode ser cancelado.");
        }

        order.Status = EOrderStatus.Canceled;
        order.UpdateAt = DateTime.Now;

        try
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return new Response<Order?>(order, 500, "Não foi possível cancelar o pedido");
        }
        return new Response<Order?>(order, 200, $"Pedido {order.Id} cancelado com sucesso");
    }

    public async Task<Response<Order?>> CreateAsync(CreateOrderRequest request)
    {
        Product? product;
        try
        {
            product = await context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ProductId && x.IsActive == true);

            if (product is null)
                return new Response<Order?>(null, 404, "Produto não encontrado");

            context.Attach(product);
        }
        catch (Exception ex)
        {
            return new Response<Order?>(null, 500, "Falha ao obter o produto informado");
        }

        Voucher? voucher = null;
        try
        {
            if (request.VoucherId is not null)
            {
                voucher = await context.Vouchers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.VoucherId && x.IsActive == true);

                if (voucher is null)
                    return new Response<Order?>(null, 404, "Voucher não encontrado");

                if (voucher.IsActive == false)
                    return new Response<Order?>(null, 400, "Voucher já foi utilizado");

                voucher.IsActive = false;

                context.Vouchers.Update(voucher);
            }
        }
        catch (Exception e)
        {
            return new Response<Order?>(null, 500, "Falha ao obter o voucher informado");
        }

        var order = new Order
        {
            UserId = request.UserId,
            Product = product,
            ProductId = product.Id,
            Voucher = voucher ?? null,
            VoucherId = request.VoucherId ?? 0,
        };

        try
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return new Response<Order?>(null, 500, "Falha ao criar o pedido");
        }

        return new Response<Order?>(order, 201, $"Pedido {order.Number} criado com sucesso");
    }

    public Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Response<Order?>> PayAsync(PayOrderRequest request)
    {
        Order? order;

        try
        {
            order = context.Orders
                .Include(x => x.Product)
                .Include(x => x.Voucher)
                .FirstOrDefault(x => x.Id == request.Id && x.UserId == request.UserId);


            if (order is null)
                return new Response<Order?>(null, 404, "Pedido não encontrado");

        }
        catch (Exception)
        {
            return new Response<Order?>(null, 500, "Falha ao consultar pedido");
        }

        switch (order.Status)
        {
            case EOrderStatus.Canceled:
                return new Response<Order?>(order, 400, "O pedido já está cancelado e não pode ser pago");

            case EOrderStatus.Paid:
                return new Response<Order?>(order, 400, "O pedido já está pago e não pode ser pago novamente");

            case EOrderStatus.Refunded:
                return new Response<Order?>(order, 400, "O pedido já foi reembolsado e não pode ser pago");
        }

    }

    public Task<Response<Order?>> RefundAsync(RefoundOrderRequest request)
    {
        throw new NotImplementedException();
    }
}
