using Employee147.Data;

/*
 * Get the employee with id 147.
 * Return only his/her first name, last name, job title and projects (print only their names).
 * The projects should be ordered by name (ascending). 
 */

namespace Employee147
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            SoftuniContext dbContext = new SoftuniContext();

            var employee147 = dbContext.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Projects = e.Projects.OrderBy(p => p.Name).ToList()
                }).FirstOrDefault();

            if (employee147 != null)
            {
                Console.WriteLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");
                foreach (var project in employee147.Projects)
                {
                    Console.WriteLine(project.Name);
                }
            }
        }
    }
}