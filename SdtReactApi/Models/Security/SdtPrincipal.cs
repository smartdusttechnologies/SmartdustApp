using System.Security.Claims;

namespace SmartdustApi.Model
{
    public class SdtPrincipal : ClaimsPrincipal
    {
        public SdtPrincipal(SdtUserIdentity userIdentity) : base(userIdentity)
        {

        }
    }
}
