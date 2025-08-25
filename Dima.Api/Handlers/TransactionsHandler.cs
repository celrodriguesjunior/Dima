using Dima.Api.Data;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class TransactionsHandler(AppDbContext context) : ITransactionHandler
{
    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        try
        {

            var Transaction = new Transaction
            {
                UserId = request.UserId,
                Title = request.Title,
                Amount = request.Amount,
                CategoryId = request.CategoryId,
                PaidOrReceivedAt = request.PaidOrReceivedAt,
                Type = request.Type,
            };

            await context.Transactions.AddAsync(Transaction);
            await context.SaveChangesAsync();
            return new Response<Transaction?>(Transaction, 201, "Transação Criada com sucesso");
        }
        catch (Exception ex)
        {
            return new Response<Transaction?>(null, code: 500, "Não foi possível criar a Transação");
        }

    }
    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        try
        {
            var Transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (Transaction == null)
                return new Response<Transaction?>(null, code: 404, "Transação Não Encontrada");

            context.Transactions.Remove(Transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(Transaction, message: "Transação Excluída com sucesso");
        }
        catch (Exception ex)
        {
            return new Response<Transaction?>(null, code: 500, "Não foi possível excluir a Transação");
        }


    }

    public async Task<PagedResponse<List<Transaction?>?>> GetByPeriodAsync(GetTransactionByPeriodRequest request)
    {
        try
        {
            var query = context.Transactions.AsNoTracking().Where(x => x.UserId == request.UserId).OrderBy(e => e.Title);

            var Transactions = await query
                .Skip((request.PageSize - 1) * request.PageNumber)
                .Take(request.PageSize)
                .ToListAsync();

            var count = await query
                .CountAsync();

            return new PagedResponse<List<Transaction?>?>(Transactions, count, request.PageNumber, request.PageSize);

        }
        catch (Exception ex)
        {
            return new PagedResponse<List<Transaction?>?>(null, code: 500, "Não foi possível obter as Transaçãos");
        }
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        try
        {
            var Transaction = await context.Transactions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            return Transaction != null
                ? new Response<Transaction?>(Transaction, message: "Transação encontrada")
                : new Response<Transaction?>(null, code: 404, "Transação não encontrada");

        }
        catch (Exception ex)
        {
            return new Response<Transaction?>(null, code: 500, "Não foi possível obter a Transação");
        }
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        try
        {

            var Transaction = await context.Transactions.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

            if (Transaction == null)
                return new Response<Transaction?>(null, code: 404, "Transação Não Encontrada");


            Transaction.Title = request.Title;
            Transaction.Amount = request.Amount;
            Transaction.CategoryId = request.CategoryId;
            Transaction.PaidOrReceivedAt = request.PaidOrReceivedAt;
            Transaction.Type = request.Type;

            context.Transactions.Update(Transaction);
            await context.SaveChangesAsync();

            return new Response<Transaction?>(Transaction, message: "Transação Atualizada com sucesso");
        }
        catch (Exception)
        {
            return new Response<Transaction?>(null, code: 500, "Não foi possível alterar a Transação");
        }


    }
}


