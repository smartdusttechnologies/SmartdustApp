using System.ComponentModel.DataAnnotations;

namespace SmartdustApp.Web.Models
{
    public class UserDTO
    {
        /// <summary>
        /// User Name.
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// First Name.
        /// </summary>
        [Required]
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Email Address.
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Mobile.
        /// </summary>
        [Required]
        public string Mobile { get; set; }
        /// <summary>
        /// Country.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// IsdCode.
        /// </summary>
        public string ISDCode { get; set; }
        public int MobileValidationStatus { get; set; }
        [Required]
        public int OrgId { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }

    }
}
