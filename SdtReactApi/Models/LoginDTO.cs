using System.ComponentModel.DataAnnotations;

namespace SmartdustApi.Model
{
    public class LoginDTO
    {
        /// <summary>
        /// UserName for Login
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Password For Login
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
