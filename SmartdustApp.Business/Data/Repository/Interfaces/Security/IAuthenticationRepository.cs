using SmartdustApp.Business.Model;

namespace SmartdustApp.Business.Data.Repository.Interfaces.Security
{
    public interface IAuthenticationRepository
    {
        PasswordLogin GetLoginPassword(string userName);
        int SaveLoginToken(LoginToken loginToken);
    }
}
