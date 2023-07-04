

using SmartdustApp.Business.Core.Models;

namespace SmartdustApp.Business.Model
{
    public class GroupClaim : Entity
    {
        public CustomClaimType ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}

