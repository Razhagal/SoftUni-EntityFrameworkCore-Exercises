using IncreaseSalaries.Data;

/*
 * Write a program that increase salaries of all employees that are in the
 * Engineering, Tool Design, Marketing or Information Services department by 12%.
 * Then return first name, last name and salary (2 symbols after the decimal separator) for those employees whose salary was increased.
 * Order them by first name (ascending), then by last name (ascending). 
 */

namespace IncreaseSalaries
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            SoftuniContext dbContext = new SoftuniContext();

            var employeesInDepartments = dbContext.Employees
                .Where(e => e.Department.Name == "Engineering" || e.Department.Name == "Tool Design" || e.Department.Name == "Marketing" || e.Department.Name == "Information Services")
                .ToList();

            foreach (var employee in employeesInDepartments)
            {
                employee.Salary *= 1.12m;
            }

            dbContext.SaveChanges();

            employeesInDepartments = employeesInDepartments.OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();
            foreach (var employee in employeesInDepartments)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }
        }
    }
}