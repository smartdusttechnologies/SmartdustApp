using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Business.Services;
using SmartdustApp.DTO;
using SmartdustApp.Web.Models;
using System.Collections.Generic;

namespace SmartdustApp.Controllers
{
    // This class defines the API endpoints for leave management.
    [ApiController]
    [Route("api/[Controller]")]
    public class LeaveController : Controller
    {
        private readonly ILeaveService _leaveService;
        private readonly IMapper _mapper;

        // Constructor for LeaveController with dependency injection.
        public LeaveController(ILeaveService leaveService, IMapper mapper)
        {
            _leaveService = leaveService;
            _mapper = mapper;
        }

        // Retrieves a list of leave records for a specific user.
        [HttpGet]
        [Route("GetLeave/{userID}")]
        public IActionResult GetLeave(int userID)
        {
            var list = _leaveService.Get(userID);
            if (list.IsSuccessful)
            {
                return Ok(list);
            }

            List<ValidationMessage> errors = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Something Went Wrong", Severity = ValidationSeverity.Error, SourceId = "fields" }
                };
            return BadRequest(new RequestResult<bool>(errors));
        }

        // Applies for leave and returns the result.
        [HttpPost]
        [Route("ApplyLeave")]
        public IActionResult ApplyLeave(LeaveDTO leave)
        {
            var leaveModel = _mapper.Map<LeaveDTO, LeaveModel>(leave);
            //var leaveModel = new LeaveModel()
            //{
            //    UserID = leave.UserID,
            //    UserName = leave.UserName,
            //    LeaveTypeID = leave.LeaveTypeID,
            //    Reason = leave.Reason,
            //    LeaveStatus = leave.LeaveStatus,
            //    LeaveStatusID = leave.LeaveStatusID,
            //    LeaveDays = leave.LeaveDays,
            //    LeaveDates = leave.LeaveDates,
            //    AttachedFileIDs = leave.AttachedFileIDs,
            //};
            var result = _leaveService.Save(leaveModel);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Updates a leave record and returns the result.
        [HttpPost]
        [Route("UpdateLeave")]
        public IActionResult UpdateLeave(LeaveDTO leave)
        {
            var leaveModel = _mapper.Map<LeaveDTO, LeaveModel>(leave);
            //var leaveModel = new LeaveModel()
            //{
            //    ID = leave.ID,
            //    UserID = leave.UserID,
            //    UserName = leave.UserName,
            //    LeaveTypeID = leave.LeaveTypeID,
            //    Reason = leave.Reason,
            //    LeaveStatus = leave.LeaveStatus,
            //    LeaveStatusID = leave.LeaveStatusID,
            //    LeaveDays = leave.LeaveDays,
            //    LeaveDates = leave.LeaveDates,
            //    AttachedFileIDs = leave.AttachedFileIDs,
            //};
            var result = _leaveService.Update(leaveModel);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Retrieves a list of leave records for employees managed by a manager.
        [HttpGet]
        [Route("GetEmployeeLeave/{userID}")]
        public IActionResult GetEmployeeLeave(int userID)
        {
            var list = _leaveService.GetEmployeeLeave(userID);
            if (list.IsSuccessful)
            {
                return Ok(list);
            }

            List<ValidationMessage> errors = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Something Went Wrong", Severity = ValidationSeverity.Error, SourceId = "fields" }
                };
            return BadRequest(new RequestResult<bool>(errors));
        }

        // Updates the status of a leave record.
        [HttpPost]
        [Route("UpdateLeaveStatus")]
        public IActionResult UpdateLeaveStatus(UpdateLeaveStatusDTO updateStatus)
        {
            var updateStatusModel = _mapper.Map<UpdateLeaveStatusDTO, UpdateLeaveModel>(updateStatus);
            //var updateStatusModel = new UpdateLeaveModel()
            //{
            //    LeaveID = updateStatus.LeaveID,
            //    StatusID = updateStatus.StatusID,
            //    Comment = updateStatus.Comment,
            //};
            var result = _leaveService.UpdateLeaveStatus(updateStatusModel);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Retrieves a list of available leave types.
        [HttpGet]
        [Route("GetLeaveTypes")]
        public IActionResult GetLeaveTypes()
        {
            var list = _leaveService.GetLeaveTypes();
            if (list.IsSuccessful)
            {
                return Ok(list);
            }

            List<ValidationMessage> errors = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Something Went Wrong", Severity = ValidationSeverity.Error, SourceId = "fields" }
                };
            return BadRequest(new RequestResult<bool>(errors));
        }

        // Retrieves a list of leave status actions available to managers.
        [HttpGet]
        [Route("GetManagerLeaveStatusActions")]
        public IActionResult GetManagerLeaveStatusActions()
        {
            var list = _leaveService.GetManagerLeaveStatusActions();
            if (list.IsSuccessful)
            {
                return Ok(list);
            }

            List<ValidationMessage> errors = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Something Went Wrong", Severity = ValidationSeverity.Error, SourceId = "fields" }
                };
            return BadRequest(new RequestResult<bool>(errors));
        }

        [HttpGet]
        [Route("GetLeaveBalance/{userID}")]
        public IActionResult GetLeaveBalance(int userID)
        {
            var list = _leaveService.GetLeaveBalance(userID);
            if (list.IsSuccessful)
            {
                return Ok(list);
            }

            List<ValidationMessage> errors = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Something Went Wrong", Severity = ValidationSeverity.Error, SourceId = "fields" }
                };
            return BadRequest(new RequestResult<bool>(errors));
        }

        // Creates a leave balance record and returns the result.
        [HttpPost]
        [Route("CreateLeaveBalance")]
        public IActionResult CreateLeaveBalance(LeaveBalanceDTO leavebalance)
        {
            var leavebalanceModel = _mapper.Map<LeaveBalanceDTO, LeaveBalance>(leavebalance);
            //var leavebalanceModel = new LeaveBalance()
            //{
            //    UserID = leavebalance.UserID,
            //    UserName = leavebalance.UserName,
            //    LeaveType = leavebalance.LeaveType,
            //    Available = leavebalance.Available,
            //};
            var result = _leaveService.CreateLeaveBalance(leavebalanceModel);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Updates a leave balance record and returns the result.
        [HttpPost]
        [Route("UpdateLeaveBalance")]
        public IActionResult UpdateLeaveBalance(LeaveBalanceDTO leavebalance)
        {
            var leavebalanceModel = _mapper.Map<LeaveBalanceDTO, LeaveBalance>(leavebalance);
            //var leavebalanceModel = new LeaveBalance()
            //{
            //    ID = leavebalance.ID,
            //    UserID = leavebalance.UserID,
            //    UserName = leavebalance.UserName,
            //    LeaveType = leavebalance.LeaveType,
            //    Available = leavebalance.Available,
            //};
            var result = _leaveService.UpdateLeaveBalance(leavebalanceModel);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Deletes a leave balance record by its ID and returns the result.
        [HttpPost]
        [Route("DeleteLeaveBalance")]
        public IActionResult DeleteLeaveBalance(LeaveBalanceDTO leavebalance)
        {
            var result = _leaveService.DeleteLeaveBalance(leavebalance.ID);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        [Route("GetEmployeeDetails/{userID}")]
        public IActionResult GetEmployeeDetails(int userID)
        {
            var list = _leaveService.GetEmployeeDetails(userID);
            if (list.IsSuccessful)
            {
                return Ok(list);
            }

            List<ValidationMessage> errors = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Something Went Wrong", Severity = ValidationSeverity.Error, SourceId = "fields" }
                };
            return BadRequest(new RequestResult<bool>(errors));
        }

        [HttpGet]
        [Route("GetEmployeeLeaveBalance/{userID}")]
        public IActionResult GetEmployeeLeaveBalance(int userID)
        {
            var list = _leaveService.GetEmployeeLeaveBalance(userID);
            if (list.IsSuccessful)
            {
                return Ok(list);
            }

            List<ValidationMessage> errors = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Something Went Wrong", Severity = ValidationSeverity.Error, SourceId = "fields" }
                };
            return BadRequest(new RequestResult<bool>(errors));
        }
        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            var list = _leaveService.GetUsers();
            if (list.IsSuccessful)
            {
                return Ok(list);
            }

            List<ValidationMessage> errors = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Something Went Wrong", Severity = ValidationSeverity.Error, SourceId = "fields" }
                };
            return BadRequest(new RequestResult<bool>(errors));
        }

        // Retrieves a list of manager and employee data.
        [HttpGet]
        [Route("GetManagerAndEmployeeData")]
        public IActionResult GetManagerAndEmployeeData()
        {
            var list = _leaveService.GetManagerAndEmployeeData();
            if (list.IsSuccessful)
            {
                return Ok(list);
            }

            List<ValidationMessage> errors = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Something Went Wrong", Severity = ValidationSeverity.Error, SourceId = "fields" }
                };
            return BadRequest(new RequestResult<bool>(errors));
        }

        // Creates manager and employee data records and returns the result.
        [HttpPost]
        [Route("CreateManagerAndEmployeeData")]
        public IActionResult CreateManagerAndEmployeeData(EmployeeDTO employeeData)
        {
            var employeeModel = _mapper.Map<EmployeeDTO, EmployeeTable>(employeeData);
            //var employeeModel = new EmployeeTable()
            //{
            //    ID = employeeData.ID,
            //    ManagerID = employeeData.ManagerID,
            //    EmployeeID = employeeData.EmployeeID,
            //};
            var result = _leaveService.CreateManagerAndEmployeeData(employeeModel);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Edits manager and employee data records and returns the result.
        [HttpPost]
        [Route("EditManagerAndEmployeeData")]
        public IActionResult EditManagerAndEmployeeData(EmployeeDTO employeeData)
        {
            var employeeModel = _mapper.Map<EmployeeDTO, EmployeeTable>(employeeData);
            //var employeeModel = new EmployeeTable()
            //{
            //    ID = employeeData.ID,
            //    ManagerID = employeeData.ManagerID,
            //    EmployeeID = employeeData.EmployeeID,
            //};
            var result = _leaveService.EditManagerAndEmployeeData(employeeModel);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Deletes manager and employee data by ID and returns the result.
        [HttpPost]
        [Route("DeleteManagerAndEmployeeData")]
        public IActionResult DeleteManagerAndEmployeeData(EmployeeDTO employeeData)
        {
            var result = _leaveService.DeleteManagerAndEmployeeData(employeeData.ID);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
