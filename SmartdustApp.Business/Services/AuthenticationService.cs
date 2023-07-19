﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Data.Repository.Interfaces.Security;
using SmartdustApp.Business.Data.Repository.Interfaces;
using SmartdustApp.Business.Core.Model;
using Microsoft.Extensions.Configuration;

namespace SmartdustApp.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISecurityParameterService _securityParameterService;
        private readonly IRoleRepository _roleRepository;
        private readonly ILoggerRepository _loggerRepository;
        public AuthenticationService(IConfiguration configuration,
            IAuthenticationRepository authenticationRepository, IUserRepository userRepository,
             ISecurityParameterService securityParameterService,
             ILoggerRepository loggerRepository,
              IRoleRepository roleRepository)
        {
            _configuration = configuration;
            _authenticationRepository = authenticationRepository;
            _userRepository = userRepository;
            _securityParameterService = securityParameterService;
            _loggerRepository = loggerRepository;
            _roleRepository = roleRepository;

        }
        /// <summary>
        /// Method to Authenticate for Login
        /// </summary>
        public RequestResult<LoginToken> Login(LoginRequest loginRequest)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                LoginToken token = new LoginToken();
                var passwordLogin = _authenticationRepository.GetLoginPassword(loginRequest.UserName);
                string valueHash = string.Empty;
                if (passwordLogin != null && !Hasher.ValidateHash(loginRequest.Password, passwordLogin.PasswordSalt, passwordLogin.PasswordHash, out valueHash))
                {
                    validationMessages.Add(new ValidationMessage { Reason = "UserName or password mismatch.", Severity = ValidationSeverity.Error });
                    return new RequestResult<LoginToken>(validationMessages);
                }
                var user = _userRepository.Get(passwordLogin.UserId);
                if (user == null)
                {
                    validationMessages.Add(new ValidationMessage { Reason = "UserName or password mismatch.", Severity = ValidationSeverity.Error });
                    return new RequestResult<LoginToken>(validationMessages);
                }
                if (!user.IsActive && user.Locked)
                {
                    validationMessages.Add(new ValidationMessage { Reason = "Access denied.", Severity = ValidationSeverity.Error });
                    return new RequestResult<LoginToken>(validationMessages);
                }
                #region this needs to be implemented once we have change password UI.
                //int changeIntervalDays = 30;
                //if (user.OrgId != 0)
                //{
                //    var passwordPolicy = _securityParameterService.Get(user.OrgId);
                //    changeIntervalDays = passwordPolicy.ChangeIntervalDays;
                //}
                //if(passwordLogin.ChangeDate.AddDays(changeIntervalDays) < DateTime.Today)
                //{
                //    validationMessages.Add(new ValidationMessage { Reason = "Password expired.", Severity = ValidationSeverity.Error });
                //    return new RequestResult<LoginToken>(validationMessages);
                //}
                #endregion

                loginRequest.Id = passwordLogin.UserId;
                token = GenerateTokens(loginRequest.UserName);

                //TODO: this should be a async operation and can be made more cross-cutting design feature rather than calling inside the actual feature.
                loginRequest.LoginDate = DateTime.Now;
                loginRequest.PasswordHash = valueHash;
                _loggerRepository.LoginLog(loginRequest);

                return new RequestResult<LoginToken>(token, validationMessages);
            }
            catch (Exception ex)
            {
                //_logger.LogException(new ExceptionLog
                // {
                //   ExceptionDate = DateTime.Now,
                //   ExceptionMsg = ex.Message,
                //  ExceptionSource = ex.Source,
                //   ExceptionType = "UserService",
                //  FullException = ex.StackTrace
                // });
                validationMessages.Add(new ValidationMessage { Reason = ex.Message, Severity = ValidationSeverity.Error, Description = ex.StackTrace });
                return new RequestResult<LoginToken>(validationMessages);
            }
        }
        /// <summary>
        /// Method to Generate Token
        /// </summary>
        private LoginToken GenerateTokens(string userName)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            DateTime now = DateTime.Now;
            var claims = GetTokenClaims(userName, now);

            var accessJwt = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                notBefore: now,
                expires: now.AddDays(1),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            );

            var encodedAccessJwt = new JwtSecurityTokenHandler().WriteToken(accessJwt);

            var refreshJwt = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                claims: claims,
                notBefore: now,
                expires: now.AddDays(30),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256Signature)
            );
            var encodedRefreshJwt = new JwtSecurityTokenHandler().WriteToken(refreshJwt);

            var loginToken = new LoginToken
            {
                UserName = userName,
                AccessToken = encodedAccessJwt,
                AccessTokenExpiry = DateTime.Now.AddDays(1),
                RefreshToken = encodedRefreshJwt,
                RefreshTokenExpiry = DateTime.Now.AddDays(30),
            };
            _authenticationRepository.SaveLoginToken(loginToken);
            //TODO: this should be a async operation and can be made more cross-cutting design feature rather than calling inside the actual feature.
            _loggerRepository.LoginTokenLog(loginToken);
            return loginToken;
        }
        /// <summary>
        ///Method to Get Token Cliams
        /// </summary>
        private List<Claim> GetTokenClaims(string sub, DateTime dateTime)
        {
            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:

            var userModel = _roleRepository.GetUserByUserName(sub);
            //var roleClaims = roleByOrganizationWithClaims.Select(x => new Claim(ClaimTypes.Role, x.RoleName));
            //var userRoleClaim = roleByOrganizationWithClaims.Select(x => new Claim(CustomClaimTypes.Permission, x.ClaimName));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, sub),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, Helpers.ToUnixEpochDate(dateTime).ToString(), ClaimValueTypes.Integer64)
            };
            //.Union(roleClaims).Union(userRoleClaim).ToList(); 

            //var roles = _roleRepository.GetRoleWithOrg(sub);
            //foreach (var role in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role.Item2));
            //}

            claims.Add(new Claim(CustomClaimType.UserId.ToString(), userModel.Id.ToString()));

            if (sub.ToLower() == "sysadmin")
                claims.Add(new Claim(CustomClaimType.OrganizationId.ToString(), "0"));
            else
                claims.Add(new Claim(CustomClaimType.OrganizationId.ToString(), userModel.OrgId.ToString()));

            return claims;
        }

        /// <summary>
        /// Method to Add new and validate existing user for Registration
        /// </summary>
        public RequestResult<bool> Add(UserModel user)
        {
            try
            {
                var validationResult = ValidateNewUserRegistration(user);
                if (validationResult.IsSuccessful)
                {
                    PasswordLogin passwordLogin = Hasher.HashPassword(user.Password);
                    _userRepository.Insert(user, passwordLogin);
                    return new RequestResult<bool>(true);
                }
                return new RequestResult<bool>(false, validationResult.Message);
            }
            catch (Exception ex)
            {

                return new RequestResult<bool>(false);
            }
        }
        /// <summary>
        /// Method to Validate the New User Registation
        /// </summary>
        private RequestResult<bool> ValidateNewUserRegistration(UserModel user)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            UserModel existingUser = _userRepository.Get(user.UserName);
            if (existingUser != null)
            {
                var error = new ValidationMessage { Reason = "The UserName not available", Severity = ValidationSeverity.Error };
                validationMessages.Add(error);
                return new RequestResult<bool>(false, validationMessages);
            }
            if (user.Password != user.NewPassword)
            {
                var error = new ValidationMessage { Reason = "Password Didn't Match", Severity = ValidationSeverity.Error };
                validationMessages.Add(error);
                return new RequestResult<bool>(false, validationMessages);
            }
            var validatePhoneResult = _securityParameterService.ValidatePhoneNumber(user.Mobile);
            if (!validatePhoneResult.IsSuccessful)
            {
                return validatePhoneResult;
            }
            var validateEmailResult = _securityParameterService.ValidateEmail(user.Email);
            if (!validateEmailResult.IsSuccessful)
            {
                return validateEmailResult;
            }

            var validatePasswordResult = _securityParameterService.ValidatePasswordPolicy(user.OrgId, user.Password);
            return validatePasswordResult;
        }
        public RequestResult<bool> UpdatePaasword(ChangePasswordModel password)

        {
            try
            {
                var passworsResult = _securityParameterService.ChangePaaswordPolicy(password);
                if (passworsResult.IsSuccessful)
                {
                    var validationResult = _securityParameterService.ValidatePasswordPolicy(0, password.NewPassword);

                    var passwordLogin = _authenticationRepository.GetLoginPassword(password.Username);
                    List<ValidationMessage> validationMessages = new List<ValidationMessage>();
                    string valueHash = string.Empty;
                    if (password != null && !Hasher.ValidateHash(password.OldPassword, passwordLogin.PasswordSalt, passwordLogin.PasswordHash, out valueHash))
                    {
                        validationMessages.Add(new ValidationMessage { Reason = "Old password is incorrect.", Severity = ValidationSeverity.Error, SourceId = "OldPassword" });
                        return new RequestResult<bool>(validationMessages);
                    }
                    if (password.ConfirmPassword != password.NewPassword)
                    {
                        validationMessages.Add(new ValidationMessage { Reason = "New Password Didn't Match.", Severity = ValidationSeverity.Error, SourceId = "OldPassword" });
                        return new RequestResult<bool>(validationMessages);
                    }
                    if (validationResult.IsSuccessful)
                    {
                        if (passworsResult.IsSuccessful)
                        {
                            PasswordLogin newPasswordLogin = Hasher.HashPassword(password.NewPassword);
                            ChangePasswordModel passwordModel = new ChangePasswordModel();
                            passwordModel.PasswordHash = newPasswordLogin.PasswordHash;
                            passwordModel.UserId = password.UserId;
                            passwordModel.PasswordSalt = newPasswordLogin.PasswordSalt;

                            _userRepository.Update(passwordModel);

                            return new RequestResult<bool>(true);
                        }

                    }
                    return new RequestResult<bool>(false, validationResult.Message);
                }
                return new RequestResult<bool>(false, passworsResult.Message);
            }
            catch (Exception ex)
            {
                return new RequestResult<bool>(false);
            }

        }
    }
}
