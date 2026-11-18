using ELearning.Identity.Domain;
using Microsoft.AspNetCore.Identity;

public class RoleSeeder
{
    public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
    {
        foreach (var role in Roles.All)
        {
            if (! await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(
                    new ApplicationRole(role)
                );
            }
        }
    }
}