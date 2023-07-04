using SmartdustApp.Business.Model;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Data.Repository.Interfaces;

namespace SmartdustApp.Business.Infrastructure
{
    public class Logger : ILogger
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
