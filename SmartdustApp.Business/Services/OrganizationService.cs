using SmartdustApp.Business.Common;
using SmartdustApp.Business.Model;
using SmartdustApp.Business.Models;
using SmartdustApp.Business.Repository.Interfaces;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Data.Repository.Interfaces;

namespace SmartdustApp.Business.Services
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
