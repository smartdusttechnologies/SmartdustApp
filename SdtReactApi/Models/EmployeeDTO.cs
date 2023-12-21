namespace SmartdustApp.Web.Models
{
    public class EmployeeDTO
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
