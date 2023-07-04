using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Core.Interfaces
{
    public interface IContactService
    {
        RequestResult<bool> Save(ContactModel contact);
    }
}
