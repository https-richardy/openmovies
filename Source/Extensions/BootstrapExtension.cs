#pragma warning disable CS8604

namespace OpenMovies.WebApi.Extensions;

/*
   BootstrapExtension serves the purpose of initializing essential configurations
   during the application startup. Specifically, it ensures the existence of a
   administrator user role and creates a admin user if one doesn't exist.
   This is crucial for setting up initial access control and administrative privileges
   before any other operations can proceed. Similar to Django's create admin feature,
   this extension ensures that the application starts with necessary administrative
   capabilities securely in place.
*/

public static class BootstrapExtension
{
    [ExcludeFromCodeCoverage]
    public static async void Bootstrap(this IApplicationBuilder builder)
    {
        var serviceProvider = builder.ApplicationServices;

        using (var scope = serviceProvider.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var adminRoleExists = await roleManager.RoleExistsAsync("Administrator");
            if (!adminRoleExists)
            {
                var adminRole = new IdentityRole("Administrator");
                await roleManager.CreateAsync(adminRole);
            }

            IdentityResult result;

            do
            {
                var usersInRole = await userManager.GetUsersInRoleAsync("Administrator");
                if (usersInRole.Count > 0)
                {
                    break;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("admin user not found. Let's create a new one.");
                Console.ResetColor();

                Console.Write("Enter an username: ");
                var username = Console.ReadLine();

                Console.Write("Enter an email: ");
                var email = Console.ReadLine();

                Console.Write("Enter a password: ");
                var password = Console.ReadLine();

                var adminUser = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                };

                result = await userManager.CreateAsync(adminUser, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"administrator ({username} - {email}) user successfully created.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error creating admin user:");

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