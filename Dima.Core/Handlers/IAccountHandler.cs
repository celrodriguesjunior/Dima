using Dima.Core.Requests.Account;
using Dima.Core.Responses;

namespace Dima.Core.Handlers;

public interface IAccountHandler
{
    Task<Response<string>> LoginAsync(LoginRequest loginRequest);
    Task<Response<string>> RegisterAsync(RegisterRequest loginRequest);
    Task LogoutAsync();
}
