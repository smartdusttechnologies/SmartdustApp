using Microsoft.AspNetCore.Mvc;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Interfaces;

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
        [Route("GetLeave")]
        public IActionResult GetLeave()
        {
            var list = _leaveService.Get();
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
    }
}
