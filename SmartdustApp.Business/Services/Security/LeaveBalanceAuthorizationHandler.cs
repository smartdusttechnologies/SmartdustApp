using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Core.Model;
using SmartdustApp.Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Services.Security
{
    /// <summary>
    /// Authorization Handler For Leave Balnace
    public class LeaveBalanceAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement,LeaveBalance>
    {
        private readonly IRoleService _roleService;

        public LeaveBalanceAuthorizationHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, LeaveBalance resource)
        {
            var user = context.User as SdtPrincipal;
            if (user == null) return Task.CompletedTask;
            var sdtUserIdentity = user.Identity as SdtUserIdentity;
            var userRoleClaims = _roleService.GetUserRoleClaims(sdtUserIdentity.OrganizationId, sdtUserIdentity.UserId, PermissionModuleType.LeaveBalancePermission.ToString(), PermissionModuleType.LeaveBalancePermission.ToString(), CustomClaimType.ApplicationPermission);
            var userClaims = _roleService.GetUserClaims(sdtUserIdentity.OrganizationId, sdtUserIdentity.UserId, PermissionModuleType.LeaveBalancePermission.ToString(), PermissionModuleType.LeaveBalancePermission.ToString(), CustomClaimType.ApplicationPermission);
            var groupClaim = _roleService.GetGroupClaims(sdtUserIdentity.OrganizationId, sdtUserIdentity.UserId, PermissionModuleType.LeaveBalancePermission.ToString(), PermissionModuleType.LeaveBalancePermission.ToString(), CustomClaimType.ApplicationPermission);
            // Validate the requirement against the resource and identity.
            if (userRoleClaims.Any(p => p.ClaimType == CustomClaimType.ApplicationPermission && p.ClaimValue == requirement.Name))
                context.Succeed(requirement);
            else if (userClaims.Any(p => p.ClaimType == CustomClaimType.ApplicationPermission && p.ClaimValue == requirement.Name))
                context.Succeed(requirement);
            else if (groupClaim.Any(p => p.ClaimType == CustomClaimType.ApplicationPermission && p.ClaimValue == requirement.Name))
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
