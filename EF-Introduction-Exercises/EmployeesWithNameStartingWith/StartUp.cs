using EmployeesWithNameStartingWith.Data;

/*
 * Write a program that finds all employees whose first name starts with "Sa".
 * Return their first, last name, their job title and salary, rounded to 2 symbols after the decimal separator
 * in the format given in the example below. Order them by first name, then by last name (ascending).
 */

namespace EmployeesWithNameStartingWith
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            SoftuniContext dbContext = new SoftuniContext();

            var employeesStartingWithSa = dbContext.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                })
                .ToList();

            foreach (var employee in employeesStartingWithSa)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
            }
        }
    }
}