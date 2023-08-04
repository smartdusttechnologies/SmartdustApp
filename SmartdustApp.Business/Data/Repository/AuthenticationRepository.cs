using Dapper;
using System.Data;
using SmartdustApp.Business.Infrastructure;
using SmartdustApp.Business.Data.Repository.Interfaces.Security;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public AuthenticationRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public PasswordLogin GetLoginPassword(string userName)
        {

            using IDbConnection db = _connectionFactory.GetConnection;
            var query = @"
                         SELECT TOP 1 pl.*, ur.RoleId
                         FROM [User] u
                         INNER JOIN [PasswordLogin] pl ON u.id = pl.userId
                         LEFT JOIN [UserRole] ur ON u.id = ur.UserId
                         WHERE u.userName = @userName AND (u.IsDeleted = 0)
                         ORDER BY pl.Id DESC";

            var result = db.QueryFirstOrDefault<PasswordLogin>(query, new { userName });
            return result;
        }
        /// <summary>
        /// Save Login Token in DB
        /// </summary>
        public int SaveLoginToken(LoginToken loginToken)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            int userId = db.Query<int>(@"Select u.Id From [User] u Where u.UserName = @UserName", new { loginToken.UserName }).FirstOrDefault();
            loginToken.UserId = userId;

            int loginTokenUserId = db.Query<int>(@"Select userId From [LoginToken] Where  UserId = @userId", new { userId }).FirstOrDefault();

            string query = loginTokenUserId > 0 ?
              @"update [LoginToken] Set 
                    AccessToken = @AccessToken,
                    RefreshToken = @RefreshToken,
                    AccessTokenExpiry = @AccessTokenExpiry,
                    DeviceCode = @DeviceCode,
                    DeviceName = @DeviceName,
                    RefreshTokenExpiry = @RefreshTokenExpiry
                  Where UserId = @UserId"
              :
              @"Insert into [LoginToken](UserId, AccessToken, RefreshToken, AccessTokenExpiry, DeviceCode, DeviceName, RefreshTokenExpiry) 
                values (@UserId, @AccessToken, @RefreshToken, @AccessTokenExpiry, @DeviceCode, @DeviceName, @RefreshTokenExpiry)";

            return db.Execute(query, loginToken);
        }
    }
}
