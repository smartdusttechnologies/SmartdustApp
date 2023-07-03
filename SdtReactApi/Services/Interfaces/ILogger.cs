using SmartdustApi.Model;

namespace SmartdustApi.Services.Interfaces
{
    public interface ILogger
    {
        Task<int> LoginLog(LoginRequest loginRequest);

        Task<int> LoginTokenLog(LoginToken loginToken);
    }
}
