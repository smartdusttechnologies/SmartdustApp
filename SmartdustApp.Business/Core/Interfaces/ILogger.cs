using SmartdustApp.Business.Model;

namespace SmartdustApp.Business.Core.Interfaces
{
    public interface ILogger
    {
        Task<int> LoginLog(LoginRequest loginRequest);

        Task<int> LoginTokenLog(LoginToken loginToken);
    }
}
