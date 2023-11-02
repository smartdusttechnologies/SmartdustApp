using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using static SmartdustApp.Business.Core.Model.PolicyTypes;
using IAuthorizationService = SmartdustApp.Business.Core.Interfaces.IAuthorizationService;

namespace SmartdustApp.Business.Services
{
    // This class implements the ILeaveService interface and provides leave management services.
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationService _authorizationService;

        // Constructor for LeaveService with dependency injection.
        public LeaveService(ILeaveRepository leaveRepository, IEmailService emailservice, IConfiguration configuration , IWebHostEnvironment hostingEnvironment, IUserRepository userRepository,IHttpContextAccessor httpContextAccessor,IAuthorizationService authorizationService)
        {
            _leaveRepository = leaveRepository;
            _emailService = emailservice;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _userRepository = userRepository;
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        // Retrieves a list of leave records for a specific user.
        public RequestResult<List<LeaveModel>> Get(int userID)
        {
            if (!_authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, new LeaveModel(), new[] { Operations.Read }).Result)
            {
                throw new UnauthorizedAccessException("You're Unauthorized");
            }
            var leave = _leaveRepository.Get(userID);
            if (leave == null)
            {
                return new RequestResult<List<LeaveModel>>();
            }
            return new RequestResult<List<LeaveModel>>(leave);
        }

        // Saves a leave record and sends an email notification.
        public RequestResult<bool> Save(LeaveModel leave)
        {
            if (!_authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, leave, new[] { Operations.Create }).Result)
            {
                throw new UnauthorizedAccessException("You're Unauthorized");
            }
            var validationResult = ValidateLeaveDate(leave);
            if (!validationResult.IsSuccessful)
            {
                return validationResult;
            }

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
            return result;
        }
        // Updates a leave record.
        public RequestResult<bool> Update(LeaveModel leave)
        {
            if (!_authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, leave, new[] { Operations.Update }).Result)
            {
                throw new UnauthorizedAccessException("You're Unauthorized");
            }
            var validationResult = ValidateLeaveDate(leave);
            if (!validationResult.IsSuccessful)
            {
                return validationResult;
            }
            var result = _leaveRepository.Update(leave);

            if (result.IsSuccessful)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Leave Updated Successfully",Severity=ValidationSeverity.Information}
                };
                result.Message = success;
                return result;
            }
            return result;
        }

        // Retrieves leave balance records for employees managed by a manager.
        public RequestResult<List<LeaveBalance>> GetEmployeeLeaveBalance(int userID)
        {
            if (!_authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, new LeaveBalance(), new[] { Operations.Read }).Result)
            {
                throw new UnauthorizedAccessException("You're Unauthorized");
            }
            var EmployeeLeaveBalance = _leaveRepository.GetEmployeeLeaveBalance(userID);
            if (EmployeeLeaveBalance == null)
            {
                return new RequestResult<List<LeaveBalance>>();
            }
            return new RequestResult<List<LeaveBalance>>(EmployeeLeaveBalance);
        }
        // Creates a leave balance record.
        public RequestResult<bool> CreateLeaveBalance(LeaveBalance leavebalance)
        {
            if (!_authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, leavebalance, new[] { Operations.Create }).Result)
            {
                throw new UnauthorizedAccessException("You're Unauthorized");
            }
            var result = _leaveRepository.CreateLeaveBalance(leavebalance);

            if (result.IsSuccessful)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Created Leave Balance Successfully",Severity=ValidationSeverity.Information}
                };
                result.Message = success;
                return result;
            }
            return result;
        }

        // Updates a leave balance record.
        public RequestResult<bool> UpdateLeaveBalance(LeaveBalance leavebalance)
        {
            if (!_authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, leavebalance, new[] { Operations.Update }).Result)
            {
                throw new UnauthorizedAccessException("You're Unauthorized");
            }
            var result = _leaveRepository.UpdateLeaveBalance(leavebalance);

            if (result.IsSuccessful)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Updated Leave Balance Successfully",Severity=ValidationSeverity.Information}
                };
                result.Message = success;
                return result;
            }
            return result;
        }

        // Deletes a leave balance record.
        public RequestResult<bool> DeleteLeaveBalance(int id)
        {
            if (!_authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, new LeaveBalance(), new[] { Operations.Delete }).Result)
            {
                throw new UnauthorizedAccessException("You're Unauthorized");
            }
            var result = _leaveRepository.DeleteLeaveBalance(id);

            if (result.IsSuccessful)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Deleted Leave Balance Successfully",Severity=ValidationSeverity.Information}
                };
                result.Message = success;
                return result;
            }
            return result;
        }

        // Retrieves a list of manager and employee data.
        public RequestResult<List<EmployeeTable>> GetManagerAndEmployeeData()
        {
            if (!_authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, new EmployeeTable(), new[] { Operations.Read }).Result)
            {
                throw new UnauthorizedAccessException("You're Unauthorized");
            }
            var ManagerAndEmployeeData = _leaveRepository.GetManagerAndEmployeeData();
            if (ManagerAndEmployeeData == null)
            {
                return new RequestResult<List<EmployeeTable>>();
            }
            return new RequestResult<List<EmployeeTable>>(ManagerAndEmployeeData);
        }

        // Retrieves a list of all users.
        public RequestResult<List<UserModel>> GetUsers()
        {
            var UsersData = _leaveRepository.GetUsers();
            if (UsersData == null)
            {
                return new RequestResult<List<UserModel>>();
            }
            return new RequestResult<List<UserModel>>(UsersData);
        }
        // Creates manager and employee data records.
        public RequestResult<bool> CreateManagerAndEmployeeData(EmployeeTable employeeData)
        {
            if (!_authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, employeeData, new[] { Operations.Create }).Result)
            {
                throw new UnauthorizedAccessException("You're Unauthorized");
            }
            var result = _leaveRepository.CreateManagerAndEmployeeData(employeeData);

            if (result.IsSuccessful)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Created Successfully",Severity=ValidationSeverity.Information}
                };
                result.Message = success;
                return result;
            }
            return result;
        }
        // Edits manager and employee data records.
        public RequestResult<bool> EditManagerAndEmployeeData(EmployeeTable employeeData)
        {
            if (!_authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, employeeData, new[] { Operations.Update }).Result)
            {
                throw new UnauthorizedAccessException("You're Unauthorized");
            }
            var result = _leaveRepository.EditManagerAndEmployeeData(employeeData);

            if (result.IsSuccessful)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Edited Successfully",Severity=ValidationSeverity.Information}
                };
                result.Message = success;
                return result;
            }
            return result;
        }
        // Deletes manager and employee data by ID.
        public RequestResult<bool> DeleteManagerAndEmployeeData(int id)
        {
            if (!_authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, new EmployeeTable(), new[] { Operations.Delete }).Result)
            {
                throw new UnauthorizedAccessException("You're Unauthorized");
            }
            var result = _leaveRepository.DeleteManagerAndEmployeeData(id);

            if (result.IsSuccessful)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Deleted Successfully",Severity=ValidationSeverity.Information}
                };
                result.Message = success;
                return result;
            }
            return result;
        }

        // Retrieves a list of leave records for employees managed by a manager.
        public RequestResult<List<LeaveModel>> GetEmployeeLeave(int userID)
        {
            if (!_authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, new UpdateLeaveModel(), new[] { Operations.Read }).Result)
            {
                throw new UnauthorizedAccessException("You're Unauthorized");
            }
            var leave = _leaveRepository.GetEmployeeLeave(userID);
            if (leave == null)
            {
                return new RequestResult<List<LeaveModel>>();
            }
            return new RequestResult<List<LeaveModel>>(leave);
        }

        // Retrieves employee details.
        public RequestResult<List<UserModel>> GetEmployeeDetails(int userID)
        {
            var EmployeeDetails = _leaveRepository.GetEmployeeDetails(userID);
            if(EmployeeDetails == null)
            {
                return new RequestResult<List<UserModel>>();
            }
            return new RequestResult<List<UserModel>>(EmployeeDetails);
        }

        // Updates the status of a leave record and sends an email notification.
        public RequestResult<bool> UpdateLeaveStatus(UpdateLeaveModel updateStatus)
        {
            if (!_authorizationService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, updateStatus, new[] { Operations.Update }).Result)
            {
                throw new UnauthorizedAccessException("You're Unauthorized");
            }
            var leave = _leaveRepository.GetLeaveDetails(updateStatus.LeaveID);
            var employee = _userRepository.Get(leave.UserID);

            // Create the email model and replace placeholders with data
            EmailModel model = new EmailModel();
            model.EmailTemplate = _configuration["SmartdustAppLeave:EmployeeTemplate"];
            model.Subject = "Updated Leave Status";

            model.HtmlMsg = CreateEmployeemailBody(model.EmailTemplate);
            model.HtmlMsg = model.HtmlMsg.Replace("*EmployeeName*", employee.UserName);
            //model.HtmlMsg = model.HtmlMsg.Replace("*Status*", employee.UserName);
            model.HtmlMsg = model.HtmlMsg.Replace("*LeaveID*", leave.LeaveTypeID.ToString());
            model.HtmlMsg = model.HtmlMsg.Replace("*LeaveDays*", leave.LeaveDays.ToString());

            model.Email = new List<string>();
            model.Email.Add(employee.Email);

            if (updateStatus.StatusID == (int)Lookup.Approve && leave.LeaveTypeID != (int)Lookup.LeaveOfAbsence)
            {
                _leaveRepository.UpdateLeaveBalance(updateStatus.LeaveID);
            }

            var result = _leaveRepository.UpdateLeaveStatus(updateStatus.LeaveID , updateStatus.StatusID , updateStatus.Comment);

            // Send the email to Employee
            var isEmailSendSuccessfully = _emailService.Sendemail(model);

            if (result.IsSuccessful && isEmailSendSuccessfully.IsSuccessful)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Updated Successfully",Severity=ValidationSeverity.Information}
                };
                result.Message = success;
                return result;
            }
            return result;
        }

        // Retrieves a list of available leave types.
        public RequestResult<List<LeaveTypes>> GetLeaveTypes()
        {
            var leavetypes = _leaveRepository.GetLeaveTypes();
            if (leavetypes == null)
            {
                return new RequestResult<List<LeaveTypes>>();
            }
            return new RequestResult<List<LeaveTypes>>(leavetypes);
        }

        // Retrieves a list of leave status actions available to managers.
        public RequestResult<List<LeaveStatusActions>> GetManagerLeaveStatusActions()
        {
            var leavestatusactions = _leaveRepository.GetManagerLeaveStatusActions();
            if (leavestatusactions == null)
            {
                return new RequestResult<List<LeaveStatusActions>>();
            }
            return new RequestResult<List<LeaveStatusActions>>(leavestatusactions);
        }

        // Retrieves leave balance for a specific user.
        public RequestResult<List<LeaveBalance>> GetLeaveBalance(int userID)
        {
            var leaveBalance = _leaveRepository.GetLeaveBalance(userID);
            if (leaveBalance == null)
            {
                return new RequestResult<List<LeaveBalance>>();
            }
            return new RequestResult<List<LeaveBalance>>(leaveBalance);
        }

        // To Check the Leave Balance before Applying Leave.
        private RequestResult<bool> CheckLeaveBalance(LeaveModel leave)
        {
            // Check if the LeaveType is "Leave of Absence" and return successful result without checking the leave balance
            if (leave.LeaveTypeID == (int)Lookup.LeaveOfAbsence)
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
        // To Validate Leave Dates.
        private RequestResult<bool> ValidateLeaveDate(LeaveModel leave)
        {
            if (leave.LeaveDates == null || leave.LeaveDates.Count == 0)
            {
                List<ValidationMessage> validationMessages = new List<ValidationMessage>()
                {
                    new ValidationMessage() { Reason = "No leave dates are selected.", Severity = ValidationSeverity.Error }
                };
                return new RequestResult<bool>(false, validationMessages);
            }
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
        // <summary>
        // To use the email Template to send mail to the Employee.
        private string CreateEmployeemailBody(string emailTemplate)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Path.Combine(_hostingEnvironment.WebRootPath, _configuration["SmartdustAppLeave:EmployeeTemplate"])))
            {
                body = reader.ReadToEnd();
            }
            return body;
        }
    }
}
