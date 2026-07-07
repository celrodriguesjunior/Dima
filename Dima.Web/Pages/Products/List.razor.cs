using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Products;

public partial class ListProductsPage : ComponentBase
{

    #region Properties

    public bool IsBusy { get; set; } = false;
    public List<Product> Products { get; set; } = [];

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IProductHandler Handler { get; set; } = null!;

    #endregion


    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        IsBusy = true;
        try
        {
            var request = new GetAllProductsRequest();
            var response = await Handler.GetAllAsync(request);
            if (response.IsSuccess)
            {
                Products = response.Data ?? [];
            }
            else
            {
                Snackbar.Add(response.Message ?? "Não foi possível obter os produtos", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Ocorreu um erro ao obter os produtos: {ex.Message}", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

}
