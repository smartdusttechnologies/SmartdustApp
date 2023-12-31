﻿using SmartdustApp.Business.Common;
using SmartdustApp.Business.Core.Interfaces;
using SmartdustApp.Business.Data.Repository.Interfaces.Security;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.Business.Services
{
    public class SecurityParameterService : ISecurityParameterService
    {

        private readonly ISecurityParameterRepository _securityParameterRepository;

        public SecurityParameterService(ISecurityParameterRepository securityParameterRepository)
        {
            _securityParameterRepository = securityParameterRepository;

        }

        /// <summary>
        /// Method to validate Password Policy
        /// </summary>
        public RequestResult<bool> ValidatePasswordPolicy( int orgId, string password)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();
            try
            {
                var passwordPolicy = _securityParameterRepository.Get(orgId);
                var validatePasswordResult = ValidatePassword(password, passwordPolicy);
                return validatePasswordResult;
            }
            catch (Exception ex)
            {
                validationMessages.Add(new ValidationMessage { Reason = "Validation failed!", Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;

            }
        }
        /// <summary>
        /// Method to validate the password like Length, Uppercaps, LowerCaps, Min and Max Digits
        /// </summary>
        private RequestResult<bool> ValidatePassword(string password, SecurityParameter securityParameter)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();

            if (password.Length < securityParameter.MinLength)
            {
                validationMessages.Add(new ValidationMessage { Reason = "Minimum length of the password should be " + securityParameter.MinLength + "characters long", Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }
            if (!Helpers.ValidateMinimumSmallChars(password, securityParameter.MinSmallChars))
            {
                validationMessages.Add(new ValidationMessage { Reason = "Minimum number of small characters the password should have is " + securityParameter.MinSmallChars, Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }

            if (!Helpers.ValidateMinimumCapsChars(password, securityParameter.MinCaps))
            {
                validationMessages.Add(new ValidationMessage { Reason = "Minimum number of capital characters the password should have is " + securityParameter.MinCaps, Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }

            if (!Helpers.ValidateMinimumDigits(password, securityParameter.MinNumber))
            {
                validationMessages.Add(new ValidationMessage { Reason = "Minimum number of numeric characters the password should have is " + securityParameter.MinNumber, Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }

            if (!Helpers.ValidateMinimumSpecialChars(password, securityParameter.MinSpecialChars))
            {
                validationMessages.Add(new ValidationMessage { Reason = "Minimum number of special characters the password should have is " + securityParameter.MinSpecialChars, Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }

            if (!Helpers.ValidateDisallowedChars(password, securityParameter.DisAllowedChars))
            {
                validationMessages.Add(new ValidationMessage { Reason = "Characters which are not allowed in password are " + securityParameter.DisAllowedChars, Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages); ;
            }
            return new RequestResult<bool>(true);
        }
        /// <summary>
        /// Method to validate the Phone Number.
        /// </summary>
        private RequestResult<bool> ValidatePhoneNumber(string phoneNumber)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();

            // Remove any non-digit characters from the phone number
            string sanitizedPhoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            if (sanitizedPhoneNumber.Length != 10)
            {
                validationMessages.Add(new ValidationMessage { Reason = "Phone number must be 10 digits long", Severity = ValidationSeverity.Error });
                return new RequestResult<bool>(false, validationMessages);
            }

            // Perform any additional validation logic specific to phone numbers, if needed

            return new RequestResult<bool>(true);
        }
        /// <summary>
        /// Method to validate the Change Password.
        /// </summary>
        public RequestResult<bool> ChangePaaswordPolicy(ChangePasswordModel password)
        {
            List<ValidationMessage> validationMessages = new List<ValidationMessage>();

            if (string.IsNullOrEmpty(password.OldPassword))
            {
                validationMessages.Add(new ValidationMessage { Reason = "Please enter Old Password", Severity = ValidationSeverity.Error, SourceId = "OldPassword" });
                return new RequestResult<bool>(false, validationMessages); ;

            }
            else if (string.IsNullOrEmpty(password.NewPassword))
            {
                validationMessages.Add(new ValidationMessage { Reason = "Please Enter New Password.", Severity = ValidationSeverity.Error, SourceId = "NewPassword" });
                return new RequestResult<bool>(false, validationMessages); ;
            }
            else if (string.IsNullOrEmpty(password.ConfirmPassword))
            {
                validationMessages.Add(new ValidationMessage { Reason = "Please Enter Confirm Password.", Severity = ValidationSeverity.Error, SourceId = "ConfirmPassword" });
                return new RequestResult<bool>(false, validationMessages); ;
            }
            else if (password.OldPassword == password.NewPassword)
            {
                validationMessages.Add(new ValidationMessage { Reason = "New password must be different from old password.", Severity = ValidationSeverity.Error, SourceId = "NewPassword" });
                return new RequestResult<bool>(false, validationMessages); ;

            }
            else if (password.NewPassword != password.ConfirmPassword)
            {
                validationMessages.Add(new ValidationMessage { Reason = "New password and confirm password fields must match.", Severity = ValidationSeverity.Error, SourceId = "ConfirmPassword" });
                return new RequestResult<bool>(false, validationMessages); ;

            }
            return new RequestResult<bool>(true);
        }
    }
}
