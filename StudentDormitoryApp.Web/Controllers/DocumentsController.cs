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
    public class DocumentsController : Controller
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        // GET: Documents
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Documents.Include(d => d.StudentDormitory);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        //// GET: Documents/Details/5
        //public async Task<IActionResult> Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var document = await _context.Documents
        //        .Include(d => d.StudentDormitory)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (document == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(document);
        //}

        //// GET: Documents/Create
        //public IActionResult Create()
        //{
        //    ViewData["StudentDormitoryId"] = new SelectList(_context.StudentDormitories, "Id", "Id");
        //    return View();
        //}

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,FilePath,StudentDormitoryId,Id")] Document document)
        {
            if (ModelState.IsValid)
            {
                document.Id = Guid.NewGuid();
                _documentService.Add(document);
                return RedirectToAction(nameof(Index));
            }
            //ViewData["StudentDormitoryId"] = new SelectList(_context.StudentDormitories, "Id", "Id", document.StudentDormitoryId);
            return View(document);
        }

        // GET: Documents/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var document = await _context.Documents.FindAsync(id);
        //    if (document == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["StudentDormitoryId"] = new SelectList(_context.StudentDormitories, "Id", "Id", document.StudentDormitoryId);
        //    return View(document);
        //}

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("Name,FilePath,StudentDormitoryId,Id")] Document document)
        //{
        //    if (id != document.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(document);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DocumentExists(document.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["StudentDormitoryId"] = new SelectList(_context.StudentDormitories, "Id", "Id", document.StudentDormitoryId);
        //    return View(document);
        //}

        // GET: Documents/Delete/5
        //public async Task<IActionResult> Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var document = await _context.Documents
        //        .Include(d => d.StudentDormitory)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (document == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(document);
        //}

        // POST: Documents/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var document = await _context.Documents.FindAsync(id);
        //    if (document != null)
        //    {
        //        _context.Documents.Remove(document);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool DocumentExists(Guid id)
        //{
        //    return _context.Documents.Any(e => e.Id == id);
        //}
    }
}

