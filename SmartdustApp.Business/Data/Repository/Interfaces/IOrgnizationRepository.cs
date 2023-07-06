using SmartdustApp.Business.Core.Model;
using System.Collections.Generic;
namespace SmartdustApp.Business.Data.Repository.Interfaces
{
    public interface IOrganizationRepository
    {
        List<Organization> Get();

    }
}