using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Data.Repository.Interfaces
{
    public interface ILeaveRepository
    {
        List<LeaveModel> Get(int userID);
        RequestResult<bool> Save(LeaveModel leave);
        //public void UpdateLeaveBalance(int userID, string leaveType, int leaveDays);
        List<string> GetLeaveTypes();
        int GetLeaveBalance(int userID, string leaveType);
        List<LeaveBalance> GetLeaveBalance(int userID);
        //List<LeaveModel> InsertDates(List<DateTime> leavedates);
        //string InsertDates(List<DateTime> leavedates);
    }
}
