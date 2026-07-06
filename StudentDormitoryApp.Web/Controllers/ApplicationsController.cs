using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using StudentDormitoryApp.Domain.DomainModels;
using StudentDormitoryApp.Domain.Email;
using StudentDormitoryApp.Domain.Identity;
using StudentDormitoryApp.Repository;
using StudentDormitoryApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly IEmailService _emailService;
        private readonly IApplicationPdfService _applicationPdfService;


        public ApplicationsController(IApplicationService applicationService, IRoomService roomService, IStudentDormitoryService studentDormitoryService, UserManager<StudentDormitoryAppUser> userManager, IConfiguration configuration, IEmailService emailService, IApplicationPdfService applicationPdfService)
        {
            _applicationService = applicationService;
            _roomService = roomService;
            _studentDormitoryService = studentDormitoryService;
            _userManager = userManager;
            _emailService = emailService;
            _applicationPdfService = applicationPdfService;
        }

        //GET: Applications
        [Authorize(Roles = "Student")]
        public IActionResult MyApplications()
        {
            var userId = _userManager.GetUserId(User);
            var applications = _applicationService.GetAllByUserId(Guid.Parse(userId));
            return View(applications);
        }

        [Authorize(Roles = "Referent")]
        public IActionResult Index()
        {
            return View(_applicationService.GetAll());
        }

        [Authorize(Roles = "Referent")]
        public IActionResult ListWithStatusPending()
        {
            return View("Index", _applicationService.GetAllWithStatusPending());
        }

        [Authorize(Roles = "Referent")]
        public IActionResult ListWithStatusApproved()
        {
            return View("Index", _applicationService.GetAllWithStatusApproved());
        }

        [HttpGet]
        [Authorize(Roles = "Referent")]
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
        [Authorize]
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

            TempData["IsAvailable"] = _roomService.isAvailable(application.RoomId);
            return View(application);
        }

        [Authorize(Roles = "Student")]
        // GET: Applications/Create
        public async Task<IActionResult> Create(Guid? roomId)
        {
            if (roomId == null)
            {
                return NotFound();
            }

            if (!_roomService.isAvailable(roomId.Value))
            {
                TempData["Message"] = "Нема слободни легла во оваа соба.";
                return RedirectToAction("Details", "Rooms", new { id = roomId });
            }
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
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create([Bind("Comment,RoomId,StudentDormitoryAppUserId,Id")] StudentDormitoryApp.Domain.DomainModels.Application application, List<IFormFile>? Documents)
        {
            async Task RepopulateViewData()
            {
                var roomForView = _roomService.GetById(application.RoomId);
                ViewData["RoomId"] = application.RoomId;
                ViewData["RoomName"] = roomForView?.Name;
                if (roomForView != null)
                {
                    ViewData["StudentDormitoryName"] = _studentDormitoryService.GetById(roomForView.StudentDormitoryId).Name;
                }
                var currentUserForView = await _userManager.GetUserAsync(User);
                ViewData["UserEmail"] = currentUserForView?.Email;
                ViewData["UserId"] = currentUserForView?.Id;
            }

            if (Documents == null || Documents.Count == 0 || Documents.All(d => d.Length == 0))
            {
                ModelState.AddModelError(string.Empty, "Мора да прикачиш барем еден документ за да поднесеш апликација.");
                await RepopulateViewData();
                return View(application);
            }

            if (ModelState.IsValid)
            {
                StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");

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
                    SuccessUrl = Url.Action("PaymentSuccess", "Applications", new { applicationId = application.Id }, Request.Scheme) + "&session_id={CHECKOUT_SESSION_ID}",
                    CancelUrl = Url.Action("PaymentCancelled", "Applications", new { applicationId = application.Id }, Request.Scheme),
                };

                var service = new SessionService();
                var session = service.Create(options);

                application.ApplicationDate = DateTime.Now;
                await _applicationService.Add(application, Documents);

                return Redirect(session.Url);

            }
            await RepopulateViewData();
            return View(application);
        }


        // GET: Applications/Delete/5
        [Authorize]
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
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> PaymentSuccess(Guid applicationId, string session_id)
        {
            //var currentUser = await _userManager.GetUserAsync(User);
            //if (currentUser == null)
            //    return RedirectToAction("Login", "Account");

            //var app = _applicationService.GetById(applicationId);
            //var pdfBytes = _applicationPdfService.Generate(app);

            //var emailMessage = new EmailMessage
            //{
            //    MailTo = currentUser.Email,
            //    Subject = "Успешна апликација",
            //    Content = $@"
            //               <p>Почитуван/а,</p>

            //               <p>Вашата апликација е успешна.</p>
            //               <p>Ќе добиете повратна информација веднаш по разгледувањето</p>
            //               <br/>
            //               <p>Во прилог ви ја испаќаме апликацијата во pdf формат</p>

            //               <p>Со почит,<br/>
            //               Student Dormitory System</p>",
            //    Attachment = pdfBytes,
            //    AttachmentName = "application.pdf"
            //};

            //await _emailService.SendEmailAsync(emailMessage);
            //ViewData["applicationId"] = applicationId;
            //return View();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");

            if (string.IsNullOrEmpty(session_id))
            {
                return RedirectToAction("PaymentCancelled", new { applicationId });
            }

            var sessionService = new SessionService();
            Session session;

            try
            {
                session = sessionService.Get(session_id);
            }
            catch (StripeException)
            {
                return RedirectToAction("PaymentCancelled", new { applicationId });
            }

            if (session.PaymentStatus != "paid")
            {
                return RedirectToAction("PaymentCancelled", new { applicationId });
            }

            var app = _applicationService.GetById(applicationId);
            var pdfBytes = _applicationPdfService.Generate(app);

            var emailMessage = new EmailMessage
            {
                MailTo = currentUser.Email,
                Subject = "Успешна апликација",
                Content = $@"
                   <p>Почитуван/а,</p>

                   <p>Вашата апликација е успешна.</p>
                   <p>Ќе добиете повратна информација веднаш по разгледувањето</p>
                   <br/>
                   <p>Во прилог ви ја испаќаме апликацијата во pdf формат</p>
                   
                   <p>Со почит,<br/>
                   Student Dormitory System</p>",
                Attachment = pdfBytes,
                AttachmentName = "application.pdf"
            };

            await _emailService.SendEmailAsync(emailMessage);
            ViewData["applicationId"] = applicationId;
            return View();
        }


        // GET: Applications/PaymentCancelled
        [Authorize(Roles = "Student")]
        public IActionResult PaymentCancelled(Guid? applicationId)
{
    if (applicationId != null && applicationId != Guid.Empty)
    {
        try
        {
            _applicationService.DeleteById(applicationId.Value);
        }
        catch (Exception)
        {
            // Апликацијата можеби веќе не постои - нема проблем.
        }
    }

    return View();
}

        [Authorize(Roles = "Referent")]
        public IActionResult HandleApplication(Guid? id, string Comment, string action)
        {
            if (id == Guid.Empty)
                return BadRequest();

            if (action == "approve")
            {
                _applicationService.ApproveApplication(id.Value, Comment);
                return RedirectToAction("ListWithStatusApproved");
            }
            else
            {
                _applicationService.RejectApplication(id.Value, Comment);
                return RedirectToAction("Index");
            }
        }
    }
}

