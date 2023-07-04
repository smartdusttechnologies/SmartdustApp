//using Dapper;
//using System.Data;
//using SmartdustApp.Business.Infrastructure;
//using SmartdustApp.Business.Models;
//using SmartdustApp.Business.DTO;
//using SmartdustApp.Business.Data.Repository.Interfaces;

///In Future When We Need Leave Then Will Uncomment
//namespace SmartdustApp.Business.Repository
//{
//    public class LeaveRepository : ILeaveRepository
//    {
//        /// <summary>
//        /// using the userRespository
//        /// </summary>
//        private readonly IConnectionFactory _connectionFactory;

//        public LeaveRepository(IConnectionFactory connectionFactory)
//        {
//            _connectionFactory = connectionFactory;
//        }

//        /// <summary>
//        ///get the data of the Orgnaization
//        /// </summary>
//        /// <returns></returns>
//        public List<LeaveDTO> Get()
//        {
//            using IDbConnection db = _connectionFactory.GetConnection;
//            return db.Query<LeaveDTO>("select * from [Leaves] WHERE IsDeleted = 0").ToList();
//        }

//        public List<LeaveDTO> GetByUser(int id)
//        {
//            using IDbConnection db = _connectionFactory.GetConnection;
//            return db.Query<LeaveDTO>("select * from [Leaves] WHERE UserId = @id AND IsDeleted = 0", new {id}).ToList();
//        }

//        public int Insert(LeaveDTO entity)
//        {
//            using IDbConnection db = _connectionFactory.GetConnection;
//            //return db.Execute("INSERT INTO [Leaves] (UserId,Date,LeaveStatus)");
//            //return db.Query<LeaveDTO>("select * from [Leaves] WHERE UserId = @id AND IsDeleted = 0", new { id }).ToList();
//            return 0;
//        }

//        public int StatusUpdate(LeaveDTO entity)
//        {
//            throw new NotImplementedException();
//        }

//    }
//}
