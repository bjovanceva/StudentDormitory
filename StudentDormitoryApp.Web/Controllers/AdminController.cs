using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using StudentDormitoryApp.Domain.DomainModels;
using StudentDormitoryApp.Domain.DTO;
using StudentDormitoryApp.Domain.Identity;
using StudentDormitoryApp.Service.Interfaces;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


namespace StudentDormitoryApp.Web.Controllers
{
    public class AdminController : Controller
    {

        private readonly IAdminService _adminService;
        private readonly IStudentDormitoryService _studentDormitoryService;

        public AdminController(IAdminService adminService, IStudentDormitoryService studentDormitoryService)
        {
            _adminService = adminService;
            _studentDormitoryService = studentDormitoryService;
        }

        public IActionResult AddStudentsCSVFile()
        {
            return View();
        }

        public IActionResult RegisterReferentUser()
        {
            var studentDormitories = _studentDormitoryService.GetAll();
            ViewData["StudentDormitoryId"] = new SelectList(studentDormitories, "Id", "Name");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddStudentsCSVFile(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                TempData["Message"] = "Please select a valid CSV file.";
                return RedirectToAction(nameof(AddStudentsCSVFile));
            }

            bool result = await _adminService.AddStudentsCSVFileAsync(csvFile);
            if (result)
            {
                TempData["Message"] = "File uploaded successfully!";
                return RedirectToAction(nameof(AddStudentsCSVFile));
            }

            TempData["Message"] = "Try again";
            return RedirectToAction(nameof(AddStudentsCSVFile));

        }


        [HttpPost]
        public async Task<IActionResult> RegisterReferentUser(RegisterReferentUser registerReferentUser)
        {
            if (registerReferentUser == null)
            {
                TempData["Message"] = "The user is null";
                return RedirectToAction(nameof(RegisterReferentUser));
            }

             await _adminService.RegisterReferentUser(registerReferentUser);

            TempData["Message"] = "User added successfully";
            return RedirectToAction(nameof(RegisterReferentUser)); 
        }
    }
}
