using SmartdustApp.Web.Models;
using SmartdustApp.Business.Core.Model;

namespace SmartdustApp.DTO
{
    public class LeaveDTO
    {
        // Unique identifier for each leave record.
        public int ID { get; set; }

        // Identifier for the user requesting the leave.
        public int UserID { get; set; }

        // Name of the user requesting the leave.
        public string UserName { get; set; }

        // Type of leave (e.g., sick leave, vacation).
        public string LeaveType { get; set; }

        // Identifier for the leave type.
        public int LeaveTypeID { get; set; }

        // Reason for taking the leave.
        public string Reason { get; set; }

        // Date when the leave was applied for.
        public DateTime AppliedDate { get; set; }

        // Status of the leave (e.g., pending, approved, rejected).
        public string LeaveStatus { get; set; }

        // Identifier for the leave status.
        public int LeaveStatusID { get; set; }

        // Number of days requested for the leave.
        public int LeaveDays { get; set; }

        // Additional comments by Manager related to the leave.
        public string Comment { get; set; }

        // List of specific dates for which leave is requested.
        public List<DateTime> LeaveDates { get; set; }

        // List of file IDs attached to the leave request.
        public List<int> AttachedFileIDs { get; set; }

    }
}
