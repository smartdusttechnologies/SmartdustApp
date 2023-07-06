using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Data.Repository.Interfaces;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _OrganizationRepository;

        public OrganizationService(IOrganizationRepository OrganizationRepository)
        {
            _OrganizationRepository = OrganizationRepository;
        }

        public RequestResult<List<Organization>> Get()
        {
           var organization = _OrganizationRepository.Get();
            if (organization == null)
            {
                return new RequestResult<List<Organization>>();
            }
            return new RequestResult<List<Organization>>(organization);
        }
    }
}
