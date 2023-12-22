namespace SmartdustApp.Web.Models
{
    public class UpdateLeaveStatusDTO
    {
        // Identifier for the leave request being updated.
        public int LeaveID { get; set; }

        // Identifier for the new status of the leave request.
        public int StatusID { get; set; }

        // Additional comments or notes related to the leave update.
        public string Comment { get; set; }
    }
}
