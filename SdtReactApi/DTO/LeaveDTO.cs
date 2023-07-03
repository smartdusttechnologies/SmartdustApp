using SmartdustApi.Model;

namespace SmartdustApi.DTO
{
    public class LeaveDTO : Entity
    {
        public DateOnly Date { get; set; }
        public int UserId { get; set; } 
        public string UserName { get; set; }
        public int LeaveStatus { get; set; }

    }
}
