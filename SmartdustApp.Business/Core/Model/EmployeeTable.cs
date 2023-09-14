using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Model
{
    public class EmployeeTable
    {
        public int ManagerID { get; set; }
        public string ManagerName { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
    }
}
