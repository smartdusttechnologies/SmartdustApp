using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Data.Repository.Interfaces
{
    public interface IContactRepository
    {
        RequestResult<bool> Save(ContactModel contact);
    }
}
