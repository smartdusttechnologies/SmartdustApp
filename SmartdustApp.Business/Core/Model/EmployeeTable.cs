using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartdustApp.Business.Core.Model
{
    public class EmployeeTable
    {
        // Unique identifier for each record in the table.
        public int ID { get; set; }

        // Identifier for the manager of the employee.
        public int ManagerID { get; set; }

        // Name of the manager associated with this employee record.
        public string ManagerName { get; set; }

        // Identifier for the employee.
        public int EmployeeID { get; set; }

        // Name of the employee.
        public string EmployeeName { get; set; }
    }
}
