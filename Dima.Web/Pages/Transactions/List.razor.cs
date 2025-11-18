using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions;

public partial class ListTransactionsPage : ComponentBase
{

    #region Properties

    public bool IsBusy { get; set; } = false;
    public List<Transaction> Transactions { get; set; } = [];
    public string SearchTerm { get; set; } = string.Empty;
    public int CurrentYear { get; set; } = DateTime.Now.Year;
    public int CurrentMonth { get; set; } = DateTime.Now.Month;

    public int[] Years { get; set; } = { DateTime.Now.Year, DateTime.Now.Year - 1, DateTime.Now.Year - 2, DateTime.Now.Year - 3 };

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    public IDialogService DialogService { get; set; } = null!;
    [Inject]
    public ITransactionHandler TransactionHandler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync() => await LoadTransactionsAsync();

    #endregion

    #region Public Methods

    public Func<Transaction, bool> FilterTransactions => t =>
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
            return true;


        return t.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || t.Id.ToString().Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
    };

    public async void OnDeleteButtonClickedAsync(long id, string title)
    {
        var result = await DialogService.ShowMessageBox(
            "Confirmação",
            "Tem certeza que deseja excluir esta transação?",
            yesText: "Sim", noText: "Não");

        if (result == true)
            await OnDeleteAsync(id, title);

        StateHasChanged();
    }

    public async Task OnSearchAsync()
    {
        await LoadTransactionsAsync();
        StateHasChanged();
    }

    #endregion

    #region Private Methods

    private async Task LoadTransactionsAsync()
    {
        try
        {
            IsBusy = true;
            var response = await TransactionHandler.GetByPeriodAsync(new()
            {
                //UserId = AppState.CurrentUser!.Id,
                StartDate = DateTime.Now.GetFirstDay(CurrentYear, CurrentMonth),
                EndDate = DateTime.Now.GetLasttDay(CurrentYear, CurrentMonth),
                PageNumber = 1,
                PageSize = 1000,
            });
            if (response.IsSuccess && response.Data is not null)
                Transactions = response.Data;
            else
                Transactions = [];
        }
        catch (Exception ex)
        {
            Snackbar.Add("Não foi possível carregar as transações." + ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
            StateHasChanged();
        }
    }

    private async Task OnDeleteAsync(long id, string title)
    {
        IsBusy = true;
        try
        {
            var result = await TransactionHandler.DeleteAsync(new Core.Requests.Transactions.DeleteTransactionRequest() { Id = id });
            if (result.IsSuccess)
            {
                Snackbar.Add(title + " excluída com sucesso.", Severity.Success);
                Transactions.RemoveAll(x => x.Id == id);
            }
            else
            {
                Snackbar.Add("Não foi possível excluir a transação. " + result.Message, Severity.Error);
            }

        }
        catch (Exception ex)
        {
            Snackbar.Add("Erro ao excluir a transação. " + ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }

    }

    #endregion

}
