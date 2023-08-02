using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Model
{
    public class UpdateLeaveModel
    {
        public int LeaveID { get; set; }
        public int StatusID { get; set; }
        public string Comment { get; set; }
    }
}
