using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Model
{
    // LeaveBalance class represents the balance of available leave for a specific user and leave type.
    public class LeaveBalance
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
