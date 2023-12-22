using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Model
{
    // LeaveDateModel class represents a model for storing specific dates associated with a leave request.
    public class LeaveDateModel
    {
        // Identifier for the leave request to which specific dates are associated.
        public int LeaveID { get; set; }

        // The specific date associated with the leave request.
        public DateTime LeaveDate { get; set; }
    }

}
