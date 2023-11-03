using EmployeesFromDepartment.Data;

namespace EmployeesFromDepartment
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            SoftuniContext dbContext = new SoftuniContext();

            var employees = dbContext.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    DepartmentName = e.Department.Name,
                    Salary = e.Salary
                })
                .OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName)
                .ToList();

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:F2}");
            }
        }
    }
}