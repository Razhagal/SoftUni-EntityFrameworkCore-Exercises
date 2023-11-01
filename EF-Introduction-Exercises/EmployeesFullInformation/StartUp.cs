using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using System.Text;

/*
 * NOTE: You will need method public static string GetEmployeesFullInformation(SoftUniContext context) and public StartUp class. 
 * Now we can use the SoftUniContext to extract data from our database.
 * Your first task is to extract all employees and return their first, last and middle name, their job title and salary,
 * rounded to 2 symbols after the decimal separator, all of those separated with a space. Order them by employee id.
*/

namespace SoftUni
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            SoftuniContext dbContext = new SoftuniContext();

            Console.WriteLine(GetEmployeesFullInformation(dbContext));
        }

        public static string GetEmployeesFullInformation(SoftuniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees
                .OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MiddleName = e.MiddleName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                })
                .ToList();

            foreach (var employee in employees)
            {
                string middleName = !string.IsNullOrEmpty(employee.MiddleName) ? " " + employee.MiddleName : "";
                sb.AppendLine($"{employee.FirstName} {employee.LastName}{middleName} {employee.JobTitle} {employee.Salary:F2}");
            }

            return sb.ToString();
        }
    }
}