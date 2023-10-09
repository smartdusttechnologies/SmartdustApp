using Moq;
using FluentAssertions;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Business.Data.Repository.Interfaces;
using SmartdustApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TestingAndCalibrationLabs.Business.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace SmartdustApp.Tests
{
    [TestFixture]
    public class LeaveControllerTest
    {

        ILeaveService _leaveService;
        IEmailService _emailService;
        IConfiguration _configuration;
        IWebHostEnvironment _hostingEnvironment;
        IUserRepository _userRepository;
        IHttpContextAccessor _httpContextAccessor;
        IAuthorizationService _authorizationService;
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

            _leaveService = new LeaveService(_leaveRepository.Object, _emailService ,_configuration , _hostingEnvironment , _userRepository , _httpContextAccessor , _authorizationService);
            // Configure the _leaveRepository mock to return the leaveModels when Get is called
            _leaveRepository.Setup(repo => repo.Get(userId)).Returns(leaveModels);

            // Create an instance of the LeaveController
            var leaveController = new LeaveController(_leaveService);

            // Act
            var result = leaveController.GetLeave(userId);

            // Assert
            // Perform assertions to verify the result matches the expected outcome
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<LeaveModel>>(result);

            result.Should().BeEquivalentTo(leaveModels);
            // Cleanup or verify other behaviors as needed
        }


        [Test]
        public void ApplyLeave_ReturnsOkResult()
        {
            // Arrange
            var leaveModel = new LeaveModel { ID = 52, UserID = 5, UserName = "YashRaj", LeaveType = "MedicalLeave", LeaveTypeID = 1, Reason = "aa", AppliedDate = DateTime.Now.Date, LeaveStatus = "null", LeaveStatusID = 1, LeaveDays = 3, LeaveDates = new List<DateTime> { DateTime.Now.Date, DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(2), }, AttachedFileIDs = new List<int> { 1, 2 } };
            var successfulResult = new RequestResult<bool>(true); // Simulate a successful result

            _leaveService = new LeaveService(_leaveRepository.Object, _emailService, _configuration, _hostingEnvironment, _userRepository, _httpContextAccessor, _authorizationService);

            _leaveRepository.Setup(x => x.Save(leaveModel)).Returns(successfulResult);

            var controller = new LeaveController(_leaveService);

            // Act
            var result = controller.ApplyLeave(leaveModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void UpdateLeave_ReturnsOkResult()
        {
            // Arrange
            var leaveModel = new LeaveModel { ID = 52, UserID = 5, UserName = null, LeaveType = "MedicalLeave", LeaveTypeID = 1, Reason = "aa", AppliedDate = new DateTime(), LeaveStatus = "null", LeaveStatusID = 1, LeaveDays = 3, LeaveDates = new List<DateTime> { DateTime.Now.Date, DateTime.Now.Date.AddDays(1), DateTime.Now.Date.AddDays(2), }, AttachedFileIDs = new List<int> { 1, 2 } };
            var successfulResult = new RequestResult<bool>(true); // Simulate a successful result

            _leaveRepository.Setup(x => x.Update(leaveModel)).Returns(successfulResult);

            var controller = new LeaveController(_leaveService);

            // Act
            var result = controller.UpdateLeave(leaveModel);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void UpdateLeaveStatus_ReturnsOkResult()
        {
            // Arrange
            var updateStatus = new UpdateLeaveModel { LeaveID=1 , StatusID = 5 , Comment="aa" };
            var successfulResult = new RequestResult<bool>(true); // Simulate a successful result

            _leaveRepository.Setup(x => x.UpdateLeaveStatus(updateStatus.LeaveID , updateStatus.StatusID)).Returns(successfulResult);

            var controller = new LeaveController(_leaveService);

            // Act
            var result = controller.UpdateLeaveStatus(updateStatus);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
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

            // Configure the _leaveRepository mock to return the leaveModels when Get is called
            _leaveRepository.Setup(repo => repo.GetLeaveTypes()).Returns(leaveTypesModels);

            // Create an instance of the LeaveController
            var leaveController = new LeaveController(_leaveService);

            // Act
            var result = leaveController.GetLeaveTypes();

            // Assert
            // Perform assertions to verify the result matches the expected outcome
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<LeaveTypes>>(result);

            result.Should().BeEquivalentTo(leaveTypesModels);
            // Cleanup or verify other behaviors as needed
        }

    }
}
