public class DataSeeder
{
    public static async Task SeedAdminUser(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var existingAdmin = await userManager.FindByEmailAsync("elkawasyahmed@gmail.com");

            if (existingAdmin != null)
                return; //admin already created

            var response = await accountService.RegistrationAsAdminAsync(new RegisterDTO
            {
                Name = "AhmedAtef22",
                Email = "elkawasyahmed@gmail.com",
                PhoneNumber = "+201026408604",
                Password = "01026408604@Ahmed",
                ConfirmPassword = "01026408604@Ahmed"
            });

            if (response.StatusCode == 200)
            {
                // Manually confirm email
                User? user = await userManager.FindByEmailAsync("elkawasyahmed@gmail.com");
                if (user == null) return;
                user.EmailConfirmed = true;
                await unitOfWork.SaveChangesAsync();

                Console.WriteLine("✅ Admin user created successfully.");
            }
            else
            {
                Console.WriteLine($"❌ Failed to create admin user: {response.Message}");
            }
        }
    }
}
