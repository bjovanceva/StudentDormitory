using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentDormitoryApp.Domain.DomainModels;
using StudentDormitoryApp.Domain.Email;
using StudentDormitoryApp.Domain.Identity;
using StudentDormitoryApp.Repository.Interfaces;
using StudentDormitoryApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Service.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IRepository<Application> _applicationRepository;
        private readonly IDocumentService _documentService;
        private readonly IRoomService _roomService;
        private readonly UserManager<StudentDormitoryAppUser> _userManager;
        private readonly IEmailService _emailService;

        public ApplicationService(IRepository<Application> applicationRepository, IDocumentService documentService, IRoomService roomService, UserManager<StudentDormitoryAppUser> userManager, IEmailService emailService)
        {
            _applicationRepository = applicationRepository;
            _documentService = documentService;
            _roomService = roomService;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<Application> Add(Application application, List<IFormFile> Documents)
        {
            List<Document> documents = new List<Document>();

            if (Documents != null)
            {
                var room = _roomService.GetById(application.RoomId);
                application.Room = room;

                foreach (var file in Documents)
                {
                    var path = "/documents/" + file.FileName;

                    using (var stream = new FileStream("wwwroot" + path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }


                    var document = new Document
                    {
                        ApplicationId = application.Id,
                        StudentDormitoryId = room.StudentDormitoryId,
                        FilePath = path
                    };

                    documents.Add(document);

                    //_documentService.Add(document);
                }
            }

            application.Documents = documents;
            return _applicationRepository.Insert(application);
        }

        public Application? GetById(Guid id)
        {
            return _applicationRepository.Get(selector: x => x, predicate: x => x.Id.Equals(id), include: x => x.Include(y => y.Room).ThenInclude(z => z.StudentDormitory).Include(y => y.Documents));
        }

        public Application DeleteById(Guid id)
        {
            Application application = GetById(id);

            if (application.Documents != null && application.Documents.Any())
            {
                foreach (var doc in application.Documents.ToList())
                {
                    _documentService.Delete(doc);
                }
            }

            return _applicationRepository.Delete(application);
        }

        public List<Application> GetAll()
        {
            return _applicationRepository.GetAll(selector: x => x, include: x => x.Include(y => y.Room).ThenInclude(z => z.StudentDormitory).Include(y => y.StudentDormitoryAppUser)).ToList();
        }

        public List<Application> GetAllWithStatusPending()
        {
            return _applicationRepository.GetAll(selector: x => x, predicate: x => x.Status.Equals(ApplicationStatus.Pending), include: x => x.Include(y => y.Room).ThenInclude(z => z.StudentDormitory).Include(y => y.StudentDormitoryAppUser)).OrderBy(a => a.ApplicationDate).ToList();
        }

        public List<Application> GetAllWithStatusApproved()
        {
            return _applicationRepository.GetAll(selector: x => x, predicate: x => x.Status.Equals(ApplicationStatus.Approved), include: x => x.Include(y => y.Room).ThenInclude(z => z.StudentDormitory).Include(y => y.StudentDormitoryAppUser)).OrderBy(a => a.ApplicationDate).ToList();
        }

        public List<Application> GetAllByUserId(Guid userId)
        {
            return _applicationRepository.GetAll(selector: x => x, predicate: x => x.StudentDormitoryAppUserId.Equals(userId), include: x => x.Include(y => y.Room).ThenInclude(z => z.StudentDormitory)).OrderByDescending(a => a.ApplicationDate).ToList();
        }

        public async Task<Application> GetByMONApplicationId(String monApplicationId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.MONApplicationId.Equals(monApplicationId));
            if (user == null)
            {
               return null;
            }
            var app = _applicationRepository.Get(selector: x => x, predicate: x => x.StudentDormitoryAppUserId.Equals(Guid.Parse(user.Id)), include: x => x.Include(y => y.Room).ThenInclude(z => z.StudentDormitory).Include(y => y.StudentDormitoryAppUser));
            return app; 
        }

        public async Task<Application> ApproveApplication(Guid id, string comment)
        {
            var application = GetById(id);
            application.Status = ApplicationStatus.Approved;
            _applicationRepository.Update(application);

            var room = _roomService.GetById(application.RoomId);
            room.OccupiedBeds += 1;
            _roomService.Update(room);

            var user = await _userManager.FindByIdAsync(application.StudentDormitoryAppUserId.ToString());

            try
            {
                var emailMessage = new EmailMessage
                {
                    MailTo = user.Email,
                    Subject = "Информација за статус на поднесената апликација",
                    Content = comment
                };

                await _emailService.SendEmailAsync(emailMessage);
            }
            catch (Exception)
            {
                // Проблем со email не смее да го спречи веќе завршеното одобрување/ажурирање на собата.
            }

            return application;
        }

        public async Task<Application> RejectApplication(Guid id, string comment)
        {
            var application = GetById(id);
            application.Status = ApplicationStatus.Rejected;
            _applicationRepository.Update(application);

            var user = await _userManager.FindByIdAsync(application.StudentDormitoryAppUserId.ToString());

            try
            {
                var emailMessage = new EmailMessage
                {
                    MailTo = user.Email,
                    Subject = "Информација за статус на поднесената апликација",
                    Content = comment
                };

                await _emailService.SendEmailAsync(emailMessage);
            }
            catch (Exception)
            {
                // Отфрлањето веќе е зачувано - проблем со email не смее да прикаже грешка.
            }

            return application;
        }
    }
}
