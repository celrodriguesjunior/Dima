using Dima.Core.Handlers;
using Dima.Core.Requests.Account;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Dima.Web.Pages.Identity;

public partial class LoginPage : ComponentBase
{
    #region Dependecies

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public IAccountHandler Handler { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public ICookieAuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

    #endregion


    #region Properties
    public bool IsBusy { get; set; } = false;
    public LoginRequest InputModel { get; set; } = new();
    #endregion


    #region Overrides
    protected override async Task OnInitializedAsync()
    {
        var authStateProvider = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authStateProvider.User;

        if (user.Identity is { IsAuthenticated: true })
            NavigationManager.NavigateTo("/");

    }
    #endregion

    #region Methods
    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        try
        {
            var response = await Handler.LoginAsync(InputModel);

            if (response.IsSuccess)
            {
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                AuthenticationStateProvider.NotifyAuthenticationStateChanged();
                //Snackbar.Add("Login realizado com sucesso!", Severity.Success);
                NavigationManager.NavigateTo("/");
            }
            else
            {
                Snackbar.Add(response.Message, Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add("Ocorreu um erro inesperado. Tente novamente mais tarde." + ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }


    }
    #endregion
}
