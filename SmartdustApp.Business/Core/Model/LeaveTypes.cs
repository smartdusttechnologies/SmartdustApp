using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Model
{
    public class LeaveTypes
    {
        // Unique identifier for each leave type.
        public int ID { get; set; }

        // The name or label of the leave type (e.g., Sick Leave, Vacation).
        public string Name { get; set; }
    }
}
