using SmartdustApi.Common;
using SmartdustApi.Model;

namespace SmartdustApi.Repository.Interfaces
{
    public interface IContactRepository
    {
        RequestResult<bool> Save(ContactDTO contact);
    }
}
