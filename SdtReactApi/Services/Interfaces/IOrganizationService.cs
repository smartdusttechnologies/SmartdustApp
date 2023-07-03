using SmartdustApi.Common;
using SmartdustApi.Model;
using SmartdustApi.Models;

namespace SmartdustApi.Services.Interfaces
{
    public interface IOrganizationService
    {
        RequestResult<List<OrganizationModel>> Get();
    }
}
