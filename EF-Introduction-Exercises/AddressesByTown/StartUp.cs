using AddressesByTown.Data;

/*
 * Find all addresses, ordered by the number of employees who live there (descending),
 * then by town name (ascending), and finally by address text (ascending).
 * Take only the first 10 addresses.
 * For each address return it in the format "<AddressText>, <TownName> - <EmployeeCount> employees"
 */

namespace AddressesByTown
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            SoftuniContext dbContext = new SoftuniContext();

            var addresses = dbContext.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Select(a => new
                {
                    AddressText = a.AddressText,
                    TownName = a.Town.Name,
                    EmployeesCount = a.Employees.Count
                })
                .Take(10).ToList();

            foreach (var address in addresses)
            {
                Console.WriteLine($"{address.AddressText}, {address.TownName} - {address.EmployeesCount} employees");
            }
        }
    }
}