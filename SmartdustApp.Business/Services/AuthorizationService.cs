using Microsoft.AspNetCore.Authorization;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Services
{
    public class AuthorizationService : SmartdustApp.Business.Core.Interfaces.IAuthorizationService
    {
        private readonly Microsoft.AspNetCore.Authorization.IAuthorizationService _authorizationService;
        public AuthorizationService(Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<bool> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            var authorizationResult = _authorizationService.AuthorizeAsync(user, resource, requirements).Result.Succeeded;

            return authorizationResult;
        }

    }
}
