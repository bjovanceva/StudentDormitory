using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentDormitoryApp.Domain.DomainModels;
using StudentDormitoryApp.Repository;
using StudentDormitoryApp.Service.Interfaces;

namespace StudentDormitoryApp.Web.Controllers
{
    public class RoomsController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IRoomService _roomService;
        private readonly IStudentDormitoryService _studentDormitoryService;
        private readonly IRoomImageService _roomImageService;

        public RoomsController(IRoomService roomService, IStudentDormitoryService studentDormitoryService, IRoomImageService roomImageService)
        {
            _roomService = roomService;
            _studentDormitoryService = studentDormitoryService;
            _roomImageService = roomImageService;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(Guid id)
        {
            return View(_roomService.GetAllByStudentDormitoryId(id));
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = _roomService.GetById(id.Value);
            if (room == null)
            {
                return NotFound();
            }
            ViewBag.RoomImages = room.RoomImages;

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create(Guid? id)
        {
            ViewBag.Orientations= new SelectList(Enum.GetValues<RoomOrientation>());
            ViewData["StudentDormitoryName"] = _studentDormitoryService.GetById(id.Value).Name;
            ViewData["StudentDormitoryId"] = _studentDormitoryService.GetById(id.Value).Id;

            //var room = new Room
            //{
            //    StudentDormitoryId = id.Value
            //};
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,BuildingSection,IsDuplex,HasPrivateBathroom,HasKitchen,BedCount,OccupiedBeds,Orientation,StudentDormitoryId,Id")] Room room, List<IFormFile>? Images)
        {
            if (ModelState.IsValid)
            {
                room.Id = Guid.NewGuid();
                _roomService.Add(room);

                if (Images != null)
                {
                    foreach (var file in Images)
                    {
                        var path = "/images/" + file.FileName;

                        using (var stream = new FileStream("wwwroot" + path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var roomImage = new RoomImage
                        {
                            RoomId = room.Id,
                            ImageUrl = path
                        };

                        _roomImageService.Add(roomImage);
                    }
                }

             return RedirectToAction(nameof(Index), new { Id = room.StudentDormitoryId });
            }

            //ViewData["StudentDormitoryId"] = new SelectList(_context.StudentDormitories, "Id", "Id", room.StudentDormitoryId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = _roomService.GetById(id.Value);
            if (room == null)
            {
                return NotFound();
            }
            ViewBag.Orientations = new SelectList(Enum.GetValues<RoomOrientation>());
           
            //ViewData["StudentDormitoryId"] = _studentDormitoryService.GetById(id.Value).Id;
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,BuildingSection,IsDuplex,HasPrivateBathroom,HasKitchen,BedCount,OccupiedBeds,Orientation,StudentDormitoryId,Id")] Room room, List<IFormFile>? Images)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _roomService.Update(room);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (Images != null)
                {
                    foreach (var file in Images)
                    {
                        var path = "/images/" + file.FileName;

                        using (var stream = new FileStream("wwwroot" + path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var roomImage = new RoomImage
                        {
                            RoomId = room.Id,
                            ImageUrl = path
                        };

                        _roomImageService.Add(roomImage);
                    }
                }

                    return RedirectToAction(nameof(Index), new { Id = room.StudentDormitoryId });
            }
            //ViewData["StudentDormitoryId"] = new SelectList(_context.StudentDormitories, "Id", "Id", room.StudentDormitoryId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = _roomService?.GetById(id.Value);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var room = _roomService.GetById(id);
            if (room != null)
            {
                _roomService.DeleteById(id);
            }

           
            return RedirectToAction(nameof(Index), new { Id = room.StudentDormitoryId });
        }

        private bool RoomExists(Guid id)
        {
            return _roomService.GetById(id)!=null;
        }
    }
}
