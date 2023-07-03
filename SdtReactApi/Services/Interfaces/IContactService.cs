using SmartdustApi.Common;
using SmartdustApi.Model;

namespace SmartdustApi.Services.Interfaces
{
    public interface IContactService
    {
        RequestResult<bool> Save(ContactDTO contact);
    }
}
