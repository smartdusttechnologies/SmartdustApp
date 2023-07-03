using Dapper;
using System.Data;
using SmartdustApi.Repository.Interface;
using SmartdustApi.Infrastructure;
using SmartdustApi.Models;
using SmartdustApi.DTO;

namespace SmartdustApi.Repository
{
    public class LeaveRepository : ILeaveRepository
    {
        /// <summary>
        /// using the userRespository
        /// </summary>
        private readonly IConnectionFactory _connectionFactory;

        public LeaveRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        ///get the data of the Orgnaization
        /// </summary>
        /// <returns></returns>
        public List<LeaveDTO> Get()
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<LeaveDTO>("select * from [Leaves] WHERE IsDeleted = 0").ToList();
        }

        public List<LeaveDTO> GetByUser(int id)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<LeaveDTO>("select * from [Leaves] WHERE UserId = @id AND IsDeleted = 0", new {id}).ToList();
        }

        public int Insert(LeaveDTO entity)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            //return db.Execute("INSERT INTO [Leaves] (UserId,Date,LeaveStatus)");
            //return db.Query<LeaveDTO>("select * from [Leaves] WHERE UserId = @id AND IsDeleted = 0", new { id }).ToList();
            return 0;
        }

        public int StatusUpdate(LeaveDTO entity)
        {
            throw new NotImplementedException();
        }

    }
}
