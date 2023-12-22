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
    [Route("api/[controller]")]
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
            var contactModel = _mapper.Map<ContactDTO,ContactModel>(contact);
            var result = _contactService.Save(contactModel);
            if (result.RequestedObject)
            {
                return Json(result);
            }
            return BadRequest(result);

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