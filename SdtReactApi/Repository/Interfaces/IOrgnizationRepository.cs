using System.Collections.Generic;
using SmartdustApi.Model;
using SmartdustApi.Model;
using SmartdustApi.Models;

namespace SmartdustApi.Repository.Interface
{
    public interface IOrganizationRepository
    {
        List<OrganizationModel> Get();
        
    }
}