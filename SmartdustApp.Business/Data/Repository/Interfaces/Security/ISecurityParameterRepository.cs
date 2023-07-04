using SmartdustApp.Business.Common;
using SmartdustApp.Business.Model;
using System.Collections.Generic;

namespace SmartdustApp.Business.Data.Repository.Interfaces.Security
{
    public interface ISecurityParameterRepository
    {
        SecurityParameter Get(int orgId);
        List<SecurityParameter> Get(SessionContext sessionContext);
        SecurityParameter Get(SessionContext sessionContext, int OrgId);
        int Insert(SecurityParameter securityParameter);
        int Update(SecurityParameter updatedSecurityParameter);
        bool Delete(SessionContext sessionContext, int id);
    }
}
