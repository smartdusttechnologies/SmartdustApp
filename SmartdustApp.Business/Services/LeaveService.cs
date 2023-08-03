﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Business.Data.Repository.Interfaces;
using SmartdustApp.Business.Enums;
using SmartdustApp.Business.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingAndCalibrationLabs.Business.Core.Interfaces;
using TestingAndCalibrationLabs.Business.Services;

namespace SmartdustApp.Business.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUserRepository _userRepository;

        public LeaveService(ILeaveRepository leaveRepository, IEmailService emailservice, IConfiguration configuration , IWebHostEnvironment hostingEnvironment, IUserRepository userRepository)
        {
            _leaveRepository = leaveRepository;
            _emailService = emailservice;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _userRepository = userRepository;
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
            var validation = CheckLeaveBalance(leave);
            if (!validation.IsSuccessful)
            {
                return validation;
            }

            // Get the manager's email using the employee ID GetManagerAndEmployeeDetails
            string managerEmail = _leaveRepository.GetManagerEmailByEmployeeId(leave.UserID);
            var user = _userRepository.Get(leave.UserID);

            // Create the email model and replace placeholders with data
            EmailModel model = new EmailModel();
            model.EmailTemplate = _configuration["SmartdustAppLeave:ManagerTemplate"];
            model.Subject = "Employee Leave Details";

            model.HtmlMsg = CreateManagerBody(model.EmailTemplate);
            model.HtmlMsg = model.HtmlMsg.Replace("*EmployeeName*", user.UserName);
            model.HtmlMsg = model.HtmlMsg.Replace("*LeaveType*", leave.LeaveType);
            model.HtmlMsg = model.HtmlMsg.Replace("*LeaveDays*", leave.LeaveDays.ToString());

            model.Email = new List<string>();
            model.Email.Add(managerEmail);

            var result = _leaveRepository.Save(leave);

            // Send the email
            var isEmailSendSuccessfully = _emailService.Sendemail(model);

            if (result.IsSuccessful && isEmailSendSuccessfully.IsSuccessful)
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
        public RequestResult<List<LeaveModel>> GetEmployeeLeave(int userID)
        {
            var leave = _leaveRepository.GetEmployeeLeave(userID);
            if (leave == null)
            {
                return new RequestResult<List<LeaveModel>>();
            }
            return new RequestResult<List<LeaveModel>>(leave);
        }
        public RequestResult<bool> UpdateLeaveStatus(UpdateLeaveModel updateStatus)
        {
            if (updateStatus.StatusID == LookupMapping.TypeToID[Lookup.Approve])
            {
                _leaveRepository.UpdateLeaveBalance(updateStatus.LeaveID);
            }

            var result = _leaveRepository.UpdateLeaveStatus(updateStatus.LeaveID , updateStatus.StatusID);

            if (result.IsSuccessful)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Updated Successfully",Severity=ValidationSeverity.Information}
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
        public RequestResult<List<LeaveTypes>> GetLeaveTypes()
        {
            var leavetypes = _leaveRepository.GetLeaveTypes();
            if (leavetypes == null)
            {
                return new RequestResult<List<LeaveTypes>>();
            }
            return new RequestResult<List<LeaveTypes>>(leavetypes);
        }

        public RequestResult<List<LeaveStatusActions>> GetManagerLeaveStatusActions()
        {
            var leavestatusactions = _leaveRepository.GetManagerLeaveStatusActions();
            if (leavestatusactions == null)
            {
                return new RequestResult<List<LeaveStatusActions>>();
            }
            return new RequestResult<List<LeaveStatusActions>>(leavestatusactions);
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
        private RequestResult<bool> CheckLeaveBalance(LeaveModel leave)
        {
            // Check if the LeaveType is "Leave of Absence" and return successful result without checking the leave balance
            if (leave.LeaveTypeID == LookupMapping.TypeToID[Lookup.LeaveOfAbsence])
            {
                return new RequestResult<bool>(true);
            }

            // Fetch the user's leave balance from the LeaveBalance table
            int leaveBalance = _leaveRepository.GetLeaveBalance(leave.UserID, leave.LeaveTypeID);

            // Check if the leave balance is sufficient
            if (leave.LeaveDays > leaveBalance)
            {
                return new RequestResult<bool>(false, new List<ValidationMessage> { new ValidationMessage { Reason = $"Insufficient {leave.LeaveType} Leave Balance", Severity = ValidationSeverity.Error } });
            }

            // Leave balance is sufficient, return successful result
            return new RequestResult<bool>(true);
        }

        // <summary>
        // To use the email Template to send mail to the Manager.
        private string CreateManagerBody(string emailTemplate)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Path.Combine(_hostingEnvironment.WebRootPath, _configuration["SmartdustAppLeave:ManagerTemplate"])))
            {
                body = reader.ReadToEnd();
            }
            return body;
        }
    }
}
