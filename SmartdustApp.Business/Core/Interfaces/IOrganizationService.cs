using SmartdustApp.Business.Common;
using SmartdustApp.Business.Model;
using SmartdustApp.Business.Models;

namespace SmartdustApp.Business.Core.Interfaces
{
    public interface IOrganizationService
    {
        RequestResult<List<OrganizationModel>> Get();
    }
}
