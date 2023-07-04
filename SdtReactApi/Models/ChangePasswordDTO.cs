using System.ComponentModel.DataAnnotations;

namespace SmartdustApp.DTO
{
    public class ChangePasswordDTO
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Username{ get; set; }
    }
}
