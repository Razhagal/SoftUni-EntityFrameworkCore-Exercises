using EmployeesWithSalary.Data;
using System.Net.Http.Headers;

/*
 * Your task is to extract all employees with salary over 50000.
 * Return their first names and salaries in format “{firstName} - {salary}”.
 * Salary must be rounded to 2 symbols, after the decimal separator.
 * Sort them alphabetically by first name.
 */

namespace EmployeesWithSalary
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            SoftuniContext dbContext = new SoftuniContext();

            var employeesWithSalaryOver50k = dbContext.Employees
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    Salary = e.Salary
                })
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .ToList();

            foreach (var employee in employeesWithSalaryOver50k)
            {
                Console.WriteLine($"{employee.FirstName} - {employee.Salary:F2}");
            }
        }
    }
}