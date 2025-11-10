using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Categories;

public partial class ListCategoriesPage : ComponentBase
{

    #region Properties
    public bool IsBusy { get; set; }
    public List<Category?> Categories { get; set; } = [];
    public string SearchString { get; set; } = string.Empty;
    #endregion

    #region Services
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    public IDialogService DialogService { get; set; } = null!;
    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;
    #endregion


    #region Overrides
    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {

            var request = new GetAllCategoriesRequest();
            var result = await Handler.GetAllAsync(request);
            if (result.IsSuccess)
            {
                Categories = result.Data ?? [];
            }
            else
            {
                Snackbar.Add($"Falha ao carregar a categoria. {result.Message}", Severity.Error);
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

    #region Methods

    public Func<Category, bool> SearchFunc => category =>
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;

        if (category.Id.ToString().Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (category.Title?.Contains(SearchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;

        if (category.Description?.Contains(SearchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;

        return false;
    };

    public async void OnDeleteButtonClickedAsync(long id, string title)
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Tem certeza que deseja excluir a categoria '{title}'?",
            yesText: "EXCLUIR", cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(id, title);

        StateHasChanged();
    }

    public async Task OnDeleteAsync(long id, string title)
    {
        IsBusy = true;
        try
        {
            var result = await Handler.DeleteAsync(new DeleteCategoryRequest() { Id = id });
            if (result.IsSuccess)
            {
                Categories.RemoveAll(c => c?.Id == id);
                Snackbar.Add($"Categoria '{title}' excluída com sucesso.", Severity.Success);
            }
            else
            {
                Snackbar.Add($"Falha ao excluir a categoria '{title}'. {result.Message}", Severity.Error);
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