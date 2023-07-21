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
using static SmartdustApp.Business.Core.Model.PolicyTypes;

namespace SmartdustApp.Business.Data.Repository
{
    public class LeaveRepository : ILeaveRepository
    {

        private readonly IConnectionFactory _connectionFactory;

        public LeaveRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Get Leave Based on UserID
        /// </summary>
        public List<LeaveModel> Get(int userID)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            // SQL query with JOIN to fetch data from both Leave and LeaveDates tables
            var query = @"SELECT L.ID, L.UserID, L.UserName, L.LeaveType, L.Reason, L.AppliedDate, L.LeaveStatus, L.LeaveDays,
                                 LD.LeaveDate as LeaveDates
                          FROM [Leave] L
                          LEFT JOIN LeaveDates LD ON L.ID = LD.LeaveID
                          WHERE L.UserID = @userID";

            var parameters = new { userID };

            // Use Dapper's Query method to map the data to the LeaveModel class
            var result = db.Query<LeaveModel, DateTime?, LeaveModel>(query, (leave, leaveDate) =>
            {
                if (leaveDate != null)
                {
                        // Initialize LeaveDates property if not already initialized
                        if (leave.LeaveDates == null)
                            leave.LeaveDates = new List<DateTime>();

                        // If LeaveDate is not null, add it to the LeaveModel's LeaveDates list
                        leave.LeaveDates.Add(leaveDate.Value);
                }
                return leave;
            }, parameters, splitOn: "LeaveDates");

            // Use LINQ GroupBy to group the results by Leave ID to avoid duplicates
            return result.GroupBy(l => l.ID)
                         .Select(g =>
                         {
                             var leave = g.First();

                             // Handle case where g.SelectMany(l => l.LeaveDates) returns null
                             leave.LeaveDates = g.Where(l => l.LeaveDates != null).SelectMany(l => l.LeaveDates).ToList();

                             return leave;
                         })
                         .ToList();
        }

        /// <summary>
        /// Insert into Leave and Leave Dates Table
        /// </summary>
        public RequestResult<bool> Save(LeaveModel leave)
        {

            using IDbConnection db = _connectionFactory.GetConnection;

            // Insert data into the Leave table
            string leaveInsertQuery = @"INSERT INTO [Leave] (UserID, UserName, LeaveType, Reason, AppliedDate, LeaveStatus, LeaveDays)
                                       VALUES (@UserID, @UserName, @LeaveType, @Reason, @AppliedDate, @LeaveStatus, @LeaveDays);
                                       SELECT CAST(SCOPE_IDENTITY() AS INT)";

            // Insert data into the LeaveDates table
            string leaveDatesInsertQuery = "INSERT INTO LeaveDates (LeaveID, LeaveDate) VALUES (@LeaveID, @LeaveDate)";

            // Transaction to ensure both inserts are executed atomically
            using var transaction = db.BeginTransaction();

            try
            {
                // Insert into the Leave table and get the newly inserted LeaveID
                int leaveID = db.QuerySingle<int>(leaveInsertQuery, leave, transaction);

                // Insert each leave date into the LeaveDates table
                foreach (DateTime leaveDate in leave.LeaveDates)
                {
                    db.Execute(leaveDatesInsertQuery, new { LeaveID = leaveID, LeaveDate = leaveDate }, transaction);
                }

                // If all inserts are successful, commit the transaction
                transaction.Commit();
                return new RequestResult<bool>(true);
            }
            catch (Exception ex)
            {
                // If any insert fails, roll back the transaction and return an error
                transaction.Rollback();
                return new RequestResult<bool>(false, new List<ValidationMessage> { new ValidationMessage { Reason = ex.Message, Severity = ValidationSeverity.Error } });
            }
        }

        public void UpdateLeaveBalance(int userID, string leaveType, int leaveDays)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string updateQuery = string.Empty;
            if (leaveType == "Medical")
            {
                updateQuery = "UPDATE LeaveBalance SET MedicalLeave = MedicalLeave - @leaveDays WHERE UserID = @userID";
            }
            else if (leaveType == "Paid")
            {
                updateQuery = "UPDATE LeaveBalance SET PaidLeave = PaidLeave - @leaveDays WHERE UserID = @userID";
            }
            else
            {
                // Handle other leave types or throw an exception for unsupported types
                throw new Exception("Unsupported leave type.");
            }

            var parameters = new { userID, leaveDays };
            db.Execute(updateQuery, parameters);
        }
    }
}
