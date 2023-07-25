using Microsoft.AspNetCore.SignalR;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Business.Data.Repository.Interfaces;
using SmartdustApp.Business.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeaveService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public RequestResult<List<LeaveModel>> Get(int userID)
        {
            var leave = _leaveRepository.Get(userID);
            if (leave == null)
            {
                return new RequestResult<List<LeaveModel>>();
            }
            return new RequestResult<List<LeaveModel>>(leave);
        }

        public RequestResult<List<LeaveModel>> Get()
        {
            throw new NotImplementedException();
        }

        public RequestResult<bool> Save(LeaveModel leave)
        {
            //var validationResult = ValidateLeaveDate(leave);
            //if (!validationResult.IsSuccessful)
            //{
            //    return validationResult;
            //}

            // Validate the leave balance before saving the leave application
            //var validation = CheckLeaveBalance(leave);
            //if (!validation.IsSuccessful)
            //{
            //    return validation;
            //}

            var result = _leaveRepository.Save(leave);
            if (result.IsSuccessful)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Leave Applied Successfully",Severity=ValidationSeverity.Information}
                };
                result.Message = success;
                return result;
            }
            List<ValidationMessage> error = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Unable To take Your Request Right Now",Severity=ValidationSeverity.Information}
                };
            result.Message = error;
            return result;
        }

        public RequestResult<List<string>> GetLeaveTypes()
        {
            var leavetypes = _leaveRepository.GetLeaveTypes();
            if (leavetypes == null)
            {
                return new RequestResult<List<string>>();
            }
            return new RequestResult<List<string>>(leavetypes);
        }
        private RequestResult<bool> CheckLeaveBalance(LeaveModel leave)
        {
            // Fetch the user's leave balance from the LeaveBalance table
            int leaveBalance = _leaveRepository.GetLeaveBalance(leave.UserID, leave.LeaveType);

            // Check if the leave balance is sufficient
            if (leave.LeaveDays > leaveBalance)
            {
                return new RequestResult<bool>(false, new List<ValidationMessage> { new ValidationMessage { Reason = $"Insufficient {leave.LeaveType} Leave Balance", Severity = ValidationSeverity.Error } });
            }

            // Leave balance is sufficient, return successful result
            return new RequestResult<bool>(true);
        }

        public RequestResult<List<LeaveBalance>> GetLeaveBalance(int userID)
        {
            var leaveBalance = _leaveRepository.GetLeaveBalance(userID);
            if (leaveBalance == null)
            {
                return new RequestResult<List<LeaveBalance>>();
            }
            return new RequestResult<List<LeaveBalance>>(leaveBalance);
        }
        //private RequestResult<bool> ValidateLeaveDate(LeaveModel leave)
        //{
        //    var currentDate = DateTime.Today;
        //    if (leave.LeaveFrom.Date < currentDate)
        //    {
        //        List<ValidationMessage> validationMessages = new List<ValidationMessage>()
        //        {
        //            new ValidationMessage() { Reason = "Leave From Date cannot be before the current date.", Severity = ValidationSeverity.Error }
        //        };
        //        return new RequestResult<bool>(false, validationMessages);
        //    }

        //    if (leave.LeaveTill.Date < currentDate)
        //    {
        //        List<ValidationMessage> validationMessages = new List<ValidationMessage>()
        //        {
        //            new ValidationMessage() { Reason = "Leave Till Date cannot be before the current date.", Severity = ValidationSeverity.Error }
        //        };
        //        return new RequestResult<bool>(false, validationMessages);
        //    }

        //    if (leave.LeaveTill.Date < leave.LeaveFrom.Date)
        //    {
        //        List<ValidationMessage> validationMessages = new List<ValidationMessage>()
        //        {
        //            new ValidationMessage() { Reason = "Leave Till Date cannot be before Leave From Date.", Severity = ValidationSeverity.Error }
        //        };
        //        return new RequestResult<bool>(false, validationMessages);
        //    }

        //    return new RequestResult<bool>(true);
        //}
    }
}
