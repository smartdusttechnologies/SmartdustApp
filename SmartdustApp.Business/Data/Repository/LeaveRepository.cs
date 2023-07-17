using Dapper;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Business.Data.Repository.Interfaces;
using SmartdustApp.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Data.Repository
{
    public class LeaveRepository : ILeaveRepository
    {

        private readonly IConnectionFactory _connectionFactory;

        public LeaveRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<LeaveModel> Get()
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<LeaveModel>("select * from [Leave] WHERE UserID = 4").ToList();
        }

        public RequestResult<bool> Save(LeaveModel leave)
        {
            string query = @"Insert into [Leave] (UserID,LeaveType, LeaveFrom, LeaveTill,Reason, AppliedDate, LeaveStatus,LeaveDays)
                                                  values (@UserID,@LeaveType,@LeaveFrom,@LeaveTill,@Reason,@AppliedDate,@LeaveStatus,@LeaveDays)";
            using IDbConnection db = _connectionFactory.GetConnection;

            var result = db.Execute(query, leave);
            if (result > 0)
            {
                return new RequestResult<bool>(true);
            }
            else
            {
                return new RequestResult<bool>(false);
            }
        }
    }
}
