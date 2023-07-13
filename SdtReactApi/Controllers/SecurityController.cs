using Microsoft.AspNetCore.Mvc;
using SmartdustApp.Web.Models;
using SmartdustApp.DTO;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Business.Common;
using AutoMapper;
using SmartdustApp.Business.Core.Interfaces;

namespace SmartdustApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        public SecurityController(IAuthenticationService authenticationService,IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp(UserDTO user)
        {
            var userModel = _mapper.Map<UserDTO,UserModel>(user);
           
            RequestResult<bool> result = _authenticationService.Add(userModel);
            if (result.IsSuccessful)
            {
                List<ValidationMessage> success = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Sign Up Successfully", Severity = ValidationSeverity.Information, SourceId = "fields" }
                };
                result.Message = success;
                return Json(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Method to get the Login details from UI and Process Login.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginDTO loginRequest)
        {
            var loginReq = new LoginRequest { UserName = loginRequest.UserName, Password = loginRequest.Password };
            RequestResult<LoginToken> result = _authenticationService.Login(loginReq);
            if (result.IsSuccessful)
            {
                List<ValidationMessage> success = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Login Successfully", Severity = ValidationSeverity.Information, SourceId = "fields" }
                };
                result.Message = success;
                return Json(result);
            }
            return BadRequest(result);
        }
        [HttpPost]
        [Route("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordDTO changepasswordDTO)
        {
            if (ModelState.IsValid)
            {
                var changepasswordRequest = new ChangePasswordModel()
                {
                    OldPassword = changepasswordDTO.OldPassword,
                    NewPassword = changepasswordDTO.NewPassword,
                    ConfirmPassword = changepasswordDTO.ConfirmPassword,
                    Username = changepasswordDTO.Username,
                    UserId = changepasswordDTO.UserId,
                };
                var result = _authenticationService.UpdatePaasword(changepasswordRequest);
                if (result.IsSuccessful)
                {
                    List<ValidationMessage> success = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Password Changed Successfully", Severity = ValidationSeverity.Information, SourceId = "fields" }
                };
                    result.Message = success;
                    return Ok(result);
                }
                return BadRequest(result);
            }
            else
            {
                List<ValidationMessage> errors = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "All Fields Are Required", Severity = ValidationSeverity.Error, SourceId = "fields" }
                };
                return Json(new RequestResult<bool>(errors));
            }
        }
    }
}
