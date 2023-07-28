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
            var query = @"SELECT L.ID, L.UserID, LT.Name as LeaveType, L.Reason, L.AppliedDate, L.LeaveStatus, L.LeaveDays,
                          LD.LeaveDate as LeaveDates
                          FROM [Leave] L
                        LEFT JOIN LeaveDates LD ON L.ID = LD.LeaveID
                        LEFT JOIN Lookup LT ON L.LeaveTypeID = LT.ID
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
        /// Insert into Leave and LeaveDates Table
        /// </summary>
        public RequestResult<bool> Save(LeaveModel leave)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            // Insert data into the Leave table
            string leaveInsertQuery = @"INSERT INTO [Leave] (UserID, LeaveTypeID, Reason, AppliedDate, LeaveStatus, LeaveDays)
                           VALUES (@UserID, @LeaveTypeID, @Reason, @AppliedDate, @LeaveStatus, @LeaveDays);
                           SELECT CAST(SCOPE_IDENTITY() AS INT)";

            // Prepare the parameters for LeaveDates
            List<LeaveDateModel> leaveDatesParameters = null;
            if (leave.LeaveDates != null && leave.LeaveDates.Any())
            {
                leaveDatesParameters = leave.LeaveDates.Select(leaveDate => new LeaveDateModel { LeaveID = 0, LeaveDate = leaveDate }).ToList();
            }

            // Transaction to ensure both inserts are executed atomically
            using var transaction = db.BeginTransaction();

            try
            {
                // Insert into the Leave table and get the newly inserted LeaveID
                int leaveID = db.QuerySingle<int>(leaveInsertQuery, leave, transaction);

                if (leaveDatesParameters != null && leaveDatesParameters.Any())
                {
                    // Set the LeaveID for all leave dates
                    leaveDatesParameters.ForEach(d => d.LeaveID = leaveID);

                    // Insert all leave dates into the LeaveDates table in a single batch
                    string leaveDatesInsertQuery = "INSERT INTO LeaveDates (LeaveID, LeaveDate) VALUES (@LeaveID, @LeaveDate)";
                    db.Execute(leaveDatesInsertQuery, leaveDatesParameters, transaction);
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

        // Method to fetch LeaveTypes from Lookup table
        public List<LeaveTypes> GetLeaveTypes()
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            // SQL query to fetch LeaveTypes from Lookup table based on LookupCategory name
            string query = @"
                           SELECT L.ID, L.Name
                           FROM Lookup L
                           INNER JOIN LookupCategory LC ON L.LookupCategoryID = LC.ID
                           WHERE LC.ID = 1 AND LC.IsDeleted = 0 AND L.IsDeleted = 0";

            // Use Dapper's Query method to fetch the LeaveTypes as a list of LeaveTypes objects
            return db.Query<LeaveTypes>(query).ToList();
        }

        public int GetLeaveBalance(int userID, string leaveType)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string query = $"SELECT Available FROM LeaveBalance WHERE UserID = @userID AND LeaveType = @leaveType";
            var parameters = new { userID, leaveType };

            int leaveBalance = db.ExecuteScalar<int>(query, parameters);
            return leaveBalance;
        }

        //get the data of the Leave Balance
        public List<LeaveBalance> GetLeaveBalance(int userID)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            string query = "SELECT UserID, LeaveType, Available FROM [LeaveBalance] WHERE UserID = @userID";
            var parameters = new { userID };

            return db.Query<LeaveBalance>(query, parameters).ToList();
        }

        //get the Email of the Manager
        public string GetManagerEmailByEmployeeId(int employeeId)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string query = @"SELECT Email FROM [User] WHERE Id IN 
                             (SELECT ManagerId FROM Employee WHERE EmployeeId = @employeeId)";

            var parameters = new { employeeId };
            return db.QuerySingleOrDefault<string>(query, parameters);
        }

    }
}
