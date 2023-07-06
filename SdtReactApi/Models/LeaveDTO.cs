using SmartdustApp.Web.Models;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.DTO
{
    public class LeaveDTO : Entity
    {
        public DateOnly Date { get; set; }
        public int UserId { get; set; } 
        public string UserName { get; set; }
        public int LeaveStatus { get; set; }

    }
}
