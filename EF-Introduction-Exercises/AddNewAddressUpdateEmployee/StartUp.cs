using AddNewAddressUpdateEmployee.Data;
using AddNewAddressUpdateEmployee.Models;

/*
 * Create a new address with text "Vitoshka 15" and TownId 4.
 * Set that address to the employee with last name "Nakov".
 * Then order by descending all the employees by their Address’ Id, take 10 rows and from them, take the AddressText.
 * Return the results each on a new line
 */

namespace AddNewAddressUpdateEmployee
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            SoftuniContext dbContext = new SoftuniContext();

            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var address = dbContext.Addresses.FirstOrDefault(e => e.AddressText == "Vitoshka 15");
            if (address == null)
            {
                dbContext.Addresses.Add(newAddress);
            }


            var nakovEmployee = dbContext.Employees
                .Where(e => e.LastName == "Nakov")
                .FirstOrDefault();
            if (nakovEmployee != null)
            {
                nakovEmployee.Address = newAddress;
            }

            dbContext.SaveChanges();

            var employees = dbContext.Employees
                .OrderByDescending(e => e.AddressId)
                .Select(e => new
                {
                    AddressText = e.Address.AddressText
                })
                .Take(10)
                .ToList();

            foreach (var employee in employees)
            {
                Console.WriteLine(employee.AddressText);
            }
        }
    }
}