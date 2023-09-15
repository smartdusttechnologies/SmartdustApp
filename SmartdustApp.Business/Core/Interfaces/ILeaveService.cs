using Microsoft.AspNetCore.Http;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Interfaces
{
    // This interface defines the contract for leave management services.
    public interface ILeaveService
    {
        // Retrieves a list of leave records for a given user.
        RequestResult<List<LeaveModel>> Get(int userID);

        // Saves a leave record and returns the result.
        RequestResult<bool> Save(LeaveModel leave);

        // Updates a leave record and returns the result.
        RequestResult<bool> Update(LeaveModel leave);

        // Retrieves a list of available leave types.
        RequestResult<List<LeaveTypes>> GetLeaveTypes();

        // Retrieves the leave balance for a specific user.
        RequestResult<List<LeaveBalance>> GetLeaveBalance(int userID);

        // Retrieves a list of leave records for employees managed by a manager.
        RequestResult<List<LeaveModel>> GetEmployeeLeave(int userID);

        // Retrieves a list of leave status actions available to managers.
        RequestResult<List<LeaveStatusActions>> GetManagerLeaveStatusActions();

        // Updates the status of a leave record and returns the result.
        RequestResult<bool> UpdateLeaveStatus(UpdateLeaveModel updateStatus);

        // Creates a leave balance record and returns the result.
        RequestResult<bool> CreateLeaveBalance(LeaveBalance leavebalance);

        // Retrieves employee details for a specific user.
        RequestResult<List<UserModel>> GetEmployeeDetails(int userID);

        // Retrieves leave balance records for a specific user.
        RequestResult<List<LeaveBalance>> GetEmployeeLeaveBalance(int userID);

        // Updates a leave balance record and returns the result.
        RequestResult<bool> UpdateLeaveBalance(LeaveBalance leavebalance);

        // Deletes a leave balance record by its ID and returns the result.
        RequestResult<bool> DeleteLeaveBalance(int id);

        // Retrieves a list of manager and employee data.
        RequestResult<List<EmployeeTable>> GetManagerAndEmployeeData();

        // Creates manager and employee data records and returns the result.
        RequestResult<bool> CreateManagerAndEmployeeData(EmployeeTable employeeData);

        // Edits manager and employee data records and returns the result.
        RequestResult<bool> EditManagerAndEmployeeData(EmployeeTable employeeData);

        // Deletes manager and employee data by ID and returns the result.
        RequestResult<bool> DeleteManagerAndEmployeeData(int id);

        // Retrieves a list of user models (possibly all users).
        RequestResult<List<UserModel>> GetUsers();
    }

}
