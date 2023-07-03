using SmartdustApi.Model;
using SmartdustApi.Repository.Interface;
using SmartdustApi.Services.Interfaces;

namespace SmartdustApi.Infrastructure
{
    public class Logger : Services.Interfaces.ILogger
    {

        private readonly ILoggerRepository _loggerRepository;

        public Logger(ILoggerRepository loggerRepository)
        {
            _loggerRepository = loggerRepository;
        }
        /// <summary>
        /// Login Token Log for Login request
        /// </summary>
        public async Task<int> LoginTokenLog(LoginToken loginToken)
        {
            return await Task.Run(() => _loggerRepository.LoginTokenLog(loginToken));
        }
        /// <summary>
        /// LoginLog for Login request
        /// </summary>
        public async Task<int> LoginLog(LoginRequest loginRequest)
        {
            return await Task.Run(() => _loggerRepository.LoginLog(loginRequest));
        }
    }
}
