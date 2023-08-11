﻿using Microsoft.AspNetCore.Mvc;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Business.Services;
using SmartdustApp.Web.Models;
using System.Collections.Generic;

namespace SmartdustApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class LeaveController : Controller
    {
        private readonly ILeaveService _leaveService;

        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

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
            return Json(new RequestResult<bool>(errors));
        }

        [HttpPost]
        [Route("ApplyLeave")]
        public IActionResult ApplyLeave(LeaveModel leave)
        {
            var result = _leaveService.Save(leave);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

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
            return Json(new RequestResult<bool>(errors));
        }

        [HttpPost]
        [Route("UpdateLeaveStatus")]
        public IActionResult UpdateLeaveStatus(UpdateLeaveModel updateStatus)
        {
            var result = _leaveService.UpdateLeaveStatus(updateStatus);
            if (result.IsSuccessful)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

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
            return Json(new RequestResult<bool>(errors));
        }

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
            return Json(new RequestResult<bool>(errors));
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
            return Json(new RequestResult<bool>(errors));
        }
        //[HttpPost]
        //[Route("UploadFile")]
        //public async Task<IActionResult> UploadFile(IFormFile file)
        //{
        //    try
        //    {
        //        if (file == null || file.Length == 0)
        //        {
        //            return BadRequest("No file uploaded.");
        //        }

        //        byte[] fileBytes;
        //        using (var ms = new MemoryStream())
        //        {
        //            await file.CopyToAsync(ms);
        //            fileBytes = ms.ToArray();
        //        }

        //        // Call your service method to process the uploaded file
        //        var result = _leaveService.UploadFile(fileBytes);

        //        if (result.IsSuccessful)
        //        {
        //            return Ok(result);
        //        }

        //        return BadRequest(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}
    }
}
