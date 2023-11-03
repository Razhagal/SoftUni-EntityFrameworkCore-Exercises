using DeleteProjectById.Data;
using Microsoft.EntityFrameworkCore;

/*
 * Let's delete the project with id 2.
 * Remember to restore your database after this task.
 */

namespace DeleteProjectById
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            SoftuniContext dbContext = new SoftuniContext();

            var projectToDelete = dbContext.Projects.Include(p => p.Employees).FirstOrDefault(p => p.ProjectId == 2);
            if (projectToDelete != null)
            {
                var employeesWithFoundProject = dbContext.Employees
                    .Where(e => e.Projects.Contains(projectToDelete))
                    .ToList();
                Console.WriteLine(employeesWithFoundProject.Count);
                foreach (var employee in employeesWithFoundProject)
                {
                    employee.Projects.Remove(projectToDelete);
                }

                projectToDelete.Employees.Clear();

                dbContext.Projects.Remove(projectToDelete);
            }

            dbContext.SaveChanges();
        }
    }
}