﻿using Microsoft.AspNetCore.Hosting;
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
            List<ValidationMessage> error = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Unable To take Your Request Right Now",Severity=ValidationSeverity.Information}
                };
            result.Message = error;
            return result;
        }
        public RequestResult<bool> Update(LeaveModel leave)
        {
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
            List<ValidationMessage> error = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Unable To take Your Request Right Now",Severity=ValidationSeverity.Information}
                };
            result.Message = error;
            return result;
        }
        public RequestResult<List<LeaveBalance>> GetEmployeeLeaveBalance(int userID)
        {
            var EmployeeLeaveBalance = _leaveRepository.GetEmployeeLeaveBalance(userID);
            if (EmployeeLeaveBalance == null)
            {
                return new RequestResult<List<LeaveBalance>>();
            }
            return new RequestResult<List<LeaveBalance>>(EmployeeLeaveBalance);
        }
        public RequestResult<bool> CreateLeaveBalance(LeaveBalance leavebalance)
        {
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
            List<ValidationMessage> error = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Unable To take Your Request Right Now",Severity=ValidationSeverity.Information}
                };
            result.Message = error;
            return result;
        }
        public RequestResult<bool> UpdateLeaveBalance(LeaveBalance leavebalance)
        {
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
            List<ValidationMessage> error = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Unable To take Your Request Right Now",Severity=ValidationSeverity.Information}
                };
            result.Message = error;
            return result;
        }
        public RequestResult<bool> DeleteLeaveBalance(int id)
        {
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
            List<ValidationMessage> error = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Unable To take Your Request Right Now",Severity=ValidationSeverity.Information}
                };
            result.Message = error;
            return result;
        }
        public RequestResult<List<EmployeeTable>> GetManagerAndEmployeeData()
        {
            var ManagerAndEmployeeData = _leaveRepository.GetManagerAndEmployeeData();
            if (ManagerAndEmployeeData == null)
            {
                return new RequestResult<List<EmployeeTable>>();
            }
            return new RequestResult<List<EmployeeTable>>(ManagerAndEmployeeData);
        }
        public RequestResult<List<UserModel>> GetUsers()
        {
            var UsersData = _leaveRepository.GetUsers();
            if (UsersData == null)
            {
                return new RequestResult<List<UserModel>>();
            }
            return new RequestResult<List<UserModel>>(UsersData);
        }
        public RequestResult<bool> CreateManagerAndEmployeeData(EmployeeTable employeeData)
        {
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
            List<ValidationMessage> error = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Unable To take Your Request Right Now",Severity=ValidationSeverity.Information}
                };
            result.Message = error;
            return result;
        }
        public RequestResult<bool> EditManagerAndEmployeeData(EmployeeTable employeeData)
        {
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
            List<ValidationMessage> error = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Unable To take Your Request Right Now",Severity=ValidationSeverity.Information}
                };
            result.Message = error;
            return result;
        }
        public RequestResult<bool> DeleteManagerAndEmployeeData(int id)
        {
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
        public RequestResult<List<UserModel>> GetEmployeeDetails(int userID)
        {
            var EmployeeDetails = _leaveRepository.GetEmployeeDetails(userID);
            if(EmployeeDetails == null)
            {
                return new RequestResult<List<UserModel>>();
            }
            return new RequestResult<List<UserModel>>(EmployeeDetails);
        }
        public RequestResult<bool> UpdateLeaveStatus(UpdateLeaveModel updateStatus)
        {
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

            var result = _leaveRepository.UpdateLeaveStatus(updateStatus.LeaveID , updateStatus.StatusID);

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

            //var currentDate = DateTime.Today;
            //var distinctDates = new HashSet<DateTime>();

            //foreach (var date in leave.LeaveDates)
            //{
            //    if (date.Date < currentDate)
            //    {
            //        List<ValidationMessage> validationMessages = new List<ValidationMessage>()
            //        {
            //            new ValidationMessage() { Reason = $"Leave Date {date.Date.ToShortDateString()} cannot be before the current date.", Severity = ValidationSeverity.Error }
            //        };
            //        return new RequestResult<bool>(false, validationMessages);
            //    }

            //    if (!distinctDates.Add(date.Date))
            //    {
            //        List<ValidationMessage> validationMessages = new List<ValidationMessage>()
            //        {
            //            new ValidationMessage() { Reason = $"Leave Date {date.Date.ToShortDateString()} is duplicated.", Severity = ValidationSeverity.Error }
            //        };
            //        return new RequestResult<bool>(false, validationMessages);
            //    }
            //}

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
        // For Documnent Controller
        public List<int> UploadFiles(IFormFileCollection files)
        {
            List<int> uploadedFileIds = new List<int>();

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    var uploadedFileId = UploadSingleFile(file);
                    uploadedFileIds.Add(uploadedFileId);
                }
            }

            return uploadedFileIds;
        }

        private int UploadSingleFile(IFormFile file)
        {
            var newFileName = GenerateUniqueFileName(file.FileName);
            var fileModel = new DocumentModel
            {
                Name = newFileName,
                FileType = Path.GetExtension(newFileName)
            };
            // Validate file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".xlsx", ".pdf" };
            if (!allowedExtensions.Contains(fileModel.FileType.ToLower()))
            {
                throw new InvalidOperationException("Unsupported file type.");
            }

            // Validate file size
            if (file.Length > 1024 * 1024) // 1 MB
            {
                throw new InvalidOperationException("File size should not exceed 1MB.");
            }
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileModel.DataFiles = memoryStream.ToArray();
            }

            return _leaveRepository.FileUpload(fileModel);
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            var fileExtension = Path.GetExtension(originalFileName);
            return $"{Guid.NewGuid()}{fileExtension}";
        }

        /// <summary>
        /// Method To download Document 
        /// </summary>
        public DocumentModel DownloadDocument(int documentID)
        {
            var document = _leaveRepository.DownloadDocument(documentID);
            return document;

        }
    }
}
