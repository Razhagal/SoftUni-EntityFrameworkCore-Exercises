using MiniORM.App.Data.Entities;
using MiniORM.App.Data;
using System.Linq;

namespace MiniORM.App
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            var context = new CompanyDbContext(@"Server=.; Database=MiniORM; Integrated Security=true; TrustServerCertificate=true");

            context.Employees.Add(new Employee
            {
                FirstName = "Penka",
                LastName = "Inserted",
                DepartmentId = context.Departments.First().Id,
                IsEmployed = true
            });

            var employee = context.Employees.Last();
            employee.MiddleName = "Modified";

            context.SaveChanges();
        }
    }
}