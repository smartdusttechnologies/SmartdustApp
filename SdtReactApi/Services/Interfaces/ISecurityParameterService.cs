using SmartdustApi.Common;
using SmartdustApi.Model;


namespace SmartdustApi.Services.Interfaces
{
    public interface ISecurityParameterService
    {
        RequestResult<bool> ValidatePasswordPolicy( int orgId, string password);
        RequestResult<bool> ChangePaaswordPolicy(ChangePasswordModel password);
    }
}
