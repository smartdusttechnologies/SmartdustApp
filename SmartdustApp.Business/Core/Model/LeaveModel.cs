using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Model
{
    public class LeaveModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string LeaveType { get; set; }
        public int LeaveTypeID { get; set; }
        public string Reason { get; set; }
        public DateTime AppliedDate { get; set; }
        public string LeaveStatus { get; set; }

        public int LeaveDays { get; set; }

        public List<DateTime> LeaveDates { get; set; }
    }
}
