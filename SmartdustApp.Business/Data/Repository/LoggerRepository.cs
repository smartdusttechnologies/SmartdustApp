﻿using Dapper;
using SmartdustApp.Business.Infrastructure;
using SmartdustApp.Business.Data.Repository.Interfaces;
using System.Data;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Repository
{
    public class LoggerRepository : ILoggerRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public LoggerRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
           
        }
        /// <summary>
        /// LoginRequest Method
        /// </summary>
        public int LoginLog(LoginRequest loginRequest)
        {
            string query = @"Insert into [LoginLog](UserId, LoginDate, Status, UserName, PasswordHash, IPAddress, Browser, DeviceCode, DeviceName) 
                values (@Id, @LoginDate, @Status, @UserName, @PasswordHash, @IPAddress, @Browser, @DeviceCode, @DeviceName)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, loginRequest);
        }
        /// <summary>
        /// LoginTokeLog Method
        /// </summary>
        public int LoginTokenLog(LoginToken loginToken)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string query = @"Insert into [LoginTokenLog](UserId, AccessToken, RefreshToken, AccessTokenExpiry, DeviceCode, DeviceName, RefreshTokenExpiry) 
                values (@UserId, @AccessToken, @RefreshToken, @AccessTokenExpiry, @DeviceCode, @DeviceName, @RefreshTokenExpiry)";

            return db.Execute(query, loginToken);
        }

    }
}
