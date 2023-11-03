using EmployeesAndProjects.Data;

/*
 * Find the first 10 employees who have projects started in the period 2001 - 2003 (inclusive).
 * Print each employee's first name, last name, manager’s first name and last name.
 * Then return all of their projects in the format "--<ProjectName> - <StartDate> - <EndDate>",
 * each on a new row. If a project has no end date, print "not finished" instead.
 */

namespace EmployeesAndProjects
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            SoftuniContext dbContext = new SoftuniContext();

            var employees = dbContext.Employees
                .Where(e => e.Projects.Any(p => p.StartDate.Year >= 2001 && p.StartDate.Year <= 2003))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    Lastname = e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.Projects
                }).Take(10).ToList();

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.Lastname} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");
                foreach (var project in employee.Projects)
                {
                    string projectEnd = project.EndDate != null ? project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt") : "not finished";
                    Console.WriteLine($"--{project.Name} - {project.StartDate:M/d/yyyy h:mm:ss tt} - {projectEnd}");
                }
            }
        }
    }
}