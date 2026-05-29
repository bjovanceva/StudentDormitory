using Microsoft.AspNetCore.Identity;
using StudentDormitoryApp.Domain.DomainModels;
using StudentDormitoryApp.Domain.Identity;
using System.Text;

namespace StudentDormitoryApp.Web.Data
{
    public static class DbInitializer
    {

        public static string GeneratePassword(int length = 12)
        {
            if (length < 4)
                throw new ArgumentException("Password length must be at least 4 to include all character types.");

            Random random = new Random();

            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()-_=+";


            StringBuilder password = new StringBuilder();
            password.Append(upper[random.Next(upper.Length)]);
            password.Append(lower[random.Next(lower.Length)]);
            password.Append(digits[random.Next(digits.Length)]);
            password.Append(special[random.Next(special.Length)]);


            string allChars = upper + lower + digits + special;
            for (int i = password.Length; i < length; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }


            return Shuffle(password.ToString(), random);
        }

        private static string Shuffle(string str, Random random)
        {
            char[] array = str.ToCharArray();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
            return new string(array);
        }


        public static async Task SeedRolesAndAdmin(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<StudentDormitoryAppUser>>();

            string adminEmail = "admin@admin.com";
            string adminPassword = "Admin123!";


            foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }


            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new StudentDormitoryAppUser
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Address = "",
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    ProfileCompleted = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, UserRole.Admin.ToString());
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }
    }
}
