using Microsoft.AspNetCore.Components.Authorization;

namespace Dima.Web.Security;

public interface ICookieAuthenticationStateProvider
{
    Task<AuthenticationState> GetAuthenticationStateAsync();
    Task<bool> CheckAuthenticationStateProvider();
    void NotifyAuthenticationStateChanged();
}
