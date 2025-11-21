using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions;

public partial class EditTransactionPage : ComponentBase
{
    #region Properties
    public string TransactionId { get; set; } = string.Empty;
    public bool IsBusy { get; set; }
    public UpdateTransactionRequest InputModel { get; set; } = new();
    public List<Category> Categories { get; set; } = [];
    #endregion


    #region Services

    [Inject]
    public ITransactionHandler TransactionHandler { get; set; } = null!;
    [Inject]
    public ICategoryHandler CategoryHandler { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;


    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;

        await GetTransactionByIdAsync();
        await GetCategoriesAsync();

        IsBusy = false;
    }

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await TransactionHandler.UpdateAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add("Lançamento atualizado", Severity.Success);
                NavigationManager.NavigateTo("/lancamento/historico");
            }
            else
            {
                Snackbar.Add(result.Message, Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

    #region Private Methods

    private async Task GetCategoriesAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllCategoriesRequest
            {
                PageNumber = 1,
                PageSize = 1000
            };
            var categoryResult = await CategoryHandler.GetAllAsync(request);
            if (categoryResult.IsSuccess)
            {
                Categories = categoryResult?.Data ?? [];
                InputModel.CategoryId = Categories.FirstOrDefault()?.Id ?? 0;
            }
            else
            {
                Snackbar.Add(categoryResult.Message, Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task GetTransactionByIdAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetTransactionByIdRequest
            {
                Id = long.Parse(TransactionId)
            };

            var categoryResult = await TransactionHandler.GetByIdAsync(request);
            if (categoryResult.IsSuccess && categoryResult.Data is null)
            {
                InputModel = new UpdateTransactionRequest
                {
                    CategoryId = categoryResult.Data.CategoryId,
                    PaidOrReceivedAt = categoryResult.Data.PaidOrReceivedAt,
                    Title = categoryResult.Data.Title,
                    Type = categoryResult.Data.Type,
                    Amount = categoryResult.Data.Amount,
                    Id = categoryResult.Data!.Id,

                };
                InputModel.CategoryId = Categories.FirstOrDefault()?.Id ?? 0;
            }
            else
            {
                Snackbar.Add(categoryResult.Message, Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

}
