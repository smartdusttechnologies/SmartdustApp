using SmartdustApi.Common;
using SmartdustApi.Model;
using SmartdustApi.Models;
using SmartdustApi.Repository.Interface;
using SmartdustApi.Repository.Interfaces;
using SmartdustApi.Services.Interfaces;

namespace SmartdustApi.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _OrganizationRepository;

        public OrganizationService(IOrganizationRepository OrganizationRepository)
        {
            _OrganizationRepository = OrganizationRepository;
        }

        public RequestResult<List<OrganizationModel>> Get()
        {
           var organization = _OrganizationRepository.Get();
            if (organization == null)
            {
                return new RequestResult<List<OrganizationModel>>();
            }
            return new RequestResult<List<OrganizationModel>>(organization);
        }
    }
}
