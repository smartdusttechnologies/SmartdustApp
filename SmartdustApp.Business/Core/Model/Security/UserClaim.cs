

namespace SmartdustApp.Business.Core.Model
{
    public class UserClaim : Entity
    {
        public CustomClaimType ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
