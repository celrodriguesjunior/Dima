using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using System.Net.Http;
using System.Net.Http.Json;

namespace Dima.Web.Handlers;

public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);

    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("v1/transactions", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ?? new Response<Transaction?>(null, 400, "Falha ao criar a transação");
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var result = await _httpClient.DeleteAsync($"v1/transactions/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ?? new Response<Transaction?>(null, 400, "Falha ao deletar a transação");
    }

    public async Task<Response<Transaction?>?> GetByIdAsync(GetTransactionByIdRequest request)
    => await _httpClient.GetFromJsonAsync<Response<Transaction?>>($"v1/transactions/{request.Id}") ?? new Response<Transaction?>(null, 400, "Não foi possível obter a transação");

    public async Task<PagedResponse<List<Transaction?>>> GetByPeriodAsync(GetTransactionByPeriodRequest request)
    {
        const string format = "yyyy-MM-dd";
        var startDateStr = request.StartDate is not null ? request.StartDate.Value.ToString(format) : DateTime.Now.GetFirstDay().ToString(format);
        var endDateStr = request.EndDate is not null ? request.EndDate.Value.ToString(format) : DateTime.Now.GetLasttDay().ToString(format);

        var url = $"v1/transactions?startDate={startDateStr}&endDate={endDateStr}";

        return await _httpClient.GetFromJsonAsync<PagedResponse<List<Transaction?>>>(url) ?? new PagedResponse<List<Transaction?>>(null, 400, "Não foi possível obter as transações");
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var result = await _httpClient.PutAsJsonAsync($"v1/transactions/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>() ?? new Response<Transaction?>(null, 400, "Falha ao atualizar a transação");
    }
}
