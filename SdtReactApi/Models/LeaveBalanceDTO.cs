namespace SmartdustApp.Web.Models
{
    public class LeaveBalanceDTO
    {
        // Unique identifier for each leave balance record.
        public int ID { get; set; }

        // Identifier for the user whose leave balance is being tracked.
        public int UserID { get; set; }

        // Name of the user whose leave balance is being tracked.
        public string UserName { get; set; }

        // Type of leave for which the balance is being tracked (e.g., Sick Leave, Vacation).
        public string LeaveType { get; set; }

        // The available balance of leave days for the specified leave type.
        public int Available { get; set; }
    }
}
