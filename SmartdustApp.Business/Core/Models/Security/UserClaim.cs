using SmartdustApp.Business.Model;

namespace SmartdustApp.Business.Core.Models
{
    public class UserClaim : Entity
    {
        public CustomClaimType ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
