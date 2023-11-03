using DepartmentsWithMoreEmployees.Data;

namespace DepartmentsWithMoreEmployees
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            SoftuniContext dbContext = new SoftuniContext();

            var departmentsWithMoreThan5Emp = dbContext.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    Name = d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList()
                }).ToList();

            foreach (var department in departmentsWithMoreThan5Emp)
            {
                Console.WriteLine($"{department.Name} - {department.ManagerFirstName} {department.ManagerLastName}");
                foreach (var employee in department.Employees)
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }
        }
    }
}