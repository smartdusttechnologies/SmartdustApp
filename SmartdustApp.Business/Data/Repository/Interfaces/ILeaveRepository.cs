using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Data.Repository.Interfaces
{
    // This interface defines the contract for managing employee leave data.
    public interface ILeaveRepository
    {
        // Retrieves a list of leave records for a given user.
        List<LeaveModel> Get(int userID);

        // Saves a leave record and returns the result.
        RequestResult<bool> Save(LeaveModel leave);

        // Updates a leave record and returns the result.
        RequestResult<bool> Update(LeaveModel leave);

        // Retrieves a list of available leave types.
        List<LeaveTypes> GetLeaveTypes();

        // Retrieves the leave balance for a specific user and leave type.
        int GetLeaveBalance(int userID, int leaveTypeID);

        // Retrieves a list of leave balances for a specific user.
        List<LeaveBalance> GetLeaveBalance(int userID);

        // Retrieves the manager's email address based on the employee ID.
        string GetManagerEmailByEmployeeId(int employeeId);

        // Retrieves a list of leave records for employees managed by a manager.
        List<LeaveModel> GetEmployeeLeave(int managerId);

        // Retrieves a list of leave status actions available to managers.
        List<LeaveStatusActions> GetManagerLeaveStatusActions();

        // Updates the status of a leave record and returns the result.
        RequestResult<bool> UpdateLeaveStatus(int leaveID, int statusID, string comment);

        // Updates the leave balance for a specific leave record.
        void UpdateLeaveBalance(int leaveID);

        // Retrieves details of a specific leave record.
        LeaveModel GetLeaveDetails(int leaveID);

        // Creates a leave balance record and returns the result.
        RequestResult<bool> CreateLeaveBalance(LeaveBalance leavebalance);

        // Retrieves employee details for employees managed by a manager.
        List<UserModel> GetEmployeeDetails(int managerId);

        // Retrieves leave balance records for employees managed by a manager.
        List<LeaveBalance> GetEmployeeLeaveBalance(int managerId);

        // Updates a leave balance record and returns the result.
        RequestResult<bool> UpdateLeaveBalance(LeaveBalance leaveBalance);

        // Deletes a leave balance record by its ID and returns the result.
        RequestResult<bool> DeleteLeaveBalance(int id);

        // Retrieves a list of manager and employee data.
        List<EmployeeTable> GetManagerAndEmployeeData();

        // Creates manager and employee data records and returns the result.
        RequestResult<bool> CreateManagerAndEmployeeData(EmployeeTable employeeData);

        // Edits manager and employee data records and returns the result.
        RequestResult<bool> EditManagerAndEmployeeData(EmployeeTable employeeData);

        // Deletes manager and employee data by ID and returns the result.
        RequestResult<bool> DeleteManagerAndEmployeeData(int id);

        // Retrieves a list of all users.
        List<UserModel> GetUsers();
    }
}
