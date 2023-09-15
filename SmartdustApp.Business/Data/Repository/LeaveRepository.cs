using Dapper;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Business.Data.Repository.Interfaces;
using SmartdustApp.Business.Infrastructure;
using System;
using System.Collections;
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
                         SELECT L.ID, L.UserID, LT.Name as LeaveType, L.Reason, L.AppliedDate, LS.Name as LeaveStatus, L.LeaveDays,L.LeaveTypeID,
                         LD.LeaveDate as LeaveDates,
                         LAF.DocumentID as AttachedFileIDs
                         FROM [Leave] L
                         LEFT JOIN LeaveDates LD ON L.ID = LD.LeaveID
                         LEFT JOIN Lookup LT ON L.LeaveTypeID = LT.ID
                         LEFT JOIN Lookup LS ON L.LeaveStatusID = LS.ID
                         LEFT JOIN LeaveAttachedFile LAF ON L.ID = LAF.LeaveID
                         WHERE L.UserID = @userID";

            var parameters = new { userID };

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
                       leave.LeaveDates = g
                           .Where(l => l.LeaveDates != null)
                           .SelectMany(l => l.LeaveDates)
                           .Where(d => d != null)
                           .Distinct() // Filter out duplicates
                           .ToList();

                       // Handle case where g.SelectMany(l => l.AttachedFileIDs) returns null
                       leave.AttachedFileIDs = g
                           .Where(l => l.AttachedFileIDs != null)
                           .SelectMany(l => l.AttachedFileIDs)
                           .Where(id => id != null)
                           .Distinct() // Filter out duplicates
                           .ToList();

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
        /// Update Leave
        /// </summary>
        public RequestResult<bool> Update(LeaveModel leave)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            // Update data in the Leave table
            string leaveUpdateQuery = @"UPDATE [Leave]
                                SET LeaveTypeID = @LeaveTypeID,
                                    Reason = @Reason,
                                    AppliedDate = @AppliedDate,
                                    LeaveDays = @LeaveDays
                                WHERE ID = @ID";

            // Prepare the parameters for LeaveDates
            List<LeaveDateModel> leaveDatesParameters = null;
            if (leave.LeaveDates != null && leave.LeaveDates.Any())
            {
                leaveDatesParameters = leave.LeaveDates.Select(leaveDate => new LeaveDateModel { LeaveID = leave.ID, LeaveDate = leaveDate }).ToList();
            }

            // Prepare the parameters for LeaveAttachedFiles
            List<LeaveAttachedFilesModel> leaveAttachedFilesParameters = null;
            if (leave.AttachedFileIDs != null && leave.AttachedFileIDs.Any())
            {
                leaveAttachedFilesParameters = leave.AttachedFileIDs.Select(DocumentID => new LeaveAttachedFilesModel { LeaveID = leave.ID, DocumentID = DocumentID }).ToList();
            }

            // Transaction to ensure both updates are executed atomically
            using var transaction = db.BeginTransaction();

            try
            {
                // Update the Leave table
                db.Execute(leaveUpdateQuery, leave, transaction);

                if (leaveDatesParameters != null && leaveDatesParameters.Any())
                {
                    // Delete existing leave dates
                    string leaveDatesDeleteQuery = "DELETE FROM LeaveDates WHERE LeaveID = @LeaveID";
                    db.Execute(leaveDatesDeleteQuery, new { LeaveID = leave.ID }, transaction);

                    // Insert updated leave dates
                    string leaveDatesInsertQuery = "INSERT INTO LeaveDates (LeaveID, LeaveDate) VALUES (@LeaveID, @LeaveDate)";
                    db.Execute(leaveDatesInsertQuery, leaveDatesParameters, transaction);
                }

                if (leaveAttachedFilesParameters != null && leaveAttachedFilesParameters.Any())
                {
                    // Delete existing attached files
                    string leaveAttachedFilesDeleteQuery = "DELETE FROM LeaveAttachedFile WHERE LeaveID = @LeaveID";
                    db.Execute(leaveAttachedFilesDeleteQuery, new { LeaveID = leave.ID }, transaction);

                    // Insert updated attached files
                    string leaveAttachedFilesInsertQuery = "INSERT INTO LeaveAttachedFile (LeaveID, DocumentID) VALUES (@LeaveID, @DocumentID)";
                    db.Execute(leaveAttachedFilesInsertQuery, leaveAttachedFilesParameters, transaction);
                }

                // If all updates are successful, commit the transaction
                transaction.Commit();
                return new RequestResult<bool>(true);
            }
            catch (Exception ex)
            {
                // If any update fails, roll back the transaction and return an error
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

        // Retrieves a list of leave records for employees managed by a manager.
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
            // Use LINQ GroupBy to group the results by Leave ID to avoid duplicates
            return result.GroupBy(l => l.ID)
                   .Select(g =>
                   {
                       var leave = g.First();

                       // Handle case where g.SelectMany(l => l.LeaveDates) returns null
                       leave.LeaveDates = g
                           .Where(l => l.LeaveDates != null)
                           .SelectMany(l => l.LeaveDates)
                           .Where(d => d != null)
                           .Distinct() // Filter out duplicates
                           .ToList();

                       // Handle case where g.SelectMany(l => l.AttachedFileIDs) returns null
                       leave.AttachedFileIDs = g
                           .Where(l => l.AttachedFileIDs != null)
                           .SelectMany(l => l.AttachedFileIDs)
                           .Where(id => id != null)
                           .Distinct() // Filter out duplicates
                           .ToList();

                       return leave;
                   })
                   .ToList();
        }

        // Retrieves employee details for employees managed by a manager.
        public List<UserModel> GetEmployeeDetails(int managerId)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            var query = @"
                 SELECT U.Id, U.UserName
                 FROM [User] U
                 WHERE U.Id IN (
                     SELECT EmployeeId FROM Employee WHERE ManagerId = @managerId
                 )";
            var parameters = new { managerId };

            return db.Query<UserModel>(query, parameters).ToList();

        }

        // Retrieves leave balance records for employees managed by a manager.
        public List<LeaveBalance> GetEmployeeLeaveBalance(int managerId)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            var query = @"
                 SELECT LB.ID, LB.UserId, LB.LeaveType, LB.Available, U.UserName
                 FROM [LeaveBalance] LB
                 INNER JOIN [User] U ON LB.UserId = U.Id
                 WHERE LB.UserId IN (
                     SELECT EmployeeId FROM Employee WHERE ManagerId = @managerId
                 )";
            var parameters = new { managerId };

            return db.Query<LeaveBalance>(query, parameters).ToList();

        }


        // Retrieves a list of leave status actions from LookUp Table available to managers.
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

        // Updates the status of a leave record.
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

        // Updates the leave balance for a specific leave record.
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

        // Retrieves details of a specific leave record.
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

        // Creates a leave balance record and returns the result.
        public RequestResult<bool> CreateLeaveBalance(LeaveBalance leavebalance)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            string insertQuery = @"
                                  INSERT INTO [LeaveBalance] (UserID, LeaveType, Available)
                                  VALUES (@UserID, @LeaveType, @Available);
                                  SELECT CAST(SCOPE_IDENTITY() as int);";

            int result = db.Execute(insertQuery, leavebalance);
            if (result > 0)
            {
                return new RequestResult<bool>(true);
            }
            else
            {
                return new RequestResult<bool>(false);
            }
        }

        // Updates a leave balance record and returns the result.
        public RequestResult<bool> UpdateLeaveBalance(LeaveBalance leaveBalance)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string updateQuery = @"
                                  UPDATE [LeaveBalance]
                                  SET UserID = @UserID, LeaveType = @LeaveType, Available = @Available
                                  WHERE ID = @ID";

            int result = db.Execute(updateQuery, leaveBalance);

            if (result > 0)
            {
                return new RequestResult<bool>(true);
            }
            else
            {
                return new RequestResult<bool>(false);
            }
        }

        // Deletes a leave balance record by its ID and returns the result.
        public RequestResult<bool> DeleteLeaveBalance(int id)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string deleteQuery = @"
                                  DELETE FROM [LeaveBalance]
                                  WHERE ID = @ID";

            int result = db.Execute(deleteQuery, new { ID = id });

            if (result > 0)
            {
                return new RequestResult<bool>(true);
            }
            else
            {
                return new RequestResult<bool>(false);
            }
        }

        // Retrieves a list of all users.
        public List<UserModel> GetUsers()
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            var query = @"
                         SELECT Id, UserName
                         FROM [User]";

            return db.Query<UserModel>(query).ToList();
        }

        // Retrieves a list of manager and employee data.
        public List<EmployeeTable> GetManagerAndEmployeeData()
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            var query = @"
                         SELECT E.ID, E.EmployeeId, E.ManagerId, M.UserName as ManagerName, U.UserName as EmployeeName
                         FROM Employee E
                         LEFT JOIN [User] M ON E.ManagerId = M.Id
                         LEFT JOIN [User] U ON E.EmployeeId = U.Id";

            var results = db.Query<EmployeeTable>(query).ToList();

            return results;
        }

        // Creates manager and employee data.
        public RequestResult<bool> CreateManagerAndEmployeeData(EmployeeTable employeeData)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string insertEmployeeQuery = @"
                                         INSERT INTO Employee (EmployeeId, ManagerId)
                                         VALUES (@EmployeeId, @ManagerId)";

            int result = db.Execute(insertEmployeeQuery, employeeData);

            if (result > 0)
            {
                return new RequestResult<bool>(true);
            }
            else
            {
                return new RequestResult<bool>(false);
            }
        }

        // Edits manager and employee data records and returns the result.
        public RequestResult<bool> EditManagerAndEmployeeData(EmployeeTable employeeData)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string updateQuery = @"
                                  UPDATE [Employee]
                                  SET EmployeeId = @EmployeeId, ManagerId = @ManagerId
                                  WHERE ID = @ID";

            int result = db.Execute(updateQuery, employeeData);

            if (result > 0)
            {
                return new RequestResult<bool>(true);
            }
            else
            {
                return new RequestResult<bool>(false);
            }
        }

        // Deletes manager and employee data by ID and returns the result.
        public RequestResult<bool> DeleteManagerAndEmployeeData(int id)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            string deleteQuery = @"
                                  DELETE FROM [Employee]
                                  WHERE ID = @ID";

            int result = db.Execute(deleteQuery, new { ID = id });

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