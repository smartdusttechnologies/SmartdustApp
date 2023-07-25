using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Model
{
    public class LeaveBalance
    {
        public int UserID { get; set; }
        public int MedicalLeave { get; set; }
        public int PaidLeave { get; set; }
    }
}
