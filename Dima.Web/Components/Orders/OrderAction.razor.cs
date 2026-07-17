using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Web.Pages.Orders;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.FileProviders;
using MudBlazor;

namespace Dima.Web.Components.Orders;

public partial class OrderActionComponent : ComponentBase
{

    #region Parameters

    [Parameter, EditorRequired]
    public Order Order { get; set; } = null!;

    [CascadingParameter]
    public DetailsPage Parent { get; set; } = null!;

    #endregion

    #region Services

    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public IOrderHandler OrderHandler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    #endregion

    #region Public Methods

    public async void OnCanceledButtonClickeAsync()
    {
        bool? result = await DialogService.ShowMessageBox(
            "Cancelar Pedido",
            "Você tem certeza que deseja cancelar este pedido?",
            yesText: "Sim", cancelText: "Não");

        if (result is not null && result == true)
            await CancelOrderAsync();


    }

    public async void OnPayButtonClickedAsync()
    {
        await Task.Delay(1);
        Snackbar.Add("Função de pagamento não implementada.", Severity.Info);
    }


    public async void OnRefoundButtonClickeAsync()
    {
        bool? result = await DialogService.ShowMessageBox(
            "Cancelar Pedido",
            "Você tem certeza que deseja estornar este pedido?",
            yesText: "Sim", cancelText: "Não");

        if (result is not null && result == true)
            await RefoundOrderAsync();


    }

    #endregion


    #region Private Methods

    private async Task CancelOrderAsync()
    {
        try
        {
            var request = new CancelOrderRequest
            {
                Id = Order.Id,
                UserId = Order.UserId
            };
            var result = await OrderHandler.CancelAsync(request);
            if (result.IsSuccess)
            {
                Parent.RefreshState(result.Data!);
                Snackbar.Add(result.Message, Severity.Success);
            }
            else
            {
                Snackbar.Add(result.Message, Severity.Error);
                return;
            }

        }
        catch (Exception ex)
        {
            Snackbar.Add($"Erro ao cancelar o pedido: {ex.Message}", Severity.Error);
        }
    }

    private async Task RefoundOrderAsync()
    {
        try
        {
            var request = new RefoundOrderRequest
            {
                Id = Order.Id,
                UserId = Order.UserId
            };
            var result = await OrderHandler.RefundAsync(request);
            if (result.IsSuccess)
            {
                Parent.RefreshState(result.Data!);
                Snackbar.Add(result.Message, Severity.Success);
            }
            else
            {
                Snackbar.Add(result.Message, Severity.Error);
                return;
            }

        }
        catch (Exception ex)
        {
            Snackbar.Add($"Erro ao cancelar o pedido: {ex.Message}", Severity.Error);
        }
    }




    #endregion

}
