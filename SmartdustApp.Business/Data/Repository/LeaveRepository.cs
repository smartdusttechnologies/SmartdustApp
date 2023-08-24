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
            var query = @"
                         SELECT L.ID, L.UserID, LT.Name as LeaveType, L.Reason, L.AppliedDate, LS.Name as LeaveStatus, L.LeaveDays,
                                LD.LeaveDate as LeaveDates
                         FROM [Leave] L
                         LEFT JOIN LeaveDates LD ON L.ID = LD.LeaveID
                         LEFT JOIN Lookup LT ON L.LeaveTypeID = LT.ID
                         LEFT JOIN Lookup LS ON L.LeaveStatusID = LS.ID
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
        /// Insert the Leave 
        /// </summary>
        public RequestResult<bool> Save(LeaveModel leave)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            // Insert data into the Leave table
            string leaveInsertQuery = @"INSERT INTO [Leave] (UserID, LeaveTypeID, LeaveStatusID, Reason, AppliedDate, LeaveDays)
                    VALUES (@UserID, @LeaveTypeID, @LeaveStatusID, @Reason, @AppliedDate, @LeaveDays);
                    SELECT CAST(SCOPE_IDENTITY() AS INT)";

            // Prepare the parameters for LeaveDates
            List<LeaveDateModel> leaveDatesParameters = null;
            if (leave.LeaveDates != null && leave.LeaveDates.Any())
            {
                leaveDatesParameters = leave.LeaveDates.Select(leaveDate => new LeaveDateModel { LeaveID = 0, LeaveDate = leaveDate }).ToList();
            }

            // Prepare the parameters for LeaveAttachedFiles
            List<LeaveAttachedFilesModel> leaveAttachedFilesParameters = null;
            if (leave.AttachedFileIDs != null && leave.AttachedFileIDs.Any())
            {
                leaveAttachedFilesParameters = leave.AttachedFileIDs.Select(DocumentID => new LeaveAttachedFilesModel { LeaveID = 0, DocumentID = DocumentID }).ToList();
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

                if (leaveAttachedFilesParameters != null && leaveAttachedFilesParameters.Any())
                {
                    // Set the LeaveID for all attached files
                    leaveAttachedFilesParameters.ForEach(f => f.LeaveID = leaveID);

                    // Insert all attached files into the LeaveAttachedFiles table in a single batch
                    string leaveAttachedFilesInsertQuery = "INSERT INTO LeaveAttachedFile (LeaveID, DocumentID) VALUES (@LeaveID, @DocumentID)";
                    db.Execute(leaveAttachedFilesInsertQuery, leaveAttachedFilesParameters, transaction);
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

        /// <summary>
        /// Image Upload in DB
        /// </summary>
        public int FileUpload(DocumentModel File)
        {
            string query = @"INSERT INTO [DocumentTable](Name, FileType, DataFiles)
                    VALUES (@Name, @FileType, @DataFiles);
                    SELECT CAST(SCOPE_IDENTITY() AS INT)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.QuerySingle<int>(query, File);
        }

        /// <summary>
        /// Image download
        /// </summary>
        public DocumentModel DownloadDocument(int documentID)
        {
            using IDbConnection con = _connectionFactory.GetConnection;
            return con.Query<DocumentModel>(@"select * from [DocumentTable] where ID = @ID ", new { ID = documentID }).FirstOrDefault();
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

        //get the Leave Balance of the Particular Leave Type
        public int GetLeaveBalance(int userID, int leaveTypeID)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string query = @"
            SELECT LB.Available 
            FROM LeaveBalance LB
            INNER JOIN Lookup LT ON LB.LeaveType = LT.Name
            WHERE LB.UserID = @userID AND LT.ID = @leaveTypeID";
            var parameters = new { userID, leaveTypeID };

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
        public List<LeaveModel> GetEmployeeLeave(int managerId)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            // SQL query with JOINs to fetch data from Leave, LeaveDates, and LeaveAttachedFiles tables
            var query = @"
                         SELECT L.ID, L.UserID, U.UserName, LT.Name as LeaveType, L.Reason, L.AppliedDate, LS.Name as LeaveStatus, L.LeaveDays,
                         LD.LeaveDate as LeaveDates,
                         LAF.DocumentID as AttachedFileIDs
                         FROM [Leave] L
                         LEFT JOIN LeaveDates LD ON L.ID = LD.LeaveID
                         LEFT JOIN Lookup LT ON L.LeaveTypeID = LT.ID
                         LEFT JOIN Lookup LS ON L.LeaveStatusID = LS.ID
                         LEFT JOIN [User] U ON L.UserID = U.Id
                         LEFT JOIN LeaveAttachedFile LAF ON L.ID = LAF.LeaveID
                         WHERE L.UserID IN (
                             SELECT EmployeeId FROM Employee WHERE ManagerId = @managerId
                         )";

            var parameters = new { managerId };

            // Use Dapper's Query method to map the data to the LeaveModel class
            var result = db.Query<LeaveModel, DateTime?, int?, LeaveModel>(query, (leave, leaveDate, attachedFileID) =>
            {
                if (leaveDate != null)
                {
                    // Initialize LeaveDates property if not already initialized
                    if (leave.LeaveDates == null)
                        leave.LeaveDates = new List<DateTime>();

                    // If LeaveDate is not null, add it to the LeaveModel's LeaveDates list
                    leave.LeaveDates.Add(leaveDate.Value);
                }

                if (attachedFileID.HasValue)
                {
                    // Set AttachedFileIDs for the LeaveModel
                    if (leave.AttachedFileIDs == null)
                        leave.AttachedFileIDs = new List<int>();

                    leave.AttachedFileIDs.Add(attachedFileID.Value);
                }

                return leave;
            }, parameters, splitOn: "LeaveDates,AttachedFileIDs");

            // Use LINQ GroupBy to group the results by Leave ID to avoid duplicates
            return result.GroupBy(l => l.ID)
             .Select(g =>
             {
                 var leave = g.First();

                 // Handle case where g.SelectMany(l => l.LeaveDates) returns null
                 leave.LeaveDates = g.Where(l => l.LeaveDates != null).SelectMany(l => l.LeaveDates).ToList();

                 // Handle case where g.SelectMany(l => l.AttachedFileIDs) returns null
                 leave.AttachedFileIDs = g.Where(l => l.AttachedFileIDs != null).SelectMany(l => l.AttachedFileIDs).ToList();

                 return leave;
             })
             .ToList();
        }


        public List<LeaveStatusActions> GetManagerLeaveStatusActions()
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            // SQL query to fetch manager leave status actions from Lookup table based on LookupCategoryID
            string query = @"
                   SELECT L.ID, L.Name
                   FROM Lookup L
                   INNER JOIN LookupCategory LC ON L.LookupCategoryID = LC.ID
                   WHERE LC.ID = 3 AND LC.IsDeleted = 0 AND L.IsDeleted = 0";

            // Use Dapper's Query method to fetch the manager leave status actions as a list of LeaveTypes objects
            return db.Query<LeaveStatusActions>(query).ToList();
        }
        public RequestResult<bool> UpdateLeaveStatus(int leaveID, int statusID)
        {
            using (IDbConnection db = _connectionFactory.GetConnection)
            {
                // SQL query to update the LeaveStatusID for the specified leaveID
                string query = @"UPDATE [Leave]
                         SET LeaveStatusID = @statusID
                         WHERE ID = @leaveID";

                var parameters = new { leaveID, statusID };
                int result = db.Execute(query, parameters);
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
        public void UpdateLeaveBalance(int leaveID)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            // Update the LeaveBalance table in a single query
            string updateLeaveBalanceQuery = @"
            UPDATE LB
            SET LB.Available = LB.Available - L.LeaveDays
            FROM LeaveBalance LB
            INNER JOIN [Leave] L ON LB.UserID = L.UserID
            INNER JOIN Lookup LT ON L.LeaveTypeID = LT.ID
            WHERE L.ID = @leaveID AND LB.LeaveType = LT.Name";

            var parameters = new { leaveID };
            db.Execute(updateLeaveBalanceQuery, parameters);
        }
        public LeaveModel GetLeaveDetails(int leaveID)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            // SQL query with JOIN to fetch data from the Leave table
            var query = @"
        SELECT L.UserID, L.LeaveTypeID, L.Reason, L.AppliedDate, L.LeaveDays
        FROM [Leave] L
        WHERE L.ID = @leaveID";

            var parameters = new { leaveID };
            return db.QuerySingleOrDefault<LeaveModel>(query, parameters);
        }
    }
}
