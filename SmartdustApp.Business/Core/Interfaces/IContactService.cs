using SmartdustApp.Business.Common;
using SmartdustApp.Business.Model;

namespace SmartdustApp.Business.Core.Interfaces
{
    public interface IContactService
    {
        RequestResult<bool> Save(ContactDTO contact);
    }
}
