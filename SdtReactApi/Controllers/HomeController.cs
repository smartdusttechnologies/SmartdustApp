using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Common;
using SmartdustApp.Web.Models;

namespace SmartdustApp.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContactService _contactService;
        private readonly IOrganizationService _organizationService;
        private readonly IMapper _mapper;
        public HomeController(IMapper mapper,ILogger<HomeController> logger, IContactService contactService, IOrganizationService organizationService)
        {
            _logger = logger;
            _contactService = contactService;
            _organizationService = organizationService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Contactus")]
        public IActionResult Contactsus(ContactDTO contact)
        {
            if (ModelState.IsValid)
            {
                var contactInfo = _mapper.Map<ContactDTO,ContactModel>(contact);
                return Json(_contactService.Save(contactInfo));
            }

            List<ValidationMessage> errors = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "All Fields Are Required", Severity = ValidationSeverity.Error, SourceId = "fields" }
                };
            return Json(new RequestResult<bool>(errors));

        }

        [HttpGet]
        [Route("GetOrganizations")]
        public IActionResult GetOrganizations()
        {
            var list = _organizationService.Get();
            if (list.IsSuccessful)
            {
                return Json(list);
            }

            List<ValidationMessage> errors = new List<ValidationMessage>
                {
                    new ValidationMessage { Reason = "Something Went Wrong", Severity = ValidationSeverity.Error, SourceId = "fields" }
                };
            return Json(new RequestResult<bool>(errors));
        }
    }
}