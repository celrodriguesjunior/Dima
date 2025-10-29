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
    #endregion

    #region Services
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
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

        #endregion

    }
}