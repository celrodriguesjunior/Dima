using Dima.Api.Data;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class ReportHandler(AppDbContext context) : IReportHandler
{
    public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReport(GetExpensesByCategoryRequest request)
    {
        try
        {
            var data = await context.ExpensesByCategory.AsNoTracking().Where(x => x.UserId == request.UserId).OrderByDescending(x => x.Year).ThenBy(x => x.Category).ToListAsync();

            return new Response<List<ExpensesByCategory>?>(data);
        }
        catch (Exception ex)
        {
            return new Response<List<ExpensesByCategory>?>(null, 500, $"Não foi possível obter as despesas por categoria. {ex.Message}");
        }
    }

    public async Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request)
    {
        try
        {
            //Resumo financeiro deste mes (começando do dia 1)
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var data = await context.Transactions.AsNoTracking()
                .Where(x => x.UserId == request.UserId && x.PaidOrReceivedAt >= startDate && x.PaidOrReceivedAt <= DateTime.Now)
                .GroupBy(x => 1)
                .Select(g => new FinancialSummary(
                    request.UserId,
                    g.Where(t => t.Type == ETransactionType.Deposit).Sum(t => t.Amount),
                    g.Where(t => t.Type == ETransactionType.Withdraw).Sum(t => t.Amount)
                )).FirstOrDefaultAsync();

            return new Response<FinancialSummary?>(data);
        }
        catch (Exception ex)
        {
            return new Response<FinancialSummary?>(null, 500, $"Não foi possível obter o resumo financeiro. {ex.Message}");
        }
    }

    public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(GetIncomesAndExpensesRequest request)
    {
        try
        {
            var data = await context.IncomesAndExpenses.AsNoTracking().Where(x => x.UserId == request.UserId).OrderByDescending(x => x.Year).ThenBy(x => x.Month).ToListAsync();

            return new Response<List<IncomesAndExpenses>?>(data);

        }
        catch (Exception ex)
        {
            return new Response<List<IncomesAndExpenses>?>(null, 500, $"Não foi possível obter as entradas e saídas. {ex.Message}");
        }
    }

    public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReport(GetIncomesByCategoryRequest request)
    {
        try
        {
            var data = await context.IncomesByCategory.AsNoTracking().Where(x => x.UserId == request.UserId).OrderByDescending(x => x.Year).ThenBy(x => x.Category).ToListAsync();

            return new Response<List<IncomesByCategory>?>(data);
        }
        catch (Exception ex)
        {
            return new Response<List<IncomesByCategory>?>(null, 500, $"Não foi possível obter as entradas por categoria. {ex.Message}");
        }
    }
}
