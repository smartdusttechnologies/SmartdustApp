
using SmartdustApi.Model;
using SmartdustApi.Model;
using SmartdustApi.Repository.Interface;
using SmartdustApi.Services.Interfaces;

namespace SmartdustApi.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public List<(int, string)> GetRoleWithOrg(string userName)
        {
            return _roleRepository.GetRoleWithOrg(userName);
        }
        //TODO: All the methods here should be saperated based on single responsibility principle
        
        public List<GroupClaim> GetGroupClaims(int organizationId, int userId, string permissionModuleType, string subPermissionModuleType, CustomClaimType claimType)
        {
            return _roleRepository.GetGroupClaims(organizationId, userId, permissionModuleType, subPermissionModuleType, claimType);
        }
        public UserModel GetUserByUserName(string userName)
        {
            return _roleRepository.GetUserByUserName(userName);
        }
        public List<UserRoleClaim> GetUserRoleClaims(int organizationId, int userId, string permissionModuleType, string subPermissionModuleType, CustomClaimType claimType)
        {
            return _roleRepository.GetUserRoleClaims(organizationId,userId, permissionModuleType, subPermissionModuleType, claimType);
        }
        public List<UserClaim> GetUserClaims(int organizationId, int userId, string permissionModuleType, string subPermissionModuleType, CustomClaimType claimType)
        {
            return _roleRepository.GetUserClaims( organizationId,  userId, permissionModuleType, subPermissionModuleType,  claimType);
        }
        //public List<string> GetRequiredClaimsForModule(PermissionModuleType permissionModuleType)
        //{
        //    return _roleRepository.GetRequiredClaimsForModule(permissionModuleType);
        //}
    }
}
