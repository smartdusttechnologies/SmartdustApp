using System.Security.Claims;

namespace SmartdustApp.Business.Model
{
    public class SdtPrincipal : ClaimsPrincipal
    {
        public SdtPrincipal(SdtUserIdentity userIdentity) : base(userIdentity)
        {

        }
    }
}
