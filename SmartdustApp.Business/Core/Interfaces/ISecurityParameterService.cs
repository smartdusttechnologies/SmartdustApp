using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Model;


namespace SmartdustApp.Business.Core.Interfaces
{
    public interface ISecurityParameterService
    {
        RequestResult<bool> ValidatePasswordPolicy(int orgId, string password);
        RequestResult<bool> ChangePaaswordPolicy(ChangePasswordModel password);

        RequestResult<bool> ValidatePhoneNumber(string phoneNumber);
        RequestResult<bool> ValidateEmail(string email);
    }
}
