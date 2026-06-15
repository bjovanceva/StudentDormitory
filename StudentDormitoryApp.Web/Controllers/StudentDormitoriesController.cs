using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentDormitoryApp.Domain.DomainModels;
using StudentDormitoryApp.Repository;
using StudentDormitoryApp.Service.Implementations;
using StudentDormitoryApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace StudentDormitoryApp.Web.Controllers
{
    public class StudentDormitoriesController : Controller
    {
        private readonly IStudentDormitoryService _studentDormitoryService;
        private readonly IDocumentService _documentService;

        public StudentDormitoriesController(IStudentDormitoryService studentDormitoryService, IDocumentService documentService)
        {
            _studentDormitoryService = studentDormitoryService;
            _documentService = documentService;
        }

        // GET: StudentDormitories
        [Authorize]
        public IActionResult Index()
        {
            return View(_studentDormitoryService.GetAll());
        }

        // GET: StudentDormitories/Details/5
        [Authorize]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentDormitory = _studentDormitoryService.GetById(id.Value);
            if (studentDormitory == null)
            {
                return NotFound();
            }

            return View(studentDormitory);
        }

        // GET: StudentDormitories/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentDormitories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Name,Photo,Address,RoomCount,BedCount,HasReadingRoom,HasCafeteria,RentPerMonth,FirstMonthRent,Id")] StudentDormitory studentDormitory, List<IFormFile>? Documents)
        {
            if (ModelState.IsValid)
            {
                studentDormitory.Id = Guid.NewGuid();
                _studentDormitoryService.Add(studentDormitory);

                if (Documents != null)
                {
                    foreach (var file in Documents)
                    {
                        var path = "/documents/" + file.FileName;

                        using (var stream = new FileStream("wwwroot" + path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var document = new Document
                        {
                            StudentDormitoryId = studentDormitory.Id,
                            FilePath = path
                        };

                        _documentService.Add(document);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(studentDormitory);
        }

        // GET: StudentDormitories/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentDormitory = _studentDormitoryService.GetById(id.Value);
            if (studentDormitory == null)
            {
                return NotFound();
            }
            return View(studentDormitory);
        }

        // POST: StudentDormitories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Photo,Address,RoomCount,BedCount,HasReadingRoom,HasCafeteria,RentPerMonth,FirstMonthRent,Id")] StudentDormitory studentDormitory, List<IFormFile>? Documents)
        {
            if (id != studentDormitory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _studentDormitoryService.Update(studentDormitory);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentDormitoryExists(studentDormitory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (Documents != null)
                {
                    foreach (var file in Documents)
                    {
                        var path = "/documents/" + file.FileName;

                        using (var stream = new FileStream("wwwroot" + path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var document = new Document
                        {
                            StudentDormitoryId = studentDormitory.Id,
                            FilePath = path
                        };

                        _documentService.Add(document);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(studentDormitory);
        }

        // GET: StudentDormitories/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentDormitory = _studentDormitoryService.GetById(id.Value);
            if (studentDormitory == null)
            {
                return NotFound();
            }

            return View(studentDormitory);
        }

        // POST: StudentDormitories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(Guid id)
        {

            _studentDormitoryService.DeleteById(id);
            return RedirectToAction(nameof(Index));
        }

        private bool StudentDormitoryExists(Guid id)
        {
            return _studentDormitoryService.GetById(id) != null;
        }
    }
}
