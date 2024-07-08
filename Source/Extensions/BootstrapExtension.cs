#pragma warning disable CS8604

namespace OpenMovies.WebApi.Extensions;

/*
   BootstrapExtension serves the purpose of initializing essential configurations
   during the application startup. Specifically, it ensures the existence of a
   super-administrator user role and creates a super-admin user if one doesn't exist.
   This is crucial for setting up initial access control and administrative privileges
   before any other operations can proceed. Similar to Django's create super admin feature,
   this extension ensures that the application starts with necessary administrative
   capabilities securely in place.
*/

public static class BootstrapExtension
{
    public static async void Bootstrap(this IApplicationBuilder builder)
    {
        var serviceProvider = builder.ApplicationServices;

        using (var scope = serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var superAdminRoleExists = await roleManager.RoleExistsAsync("SuperAdmin");
            if (!superAdminRoleExists)
            {
                var superAdminRole = new IdentityRole("SuperAdmin");
                await roleManager.CreateAsync(superAdminRole);
            }

            IdentityResult result;

            do
            {
                var usersInRole = await userManager.GetUsersInRoleAsync("SuperAdmin");
                if (usersInRole.Count > 0)
                {
                    break;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("super-admin user not found. Let's create a new one.");
                Console.ResetColor();

                Console.Write("Enter an email: ");
                var email = Console.ReadLine();

                Console.Write("Enter a password: ");
                var password = Console.ReadLine();

                var superAdminUser = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                };

                result = await userManager.CreateAsync(superAdminUser, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"super-admin ({email}) user successfully created.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error creating super-admin user:");

                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Description}");
                    }

                    Console.ResetColor();
                }
            } while (!result.Succeeded);
        }
    }
}