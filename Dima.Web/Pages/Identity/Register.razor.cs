using Dima.Web.Security;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Identity;

public partial class RegisterPage(CookieAuthenticationStateProvider cookieAuthenticationState) : ComponentBase
{
    [Inject]
    public CookieAuthenticationStateProvider AuthState { get; set; } = null!;

    public MudForm MudForm { get; set; } = null!;

}
