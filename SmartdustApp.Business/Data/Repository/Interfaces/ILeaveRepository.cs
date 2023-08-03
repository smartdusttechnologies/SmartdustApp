using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Data.Repository.Interfaces
{
    public interface ILeaveRepository
    {
        List<LeaveModel> Get(int userID);
        RequestResult<bool> Save(LeaveModel leave);
        //public void UpdateLeaveBalance(int userID, string leaveType, int leaveDays);
        List<LeaveTypes> GetLeaveTypes();
        int GetLeaveBalance(int userID, int leaveTypeID);
        List<LeaveBalance> GetLeaveBalance(int userID);
        string GetManagerEmailByEmployeeId(int employeeId);
        List<LeaveModel> GetEmployeeLeave(int managerId);
        List<LeaveStatusActions> GetManagerLeaveStatusActions();
        RequestResult<bool> UpdateLeaveStatus(int leaveID, int statusID);
        void UpdateLeaveBalance(int leaveID);
    }
}
