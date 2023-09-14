using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Data.Repository.Interfaces
{
    public interface ILeaveRepository
    {
        List<LeaveModel> Get(int userID);
        RequestResult<bool> Save(LeaveModel leave);
        RequestResult<bool> Update(LeaveModel leave);
        List<LeaveTypes> GetLeaveTypes();
        int GetLeaveBalance(int userID, int leaveTypeID);
        List<LeaveBalance> GetLeaveBalance(int userID);
        string GetManagerEmailByEmployeeId(int employeeId);
        List<LeaveModel> GetEmployeeLeave(int managerId);
        List<LeaveStatusActions> GetManagerLeaveStatusActions();
        RequestResult<bool> UpdateLeaveStatus(int leaveID, int statusID);
        void UpdateLeaveBalance(int leaveID);
        LeaveModel GetLeaveDetails(int leaveID);
        int FileUpload(DocumentModel File);
        DocumentModel DownloadDocument(int documentID);
        RequestResult<bool> CreateLeaveBalance(LeaveBalance leavebalance);
        List<UserModel> GetEmployeeDetails(int managerId);
        List<LeaveBalance> GetEmployeeLeaveBalance(int managerId);
        RequestResult<bool> UpdateLeaveBalance(LeaveBalance leaveBalance);
        RequestResult<bool> DeleteLeaveBalance(int id);
        List<EmployeeTable> GetManagerAndEmployeeData();
        RequestResult<bool> CreateManagerAndEmployeeData(EmployeeTable employeeData);
        List<UserModel> GetUsers();
    }
}
