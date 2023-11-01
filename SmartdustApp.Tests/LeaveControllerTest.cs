using Moq;
using FluentAssertions;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Business.Data.Repository.Interfaces;
using SmartdustApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TestingAndCalibrationLabs.Business.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace SmartdustApp.Tests
{
    [TestFixture]
    public class LeaveControllerTest
    {

        ILeaveService? _leaveService;
        IEmailService? _emailService;
        IConfiguration? _configuration;
        IWebHostEnvironment? _hostingEnvironment;
        IUserRepository? _userRepository;
        IHttpContextAccessor? _httpContextAccessor;
        Business.Core.Interfaces.IAuthorizationService? _authorizationService;
        Mock<ILeaveRepository> _leaveRepository = new Mock<ILeaveRepository>();

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetLeave_Test()
        {
            var userId = 1;
            var leaveModels = new List<LeaveModel>
            {
                // Create some LeaveModel instances for testing
                new LeaveModel { ID= 52, UserID= 5, UserName= "YashRaj" , LeaveType = "MedicalLeave", LeaveTypeID= 1, Reason="aa", AppliedDate = new DateTime(), LeaveStatus="null", LeaveStatusID=1 , LeaveDays=3 ,LeaveDates = new List<DateTime> { DateTime.Now.Date,DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(2), },AttachedFileIDs = new List<int>{1,2}},
                new LeaveModel { ID= 42, UserID= 5, UserName= "YashRaj", LeaveType = "MedicalLeave", LeaveTypeID= 1, Reason="aa" , AppliedDate = new DateTime(), LeaveStatus="null", LeaveStatusID=1 , LeaveDays=3 ,LeaveDates = new List<DateTime> { DateTime.Now.Date,DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(2), },AttachedFileIDs = new List<int>{1,2}},
            };


            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),  
                
            }, "mock"));

            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, It.IsAny<LeaveModel>(), new[] { Operations.Read }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);
            // Configure the _leaveRepository mock to return the leaveModels when Get is called
            _leaveRepository.Setup(repo => repo.Get(userId)).Returns(leaveModels);

            // Create an instance of the LeaveController
            var leaveController = new LeaveController(_leaveService);

            // Act
            var result = leaveController.GetLeave(userId);

            // Assert
            // Perform assertions to verify the result matches the expected outcome
            Assert.IsNotNull(result);
            //Assert.IsInstanceOf<List<LeaveModel>>(result);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeOfType<RequestResult<List<LeaveModel>>>();
            var requestResult = (RequestResult<List<LeaveModel>>)okResult.Value;

            requestResult.RequestedObject.Should().BeEquivalentTo(leaveModels);
        }

        [Test]
        public void GetLeave_ReturnsBadRequest()
        {
            var userId = 1;
            List<LeaveModel> leaveModels = null;


            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),  
                
            }, "mock"));

            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, It.IsAny<LeaveModel>(), new[] { Operations.Read }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);
            // Configure the _leaveRepository mock to return the leaveModels when Get is called
            _leaveRepository.Setup(repo => repo.Get(userId)).Returns(leaveModels);

            // Create an instance of the LeaveController
            var leaveController = new LeaveController(_leaveService);

            // Act
            var result = leaveController.GetLeave(userId);


            result.Should().BeOfType<BadRequestObjectResult>();
        }


        [Test]
        public void ApplyLeave_Test()
            {
            // Arrange
            var leaveModel = new LeaveModel { ID = 52, UserID = 5, UserName = "YashRaj", LeaveType = "LeaveOfAbsence", LeaveTypeID = 3, Reason = "aa", AppliedDate = DateTime.Now.Date, LeaveStatus = "null", LeaveStatusID = 1, LeaveDays = 3, LeaveDates = new List<DateTime> { DateTime.Now.Date, DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(2), }, AttachedFileIDs = new List<int> { 1, 2 } };
            var successfulResult = new RequestResult<bool>(true); // Simulate a successful result

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),  
                
            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, leaveModel, new[] { Operations.Create }))
                  .ReturnsAsync(true);


            var userRepositoryMock = new Mock<IUserRepository>();
            var user = new UserModel
            {
                Id = 5,
                UserName = "TestUser",
                FirstName = "John",
                LastName = "Doe",
                Email = "testuser@example.com",
                Mobile = "+1234567890",
                Country = "United States",
                ISDCode = "+1",
                TwoFactor = true,
                Locked = false,
                IsActive = true,
                EmailValidationStatus = 1,
                MobileValidationStatus = 1,
                OrgId = 1,
                AdminLevel = 1,
                Password = "Password123",
                NewPassword = "NewPassword456"
            };
            userRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).Returns(user);

            var emailServiceMock = new Mock<IEmailService>();
            var isEmailSendSuccessfully = true; // Set the desired result

            emailServiceMock
                .Setup(x => x.Sendemail(It.IsAny<EmailModel>()))
                .Returns(new RequestResult<bool>(isEmailSendSuccessfully));
            // Mock the IConfiguration
            var configurationMock = new Mock<IConfiguration>();
            var managerTemplate = "emailformat/LeaveManagerMailTemplate.html";

            // Configure the IConfiguration mock to return the manager template value
            configurationMock.Setup(config => config["SmartdustAppLeave:ManagerTemplate"]).Returns(managerTemplate);


            // Mock the IWebHostEnvironment
            var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Configure the WebRootPath property to return a specific path for your test
            hostingEnvironmentMock.Setup(env => env.WebRootPath).Returns("C:\\Users\\HP\\Desktop\\SmartdustAppmerged\\SmartdustApp\\SdtReactApi\\wwwroot");


            _leaveService = new LeaveService(_leaveRepository.Object, emailServiceMock.Object, configurationMock.Object, hostingEnvironmentMock.Object, userRepositoryMock.Object, httpContextAccessorMock.Object,authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.Save(leaveModel)).Returns(successfulResult);

            var controller = new LeaveController(_leaveService);

            // Act
            var result = controller.ApplyLeave(leaveModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void ApplyLeave_ReturnsBadRequest_WhenSaveFails()
            {
            // Arrange
            var leaveModel = new LeaveModel { ID = 52, UserID = 5, UserName = "YashRaj", LeaveType = "LeaveOfAbsence", LeaveTypeID = 3, Reason = "aa", AppliedDate = DateTime.Now.Date, LeaveStatus = "null", LeaveStatusID = 1, LeaveDays = 3, LeaveDates = new List<DateTime> { DateTime.Now.Date, DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(2), }, AttachedFileIDs = new List<int> { 1, 2 } };

            List<ValidationMessage> validationMessages = new List<ValidationMessage>()
                {
                    new ValidationMessage() { Reason = "Not Successful.", Severity = ValidationSeverity.Error }
                };
            var failedResult = new RequestResult<bool>(false, validationMessages);

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),  
                
            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, leaveModel, new[] { Operations.Create }))
                  .ReturnsAsync(true);


            var userRepositoryMock = new Mock<IUserRepository>();
            var user = new UserModel
            {
                Id = 5,
                UserName = "TestUser",
                FirstName = "John",
                LastName = "Doe",
                Email = "testuser@example.com",
                Mobile = "+1234567890",
                Country = "United States",
                ISDCode = "+1",
                TwoFactor = true,
                Locked = false,
                IsActive = true,
                EmailValidationStatus = 1,
                MobileValidationStatus = 1,
                OrgId = 1,
                AdminLevel = 1,
                Password = "Password123",
                NewPassword = "NewPassword456"
            };
            userRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).Returns(user);

            var emailServiceMock = new Mock<IEmailService>();
            var isEmailSendSuccessfully = false;

            emailServiceMock
                .Setup(x => x.Sendemail(It.IsAny<EmailModel>()))
                .Returns(new RequestResult<bool>(isEmailSendSuccessfully));
            // Mock the IConfiguration
            var configurationMock = new Mock<IConfiguration>();
            var managerTemplate = "emailformat/LeaveManagerMailTemplate.html";

            // Configure the IConfiguration mock to return the manager template value
            configurationMock.Setup(config => config["SmartdustAppLeave:ManagerTemplate"]).Returns(managerTemplate);


            // Mock the IWebHostEnvironment
            var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Configure the WebRootPath property to return a specific path for your test
            hostingEnvironmentMock.Setup(env => env.WebRootPath).Returns("C:\\Users\\HP\\Desktop\\SmartdustAppmerged\\SmartdustApp\\SdtReactApi\\wwwroot");


            _leaveService = new LeaveService(_leaveRepository.Object, emailServiceMock.Object, configurationMock.Object, hostingEnvironmentMock.Object, userRepositoryMock.Object, httpContextAccessorMock.Object,authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.Save(leaveModel)).Returns(failedResult);

            var controller = new LeaveController(_leaveService);

            // Act
            var result = controller.ApplyLeave(leaveModel);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void UpdateLeave_Test()
        {
            // Arrange
            var leaveModel = new LeaveModel { ID = 52, UserID = 5, UserName = null, LeaveType = "MedicalLeave", LeaveTypeID = 1, Reason = "aa", AppliedDate = new DateTime(), LeaveStatus = "null", LeaveStatusID = 1, LeaveDays = 3, LeaveDates = new List<DateTime> { DateTime.Now.Date, DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(2), }, AttachedFileIDs = new List<int> { 1, 2 } };
            var successfulResult = new RequestResult<bool>(true); // Simulate a successful result

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),  
                
            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, leaveModel, new[] { Operations.Update }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.Update(leaveModel)).Returns(successfulResult);

            var controller = new LeaveController(_leaveService);

            // Act
            var result = controller.UpdateLeave(leaveModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void UpdateLeave_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var leaveModel = new LeaveModel { ID = 52, UserID = 5, UserName = null, LeaveType = "MedicalLeave", LeaveTypeID = 1, Reason = "aa", AppliedDate = new DateTime(), LeaveStatus = "null", LeaveStatusID = 1, LeaveDays = 3, LeaveDates = new List<DateTime> { DateTime.Now.Date, DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(2), }, AttachedFileIDs = new List<int> { 1, 2 } };

            List<ValidationMessage> validationMessages = new List<ValidationMessage>()
                {
                    new ValidationMessage() { Reason = "Not Successful.", Severity = ValidationSeverity.Error }
                };
            var failedResult = new RequestResult<bool>(false, validationMessages);

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),  
                
            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, leaveModel, new[] { Operations.Update }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.Update(leaveModel)).Returns(failedResult);

            var controller = new LeaveController(_leaveService);

            // Act
            var result = controller.UpdateLeave(leaveModel);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void UpdateLeaveStatus_Test()
        {
            // Arrange
            var updateStatus = new UpdateLeaveModel { LeaveID=1 , StatusID = 5 , Comment="aa" };
            var successfulResult = new RequestResult<bool>(true);

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, updateStatus, new[] { Operations.Update }))
                  .ReturnsAsync(true);

            var userRepositoryMock = new Mock<IUserRepository>();
            var user = new UserModel
            {
                Id = 5,
                UserName = "TestUser",
                FirstName = "John",
                LastName = "Doe",
                Email = "testuser@example.com",
                Mobile = "+1234567890",
                Country = "United States",
                ISDCode = "+1",
                TwoFactor = true,
                Locked = false,
                IsActive = true,
                EmailValidationStatus = 1,
                MobileValidationStatus = 1,
                OrgId = 1,
                AdminLevel = 1,
                Password = "Password123",
                NewPassword = "NewPassword456"
            };
            userRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).Returns(user);

            var emailServiceMock = new Mock<IEmailService>();
            var isEmailSendSuccessfully = true; // Set the desired result

            emailServiceMock
                .Setup(x => x.Sendemail(It.IsAny<EmailModel>()))
                .Returns(new RequestResult<bool>(isEmailSendSuccessfully));
            // Mock the IConfiguration
            var configurationMock = new Mock<IConfiguration>();
            var managerTemplate = "emailformat/LeaveManagerMailTemplate.html";

            // Configure the IConfiguration mock to return the manager template value
            configurationMock.Setup(config => config["SmartdustAppLeave:EmployeeTemplate"]).Returns(managerTemplate);


            // Mock the IWebHostEnvironment
            var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Configure the WebRootPath property to return a specific path for your test
            hostingEnvironmentMock.Setup(env => env.WebRootPath).Returns("C:\\Users\\HP\\Desktop\\SmartdustAppmerged\\SmartdustApp\\SdtReactApi\\wwwroot");

            _leaveService = new LeaveService(_leaveRepository.Object, emailServiceMock.Object, configurationMock.Object, hostingEnvironmentMock.Object, userRepositoryMock.Object, httpContextAccessorMock.Object, authorizationServiceMock.Object);
            _leaveRepository.Setup(x => x.GetLeaveDetails(It.IsAny<int>())).Returns(new LeaveModel { ID = 52, UserID = 5, UserName = "YashRaj", LeaveType = "MedicalLeave", LeaveTypeID = 1, Reason = "aa", AppliedDate = new DateTime(), LeaveStatus = "null", LeaveStatusID = 1, LeaveDays = 3, LeaveDates = new List<DateTime> { DateTime.Now.Date, DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(2), }, AttachedFileIDs = new List<int> { 1, 2 } });
            _leaveRepository.Setup(x => x.UpdateLeaveStatus(updateStatus.LeaveID , updateStatus.StatusID)).Returns(successfulResult);

            var controller = new LeaveController(_leaveService);

            // Act
            var result = controller.UpdateLeaveStatus(updateStatus);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void UpdateLeaveStatus_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var updateStatus = new UpdateLeaveModel { LeaveID=1 , StatusID = 5 , Comment="aa" };

            List<ValidationMessage> validationMessages = new List<ValidationMessage>()
                {
                    new ValidationMessage() { Reason = "Not Successful.", Severity = ValidationSeverity.Error }
                };
            var failedResult = new RequestResult<bool>(false, validationMessages);


            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, updateStatus, new[] { Operations.Update }))
                  .ReturnsAsync(true);

            var userRepositoryMock = new Mock<IUserRepository>();
            var user = new UserModel
            {
                Id = 5,
                UserName = "TestUser",
                FirstName = "John",
                LastName = "Doe",
                Email = "testuser@example.com",
                Mobile = "+1234567890",
                Country = "United States",
                ISDCode = "+1",
                TwoFactor = true,
                Locked = false,
                IsActive = true,
                EmailValidationStatus = 1,
                MobileValidationStatus = 1,
                OrgId = 1,
                AdminLevel = 1,
                Password = "Password123",
                NewPassword = "NewPassword456"
            };
            userRepositoryMock.Setup(repo => repo.Get(It.IsAny<int>())).Returns(user);

            var emailServiceMock = new Mock<IEmailService>();
            var isEmailSendSuccessfully = false;

            emailServiceMock
                .Setup(x => x.Sendemail(It.IsAny<EmailModel>()))
                .Returns(new RequestResult<bool>(isEmailSendSuccessfully));
            // Mock the IConfiguration
            var configurationMock = new Mock<IConfiguration>();
            var managerTemplate = "emailformat/LeaveManagerMailTemplate.html";

            // Configure the IConfiguration mock to return the manager template value
            configurationMock.Setup(config => config["SmartdustAppLeave:EmployeeTemplate"]).Returns(managerTemplate);


            // Mock the IWebHostEnvironment
            var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Configure the WebRootPath property to return a specific path for your test
            hostingEnvironmentMock.Setup(env => env.WebRootPath).Returns("C:\\Users\\HP\\Desktop\\SmartdustAppmerged\\SmartdustApp\\SdtReactApi\\wwwroot");

            _leaveService = new LeaveService(_leaveRepository.Object, emailServiceMock.Object, configurationMock.Object, hostingEnvironmentMock.Object, userRepositoryMock.Object, httpContextAccessorMock.Object, authorizationServiceMock.Object);
            _leaveRepository.Setup(x => x.GetLeaveDetails(It.IsAny<int>())).Returns(new LeaveModel { ID = 52, UserID = 5, UserName = "YashRaj", LeaveType = "MedicalLeave", LeaveTypeID = 1, Reason = "aa", AppliedDate = new DateTime(), LeaveStatus = "null", LeaveStatusID = 1, LeaveDays = 3, LeaveDates = new List<DateTime> { DateTime.Now.Date, DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(2), }, AttachedFileIDs = new List<int> { 1, 2 } });
            _leaveRepository.Setup(x => x.UpdateLeaveStatus(updateStatus.LeaveID , updateStatus.StatusID)).Returns(failedResult);

            var controller = new LeaveController(_leaveService);

            // Act
            var result = controller.UpdateLeaveStatus(updateStatus);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public void GetLeaveTypes_Test()
        {
            var leaveTypesModels = new List<LeaveTypes>
            {
                // Create some LeaveModel instances for testing
                new LeaveTypes {ID = 2 , Name = "MedicalLeave"},
                new LeaveTypes {ID = 3 , Name = "Leave"},
            };

            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, _httpContextAccessor, _authorizationService);

            // Configure the _leaveRepository mock to return the leaveModels when Get is called
            _leaveRepository.Setup(repo => repo.GetLeaveTypes()).Returns(leaveTypesModels);

            // Create an instance of the LeaveController
            var leaveController = new LeaveController(_leaveService);

            // Act
            var result = leaveController.GetLeaveTypes();


            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeOfType<RequestResult<List<LeaveTypes>>>();
            var requestResult = (RequestResult<List<LeaveTypes>>)okResult.Value;

            requestResult.RequestedObject.Should().BeEquivalentTo(leaveTypesModels);

        }
        [Test]
        public void GetManagerLeaveStatusActions_Test()
        {
            var leavestatusModels = new List<LeaveStatusActions>
            {
                // Create some LeaveModel instances for testing
                new LeaveStatusActions {ID = 2 , Name = "Approve"},
                new LeaveStatusActions {ID = 3 , Name = "Decline"},
            };

            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, _httpContextAccessor, _authorizationService);

            // Configure the _leaveRepository mock to return the leaveModels when Get is called
            _leaveRepository.Setup(repo => repo.GetManagerLeaveStatusActions()).Returns(leavestatusModels);

            // Create an instance of the LeaveController
            var leaveController = new LeaveController(_leaveService);

            // Act
            var result = leaveController.GetManagerLeaveStatusActions();


            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeOfType<RequestResult<List<LeaveStatusActions>>>();
            var requestResult = (RequestResult<List<LeaveStatusActions>>)okResult.Value;

            requestResult.RequestedObject.Should().BeEquivalentTo(leavestatusModels);

        }
        [Test]
        public void GetLeaveBalance_Test()
        {
            var leavebalanceModels = new List<LeaveBalance>
            {
                // Create some LeaveModel instances for testing
                new LeaveBalance
                {
                    ID = 1,
                    UserID = 5,
                    UserName = "test",
                    LeaveType = "Vacation",
                    Available = 10
                }
            };

            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, _httpContextAccessor, _authorizationService);

            // Configure the _leaveRepository mock to return the leaveModels when Get is called
            _leaveRepository.Setup(repo => repo.GetLeaveBalance(1)).Returns(leavebalanceModels);

            // Create an instance of the LeaveController
            var leaveController = new LeaveController(_leaveService);

            // Act
            var result = leaveController.GetLeaveBalance(1);


            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeOfType<RequestResult<List<LeaveBalance>>>();
            var requestResult = (RequestResult<List<LeaveBalance>>)okResult.Value;

            requestResult.RequestedObject.Should().BeEquivalentTo(leavebalanceModels);

        }

        [Test]
        public void CreateLeaveBalance_Test()
        {
            // Arrange
            var leavebalance = new LeaveBalance
            {
                ID = 1,
                UserID = 5,
                UserName = "test",
                LeaveType = "Vacation",
                Available = 10
            };

            var successfulResult = new RequestResult<bool>(true); // Simulate a successful result

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, leavebalance, new[] { Operations.Create }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.CreateLeaveBalance(leavebalance)).Returns(successfulResult);

            var controller = new LeaveController(_leaveService);

            var result = controller.CreateLeaveBalance(leavebalance);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void CreateLeaveBalance_ReturnsBadRequest()
        {
            // Arrange
            var leavebalance = new LeaveBalance
            {
                ID = 1,
                UserID = 5,
                UserName = "test",
                LeaveType = "Vacation",
                Available = 10
            };

            List<ValidationMessage> validationMessages = new List<ValidationMessage>()
                {
                    new ValidationMessage() { Reason = "Not Successful.", Severity = ValidationSeverity.Error }
                };
            var failedResult = new RequestResult<bool>(false, validationMessages);

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, leavebalance, new[] { Operations.Create }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.CreateLeaveBalance(leavebalance)).Returns(failedResult);

            var controller = new LeaveController(_leaveService);

            var result = controller.CreateLeaveBalance(leavebalance);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void UpdateLeaveBalance_Test()
        {
            // Arrange
            var leavebalance = new LeaveBalance
            {
                ID = 1,
                UserID = 5,
                UserName = "test",
                LeaveType = "Vacation",
                Available = 10
            };

            var successfulResult = new RequestResult<bool>(true); // Simulate a successful result

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, leavebalance, new[] { Operations.Update }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.UpdateLeaveBalance(leavebalance)).Returns(successfulResult);

            var controller = new LeaveController(_leaveService);

            var result = controller.UpdateLeaveBalance(leavebalance);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void UpdateLeaveBalance_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var leavebalance = new LeaveBalance
            {
                ID = 1,
                UserID = 5,
                UserName = "test",
                LeaveType = "Vacation",
                Available = 10
            };

            List<ValidationMessage> validationMessages = new List<ValidationMessage>()
                {
                    new ValidationMessage() { Reason = "Not Successful.", Severity = ValidationSeverity.Error }
                };
            var failedResult = new RequestResult<bool>(false, validationMessages);

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, leavebalance, new[] { Operations.Update }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.UpdateLeaveBalance(leavebalance)).Returns(failedResult);

            var controller = new LeaveController(_leaveService);

            var result = controller.UpdateLeaveBalance(leavebalance);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void DeleteLeaveBalance_Test()
        {
            // Arrange
            var leavebalance = new LeaveBalance
            {
                ID = 1,
                UserID = 5,
                UserName = "test",
                LeaveType = "Vacation",
                Available = 10
            };

            var successfulResult = new RequestResult<bool>(true); // Simulate a successful result

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, It.IsAny<LeaveBalance>(), new[] { Operations.Delete }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.DeleteLeaveBalance(leavebalance.ID)).Returns(successfulResult);

            var controller = new LeaveController(_leaveService);

            var result = controller.DeleteLeaveBalance(leavebalance.ID);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void DeleteLeaveBalance_ReturnsBadRequest()
        {
            // Arrange
            var leavebalance = new LeaveBalance
            {
                ID = 1,
                UserID = 5,
                UserName = "test",
                LeaveType = "Vacation",
                Available = 10
            };

            List<ValidationMessage> validationMessages = new List<ValidationMessage>()
                {
                    new ValidationMessage() { Reason = "Not Successful.", Severity = ValidationSeverity.Error }
                };
            var failedResult = new RequestResult<bool>(false, validationMessages);

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, It.IsAny<LeaveBalance>(), new[] { Operations.Delete }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.DeleteLeaveBalance(leavebalance.ID)).Returns(failedResult);

            var controller = new LeaveController(_leaveService);

            var result = controller.DeleteLeaveBalance(leavebalance.ID);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void GetEmployeeDetails_Test()
        {
            var userModels = new List<UserModel>
            {
                // Create some LeaveModel instances for testing
                new UserModel
            {
                Id = 5,
                UserName = "TestUser",
                FirstName = "John",
                LastName = "Doe",
                Email = "testuser@example.com",
                Mobile = "+1234567890",
                Country = "United States",
                ISDCode = "+1",
                TwoFactor = true,
                Locked = false,
                IsActive = true,
                EmailValidationStatus = 1,
                MobileValidationStatus = 1,
                OrgId = 1,
                AdminLevel = 1,
                Password = "Password123",
                NewPassword = "NewPassword456"
            }
            };

            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, _httpContextAccessor, _authorizationService);

            // Configure the _leaveRepository mock to return the leaveModels when Get is called
            _leaveRepository.Setup(repo => repo.GetEmployeeDetails(1)).Returns(userModels);

            // Create an instance of the LeaveController
            var leaveController = new LeaveController(_leaveService);

            // Act
            var result = leaveController.GetEmployeeDetails(1);


            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeOfType<RequestResult<List<UserModel>>>();
            var requestResult = (RequestResult<List<UserModel>>)okResult.Value;

            requestResult.RequestedObject.Should().BeEquivalentTo(userModels);

        }

        [Test]
        public void GetEmployeeLeaveBalance_Test()
        {
            var leavebalanceModels = new List<LeaveBalance>
            {
                // Create some LeaveModel instances for testing
                new LeaveBalance
                {
                    ID = 1,
                    UserID = 5,
                    UserName = "test",
                    LeaveType = "Vacation",
                    Available = 10
                }
            };

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));

            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, It.IsAny<LeaveBalance>(), new[] { Operations.Read }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            // Configure the _leaveRepository mock to return the leaveModels when Get is called
            _leaveRepository.Setup(repo => repo.GetEmployeeLeaveBalance(1)).Returns(leavebalanceModels);

            // Create an instance of the LeaveController
            var leaveController = new LeaveController(_leaveService);

            // Act
            var result = leaveController.GetEmployeeLeaveBalance(1);


            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeOfType<RequestResult<List<LeaveBalance>>>();
            var requestResult = (RequestResult<List<LeaveBalance>>)okResult.Value;

            requestResult.RequestedObject.Should().BeEquivalentTo(leavebalanceModels);

        }
        [Test]
        public void GetUsers_Test()
        {
            var userModels = new List<UserModel>
            {
                // Create some LeaveModel instances for testing
                new UserModel
            {
                Id = 5,
                UserName = "TestUser",
                FirstName = "John",
                LastName = "Doe",
                Email = "testuser@example.com",
                Mobile = "+1234567890",
                Country = "United States",
                ISDCode = "+1",
                TwoFactor = true,
                Locked = false,
                IsActive = true,
                EmailValidationStatus = 1,
                MobileValidationStatus = 1,
                OrgId = 1,
                AdminLevel = 1,
                Password = "Password123",
                NewPassword = "NewPassword456"
            }
            };

            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, _httpContextAccessor, _authorizationService);

            // Configure the _leaveRepository mock to return the leaveModels when Get is called
            _leaveRepository.Setup(repo => repo.GetUsers()).Returns(userModels);

            // Create an instance of the LeaveController
            var leaveController = new LeaveController(_leaveService);

            // Act
            var result = leaveController.GetUsers();


            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeOfType<RequestResult<List<UserModel>>>();
            var requestResult = (RequestResult<List<UserModel>>)okResult.Value;

            requestResult.RequestedObject.Should().BeEquivalentTo(userModels);

        }

        [Test]
        public void GetManagerAndEmployeeData_Test()
        {
            var employeeModels = new List<EmployeeTable>
            {
                new EmployeeTable {},
            };


            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));

            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, It.IsAny<EmployeeTable>(), new[] { Operations.Read }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);
           
            _leaveRepository.Setup(repo => repo.GetManagerAndEmployeeData()).Returns(employeeModels);

            // Create an instance of the LeaveController
            var leaveController = new LeaveController(_leaveService);

            // Act
            var result = leaveController.GetManagerAndEmployeeData();

            // Assert
            // Perform assertions to verify the result matches the expected outcome
            Assert.IsNotNull(result);

            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeOfType<RequestResult<List<EmployeeTable>>>();
            var requestResult = (RequestResult<List<EmployeeTable>>)okResult.Value;

            requestResult.RequestedObject.Should().BeEquivalentTo(employeeModels);
        }


        [Test]
        public void CreateManagerAndEmployeeData_Test()
        {
            // Arrange
            var employeeModel = new EmployeeTable { };

            var successfulResult = new RequestResult<bool>(true); // Simulate a successful result

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, employeeModel, new[] { Operations.Create }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.CreateManagerAndEmployeeData(employeeModel)).Returns(successfulResult);

            var controller = new LeaveController(_leaveService);

            var result = controller.CreateManagerAndEmployeeData(employeeModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void CreateManagerAndEmployeeData_ReturnsBadRequest()
        {
            // Arrange
            var employeeModel = new EmployeeTable { };

            List<ValidationMessage> validationMessages = new List<ValidationMessage>()
                {
                    new ValidationMessage() { Reason = "Not Successful.", Severity = ValidationSeverity.Error }
                };
            var failedResult = new RequestResult<bool>(false, validationMessages);


            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, employeeModel, new[] { Operations.Create }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.CreateManagerAndEmployeeData(employeeModel)).Returns(failedResult);

            var controller = new LeaveController(_leaveService);

            var result = controller.CreateManagerAndEmployeeData(employeeModel);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void EditManagerAndEmployeeData_Test()
        {
            var employeeModel = new EmployeeTable { };

            var successfulResult = new RequestResult<bool>(true); // Simulate a successful result

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, employeeModel, new[] { Operations.Update }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.EditManagerAndEmployeeData(employeeModel)).Returns(successfulResult);

            var controller = new LeaveController(_leaveService);

            var result = controller.EditManagerAndEmployeeData(employeeModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }
        

        [Test]
        public void EditManagerAndEmployeeData_ReturnsBadRequest()
        {
            var employeeModel = new EmployeeTable { };

            List<ValidationMessage> validationMessages = new List<ValidationMessage>()
                {
                    new ValidationMessage() { Reason = "Not Successful.", Severity = ValidationSeverity.Error }
                };
            var failedResult = new RequestResult<bool>(false, validationMessages);


            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, employeeModel, new[] { Operations.Update }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.EditManagerAndEmployeeData(employeeModel)).Returns(failedResult);

            var controller = new LeaveController(_leaveService);

            var result = controller.EditManagerAndEmployeeData(employeeModel);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void DeleteManagerAndEmployeeData_Test()
        {
            var employeeModel = new EmployeeTable { };

            var successfulResult = new RequestResult<bool>(true); // Simulate a successful result

            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, It.IsAny<EmployeeTable>(), new[] { Operations.Delete }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.DeleteManagerAndEmployeeData(1)).Returns(successfulResult);

            var controller = new LeaveController(_leaveService);

            var result = controller.DeleteManagerAndEmployeeData(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void DeleteManagerAndEmployeeData_ReturnsBadRequest()
        {
            var employeeModel = new EmployeeTable { };

            List<ValidationMessage> validationMessages = new List<ValidationMessage>()
                {
                    new ValidationMessage() { Reason = "Not Successful.", Severity = ValidationSeverity.Error }
                };
            var failedResult = new RequestResult<bool>(false, validationMessages);


            // Mock the IHttpContextAccessor and IAuthorizationService
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var authorizationServiceMock = new Mock<Business.Core.Interfaces.IAuthorizationService>();

            // Configure HttpContext.User to have an authorized user
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Admin"),

            }, "mock"));


            httpContextAccessorMock.SetupGet(x => x.HttpContext.User).Returns(userPrincipal);

            // Configure IAuthorizationService to return a successful authorization result
            authorizationServiceMock
                  .Setup(x => x.AuthorizeAsync(userPrincipal, It.IsAny<EmployeeTable>(), new[] { Operations.Delete }))
                  .ReturnsAsync(true);



            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, httpContextAccessorMock.Object, authorizationServiceMock.Object);

            _leaveRepository.Setup(x => x.DeleteManagerAndEmployeeData(1)).Returns(failedResult);

            var controller = new LeaveController(_leaveService);

            var result = controller.DeleteManagerAndEmployeeData(1);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}
