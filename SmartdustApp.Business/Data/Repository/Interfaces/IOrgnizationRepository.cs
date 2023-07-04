using System.Collections.Generic;
using SmartdustApp.Business.Model;
using SmartdustApp.Business.Model;
using SmartdustApp.Business.Models;

namespace SmartdustApp.Business.Data.Repository.Interfaces
{
    public interface IOrganizationRepository
    {
        List<OrganizationModel> Get();

    }
}