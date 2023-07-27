using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Enums
{
    public class LeaveTypeMapping
    {
        public static Dictionary<LeaveType, int> TypeToID = new Dictionary<LeaveType, int>
        {
        { LeaveType.Medical, 1 },   // Assuming Medical has LeaveTypeID 1
        { LeaveType.Paid, 2 },      // Assuming Paid has LeaveTypeID 2
        { LeaveType.LeaveOfAbsence, 3 } // Assuming LeaveOfAbsence has LeaveTypeID 3
        };
    }
}
