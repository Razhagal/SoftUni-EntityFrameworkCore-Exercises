using Microsoft.EntityFrameworkCore;
using RemoveTown.Data;

/*
 * Write a program that deletes a town with name „Seattle”.
 * Also, delete all addresses that are in those towns.
 * Return the number of addresses that were deleted in format “{count} addresses in Seattle were deleted”.
 * There will be employees living at those addresses, which will be a problem when trying to delete the addresses.
 */

namespace RemoveTown
{
    internal class StartUp
    {
        static void Main(string[] args)
        {
            SoftuniContext dbContext = new SoftuniContext();

            var townToDelete = dbContext.Towns.FirstOrDefault(t => t.Name == "Seattle");
            if (townToDelete != null)
            {
                var addresses = dbContext.Addresses
                    .Include(a => a.Employees)
                    .Where(a => a.TownId == townToDelete.TownId)
                    .ToList();
                foreach (var address in addresses)
                {
                    address.Employees.Clear();
                    address.Town = null;
                    dbContext.Addresses.Remove(address);
                }

                dbContext.Towns.Remove(townToDelete);
                dbContext.SaveChanges();

                Console.WriteLine($"{addresses.Count} addresses in {townToDelete.Name} were deleted");
            }
        }
    }
}