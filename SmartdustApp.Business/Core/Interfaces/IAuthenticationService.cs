using SmartdustApp.Business.Common;
using SmartdustApp.Business.Model;
using SmartdustApp.Business.Model;

namespace SmartdustApp.Business.Core.Interfaces
{
    public interface IAuthenticationService
    {
        RequestResult<LoginToken> Login(LoginRequest loginRequest);
        //TODo: This should be moved to User service.
        RequestResult<bool> Add(UserModel user);
        RequestResult<bool> UpdatePaasword(ChangePasswordModel password);

    }
}
