using Microsoft.AspNetCore.Mvc;
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
    }
}
