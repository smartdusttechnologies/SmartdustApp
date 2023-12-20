using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Model
{
    public class LeaveStatusActions
    {
        // Unique identifier for each leave status action.
        public int ID { get; set; }

        // The name or label of the leave status action (e.g., Approve, Reject).
        public string Name { get; set; }
    }
}
