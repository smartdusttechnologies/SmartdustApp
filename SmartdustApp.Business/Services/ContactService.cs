using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Data.Repository.Interfaces;
using SmartdustApp.Business.Core.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using TestingAndCalibrationLabs.Business.Core.Interfaces;
using static SmartdustApp.Business.Core.Model.PolicyTypes;

namespace SmartdustApp.Business.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ISecurityParameterService _securityParameterService;


        public ContactService(IContactRepository contactRepository, IEmailService emailservice, IConfiguration configuration, IWebHostEnvironment hostingEnvironment, ISecurityParameterService securityParameterService)
        {
            _contactRepository = contactRepository;
            _emailService = emailservice;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _securityParameterService = securityParameterService;
        }

        public RequestResult<bool> Save(ContactModel contact)
        {

            string phone = $"{contact.Phone}";
            var validatePhoneResult = _securityParameterService.ValidatePhoneNumber(phone);
            if (!validatePhoneResult.IsSuccessful)
            {
                return validatePhoneResult;
            }
            var validateEmailResult = _securityParameterService.ValidateEmail(contact.Mail);
            if (!validateEmailResult.IsSuccessful)
            {
                return validateEmailResult;
            }

            EmailModel model = new EmailModel();

            //Read other values from Appsetting .Json 
            model.EmailTemplate = _configuration["TestingAndCalibrationSurvey:UserTemplate"];
            model.Subject = _configuration["TestingAndCalibrationSurvey:Subject"];

            //Create User Mail
            model.HtmlMsg = CreateBody(model.EmailTemplate);
            model.HtmlMsg = model.HtmlMsg.Replace("*name*", contact.Name);
            model.HtmlMsg = model.HtmlMsg.Replace("*Emailid*", contact.Mail);
            model.HtmlMsg = model.HtmlMsg.Replace("*mobilenumber*", phone);
            model.HtmlMsg = model.HtmlMsg.Replace("*Subject*", contact.Subject);
            model.HtmlMsg = model.HtmlMsg.Replace("*Address*", contact.Address);
            model.HtmlMsg = model.HtmlMsg.Replace("*Message*", contact.Message);
            model.Subject = model.Subject;

            model.Email = new List<string>();
            model.Email.Add("yashrajsmartdust@gmail.com");

            var isemailsendsuccessfully = _emailService.Sendemail(model);

            var result = _contactRepository.Save(contact);
            if (isemailsendsuccessfully.RequestedObject)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Thank You We Will Contact You As soon As Possible",Severity=ValidationSeverity.Information}
                };
                isemailsendsuccessfully.Message = success;
                return isemailsendsuccessfully;
            }
            List<ValidationMessage> error = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Unable To take Your Request Right Now",Severity=ValidationSeverity.Information}
                };
            isemailsendsuccessfully.Message = error;
            return isemailsendsuccessfully;
        }

        /// <summary>
        /// To use the email Template to send mail to the User participated.
        /// </summary>
        /// <param name="emailTemplate"></param>
        ///returns></returns>
        private string CreateBody(string emailTemplate)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Path.Combine(_hostingEnvironment.WebRootPath, _configuration["TestingAndCalibrationSurvey:UserTemplate"])))
            {
                body = reader.ReadToEnd();
            }
            return body;
        }
    }
}
