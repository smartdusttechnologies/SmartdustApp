using System.ComponentModel.DataAnnotations;

namespace SmartdustApp.Web.Models
{
    public class ContactDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public int Phone { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
