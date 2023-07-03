using SmartdustApi.Common;
using SmartdustApi.Model;
using SmartdustApi.Model;

namespace SmartdustApi.Services.Interfaces
{
    public interface IAuthenticationService
    {
        RequestResult<LoginToken> Login(LoginRequest loginRequest);
        //TODo: This should be moved to User service.
        RequestResult<bool> Add(UserModel user);
        RequestResult<bool> UpdatePaasword(ChangePasswordModel password);

    }
}
