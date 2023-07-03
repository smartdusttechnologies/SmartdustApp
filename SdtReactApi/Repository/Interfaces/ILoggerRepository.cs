

using SmartdustApi.Model;

namespace SmartdustApi.Repository.Interface
{
    public interface ILoggerRepository
    {
       int LoginLog(LoginRequest loginRequest);
       int LoginTokenLog(LoginToken loginToken);
    }
}
