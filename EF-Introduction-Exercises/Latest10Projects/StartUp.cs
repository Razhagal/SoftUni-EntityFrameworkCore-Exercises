using Latest10Projects.Data;

/*
 * Write a program that return information about the last 10 started projects.
 * Sort them by name lexicographically and return their name, description and start date, each on a new row. 
 */

namespace Latest10Projects
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            SoftuniContext dbContext = new SoftuniContext();

            var projects = dbContext.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate
                })
                .ToList();

            foreach (var project in projects)
            {
                Console.WriteLine(project.Name);
                Console.WriteLine(project.Description);
                Console.WriteLine($"{project.StartDate:M/d/yyyy h:mm:ss tt}");
            }
        }
    }
}