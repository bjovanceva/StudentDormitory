using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StudentDormitoryApp.Domain.DomainModels;
using StudentDormitoryApp.Domain.DTO;
using StudentDormitoryApp.Domain.Email;
using StudentDormitoryApp.Domain.Identity;
using StudentDormitoryApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Service.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<StudentDormitoryAppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IStudentDormitoryService _studentDormitoryService;
        private readonly IEmailService _emailService;

        public AdminService(UserManager<StudentDormitoryAppUser> userManager, RoleManager<IdentityRole> roleManager, IStudentDormitoryService studentDormitoryService,  IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _studentDormitoryService = studentDormitoryService;
            _emailService = emailService;
        }

       

        public string GeneratePassword(int length = 12)
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


        public async Task<bool> AddStudentsCSVFileAsync(IFormFile csvFile)
        {
           
            var reader = new StreamReader(csvFile.OpenReadStream());

            //string text = reader.ReadToEnd();

            List<string> lines = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                lines.Add(line);
            }
            reader.Close();

            for (int i = 1; i < lines.Count; i++)
            {
                string[] line = lines[i].Split(",");

                var existingUser = await _userManager.FindByEmailAsync(line[0]);
                if (existingUser != null)
                    continue;

                var studentDormitory = _studentDormitoryService.FindByName(line[6]);

                var student = new StudentDormitoryAppUser
                {
                    Email = line[0],
                    UserName = line[0],
                    FirstName = line[1],
                    LastName = line[2],
                    Address = line[3],
                    Faculty = line[4],
                    StudyYear = int.Parse(line[5]),
                    StudentDormitory = studentDormitory,
                    StudentDormitoryId = studentDormitory.Id,
                    MONApplicationId = line[7]

                };

                string password = GeneratePassword();
                var result = await _userManager.CreateAsync(student, password);


                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(student, UserRole.Student.ToString());

                    var emailMessage = new EmailMessage
                    {
                        MailTo = line[0],
                        Subject = "Податоци за најава во системот",
                        Content = $@"
                           <p>Почитуван/а,</p>


                           <p>Во прилог се податоците за вашата најава во системот за студентски дом:</p>

                           <p><strong>Username:</strong> {line[0]}<br/>
                           <strong>Password:</strong> {password}</p>

                           <p>Ве молиме сменете ја лозинката при вашата прва најава.</p>


                           <p>Со почит,<br/>
                           Student Dormitory System</p>"
                    };

                    await _emailService.SendEmailAsync(emailMessage);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public async Task RegisterReferentUser(RegisterReferentUser registerReferentUser)
        {

            var existingUser = await _userManager.FindByEmailAsync(registerReferentUser.Email);
            if (existingUser != null)
                return;

            var referent = new StudentDormitoryAppUser
            {
                FirstName = registerReferentUser.FirstName,
                UserName = registerReferentUser.Email,
                LastName = registerReferentUser.LastName,
                Email = registerReferentUser.Email,
                Address = registerReferentUser.Address,
                StudentDormitoryId = registerReferentUser.StudentDormitoryId
            };

            string password = GeneratePassword();
            var result = await _userManager.CreateAsync(referent, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(referent, UserRole.Referent.ToString());

                var emailMessage = new EmailMessage
                {
                    MailTo = registerReferentUser.Email,
                    Subject = "Податоци за најава во системот",
                    Content = $@"
                           <p>Почитуван/а,</p>


                           <p>Во прилог се податоците за вашата најава во системот за студентски дом:</p>

                           <p><strong>Username:</strong> {registerReferentUser.Email}<br/>
                           <strong>Password:</strong> {password}</p>

                           <p>Ве молиме сменете ја лозинката при вашата прва најава.</p>


                           <p>Со почит,<br/>
                           Student Dormitory System</p>"
                };

                await _emailService.SendEmailAsync(emailMessage);
            }
        }
    }
}
