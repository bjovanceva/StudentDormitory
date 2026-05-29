using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentDormitoryApp.Domain.DomainModels;
using StudentDormitoryApp.Repository;

namespace StudentDormitoryApp.Web.Controllers
{
    public class RoomImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RoomImages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RoomImages.Include(r => r.Room);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RoomImages/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomImage = await _context.RoomImages
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomImage == null)
            {
                return NotFound();
            }

            return View(roomImage);
        }

        // GET: RoomImages/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id");
            return View();
        }

        // POST: RoomImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageUrl,RoomId,Id")] RoomImage roomImage)
        {
            if (ModelState.IsValid)
            {
                roomImage.Id = Guid.NewGuid();
                _context.Add(roomImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", roomImage.RoomId);
            return View(roomImage);
        }

        // GET: RoomImages/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomImage = await _context.RoomImages.FindAsync(id);
            if (roomImage == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", roomImage.RoomId);
            return View(roomImage);
        }

        // POST: RoomImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ImageUrl,RoomId,Id")] RoomImage roomImage)
        {
            if (id != roomImage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomImageExists(roomImage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", roomImage.RoomId);
            return View(roomImage);
        }

        // GET: RoomImages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomImage = await _context.RoomImages
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roomImage == null)
            {
                return NotFound();
            }

            return View(roomImage);
        }

        // POST: RoomImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var roomImage = await _context.RoomImages.FindAsync(id);
            if (roomImage != null)
            {
                _context.RoomImages.Remove(roomImage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomImageExists(Guid id)
        {
            return _context.RoomImages.Any(e => e.Id == id);
        }
    }
}

