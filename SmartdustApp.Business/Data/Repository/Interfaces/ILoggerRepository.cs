﻿using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Data.Repository.Interfaces
{
    public interface ILoggerRepository
    {
        int LoginLog(LoginRequest loginRequest);
        int LoginTokenLog(LoginToken loginToken);
    }
}
