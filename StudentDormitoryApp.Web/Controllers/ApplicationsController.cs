using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using StudentDormitoryApp.Domain.DomainModels;
using StudentDormitoryApp.Domain.Identity;
using StudentDormitoryApp.Repository;
using StudentDormitoryApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application = StudentDormitoryApp.Domain.DomainModels.Application;

namespace StudentDormitoryApp.Web.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly IApplicationService _applicationService;
        private readonly IRoomService _roomService;
        private readonly IStudentDormitoryService _studentDormitoryService;
        private readonly UserManager<StudentDormitoryAppUser> _userManager;


        public ApplicationsController(IApplicationService applicationService, IRoomService roomService, IStudentDormitoryService studentDormitoryService, UserManager<StudentDormitoryAppUser> userManager)
        {
            _applicationService = applicationService;
            _roomService = roomService;
            _studentDormitoryService = studentDormitoryService;
            _userManager = userManager;
        }

        //[Authorize(Roles = "Referent")]
        //GET: Applications
        public IActionResult Index()
        {
            return View(_applicationService.GetAll());
        }

        public IActionResult ListWithStatusPending()
        {
            return View("Index", _applicationService.GetAllWithStatusPending());
        }

        public IActionResult ListWithStatusApproved()
        {
            return View("Index", _applicationService.GetAllWithStatusApproved());
        }

        [HttpGet]
        public async Task<IActionResult> GetByMONId(String search)
        {
            var application = await _applicationService.GetByMONApplicationId(search);
            List<Application> applications = new List<Application>();
            if (application != null)
            {
                applications.Add(application);
            }
           
            return View("Index", applications);
        }



        //GET: Applications/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = _applicationService.GetById(id.Value);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Applications/Create
        public async Task<IActionResult> Create(Guid? roomId)
        {
            var room = _roomService.GetById(roomId.Value);
            ViewData["RoomId"] = roomId;
            ViewData["RoomName"] = room.Name;
            ViewData["StudentDormitoryName"] = _studentDormitoryService.GetById(room.StudentDormitoryId).Name;
            var user = await _userManager.GetUserAsync(User);
            ViewData["UserEmail"] = user.Email;
            ViewData["UserId"] = user.Id;
          
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Comment,RoomId,StudentDormitoryAppUserId,Id")] StudentDormitoryApp.Domain.DomainModels.Application application, List<IFormFile>? Documents)
        {

            if (ModelState.IsValid && Documents!=null)
            {
                StripeConfiguration.ApiKey = "enter api key here";

                var room = _roomService.GetById(application.RoomId);
                var studentDormitoryId = room.StudentDormitoryId;
                var studentDormitory = _studentDormitoryService.GetById(studentDormitoryId);

                application.Id = Guid.NewGuid();

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long?)((long?)studentDormitory.FirstMonthRent * 100 / 55.2371), 
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Dormitory Application Fee"
                        },
                    },
                    Quantity = 1,
                },
            },
                    Mode = "payment",
                    SuccessUrl = Url.Action("PaymentSuccess", "Applications", new { applicationId = application.Id }, Request.Scheme),
                    CancelUrl = Url.Action("PaymentCancelled", "Applications", null, Request.Scheme),
                };

                var service = new SessionService();
                var session = service.Create(options);

                application.ApplicationDate = DateTime.Now;
                await _applicationService.Add(application, Documents);

                return Redirect(session.Url);

               
                //return RedirectToAction(nameof(Index));
            }
            //ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", application.RoomId);
            return View(application);
        }

        // GET: Applications/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var application = await _context.Applications.FindAsync(id);
        //    if (application == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", application.RoomId);
        //    return View(application);
        //}

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("Status,comment,ApplicationDate,RoomId,StudentDormitoryAppUserId,Id")] Application application)
        //{
        //    if (id != application.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(application);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ApplicationExists(application.Id))
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
        //    ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Id", application.RoomId);
        //    return View(application);
        //}

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var application = _applicationService.DeleteById(id.Value);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Applications/PaymentSuccess
        public IActionResult PaymentSuccess(Guid applicationId)
        {
            ViewData["applicationId"] = applicationId;
            return View();
        }


        // GET: Applications/PaymentCancelled
        public IActionResult PaymentCancelled()
        {
            return View();
        }

        public IActionResult ApproveApplication(Guid? id, string Comment)
        {
            if (id == Guid.Empty)
                return BadRequest();

            _applicationService.ApproveApplication(id.Value, Comment);
            return RedirectToAction("ListWithStatusApproved");
        }

        // POST: Applications/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(Guid id)
        //{
        //    var application = await _context.Applications.FindAsync(id);
        //    if (application != null)
        //    {
        //        _context.Applications.Remove(application);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ApplicationExists(Guid id)
        //{
        //    return _context.Applications.Any(e => e.Id == id);
        //}
    }
}

