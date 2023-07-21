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

            // Check if the leave type is Medical or Paid, and update the LeaveBalance accordingly
            if (leave.LeaveType == "Medical" || leave.LeaveType == "Paid")
            {
                _leaveRepository.UpdateLeaveBalance(leave.UserID, leave.LeaveType, leave.LeaveDays);
            }

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
