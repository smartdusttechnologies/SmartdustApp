using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Interfaces
{
    public interface ILeaveService
    {
        RequestResult<List<LeaveModel>> Get(int userID);
        RequestResult<bool> Save(LeaveModel leave);
        RequestResult<List<LeaveTypes>> GetLeaveTypes();
        RequestResult<List<LeaveBalance>> GetLeaveBalance(int userID);
        RequestResult<List<LeaveModel>> GetEmployeeLeave(int userID);
        RequestResult<List<LeaveStatusActions>> GetManagerLeaveStatusActions();
        RequestResult<bool> UpdateLeaveStatus(UpdateLeaveModel updateStatus);
        int FileUpload(AttachedFileModel fileUpload);
    }
}
