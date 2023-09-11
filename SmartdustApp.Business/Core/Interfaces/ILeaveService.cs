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
    public interface ILeaveService
    {
        RequestResult<List<LeaveModel>> Get(int userID);
        RequestResult<bool> Save(LeaveModel leave);
        RequestResult<bool> Update(LeaveModel leave);
        RequestResult<List<LeaveTypes>> GetLeaveTypes();
        RequestResult<List<LeaveBalance>> GetLeaveBalance(int userID);
        RequestResult<List<LeaveModel>> GetEmployeeLeave(int userID);
        RequestResult<List<LeaveStatusActions>> GetManagerLeaveStatusActions();
        RequestResult<bool> UpdateLeaveStatus(UpdateLeaveModel updateStatus);
        List<int> UploadFiles(IFormFileCollection files);
        DocumentModel DownloadDocument(int documentID);
        RequestResult<bool> CreateLeaveBalance(LeaveBalance leavebalance);
        RequestResult<List<UserModel>> GetEmployeeDetails(int userID);
    }
}
