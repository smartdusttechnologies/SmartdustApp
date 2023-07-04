using SmartdustApp.Business.Common;
using SmartdustApp.Business.Model;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Data.Repository.Interfaces;

namespace SmartdustApp.Business.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public RequestResult<bool> Save(ContactDTO contact)
        {
           var result =  _contactRepository.Save(contact);
            if(result.IsSuccessful)
            {
                List<ValidationMessage> success = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Thank You We Will Contact You As soon As Possible",Severity=ValidationSeverity.Information}
                };
                result.Message = success;
                return result;
            }
            List<ValidationMessage> error = new List<ValidationMessage>()
                {
                    new ValidationMessage(){Reason = "Unable To take Your Request Right Now",Severity=ValidationSeverity.Information}
                };
            result.Message = error;
            return result;
        }
    }
}
