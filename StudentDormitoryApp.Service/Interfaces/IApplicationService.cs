using Microsoft.AspNetCore.Http;
using StudentDormitoryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Service.Interfaces
{
    public interface IApplicationService
    {
        Task<Application> Add(Application application, List<IFormFile> Documents);
        Application? GetById(Guid id);
        List<Application> GetAll();
        List<Application> GetAllWithStatusPending();
        List<Application> GetAllWithStatusApproved();
        List<Application> GetAllByUserId(Guid userId);

        Task<Application> GetByMONApplicationId(String monApplicationId);
        Application DeleteById(Guid id);
        Task<Application> ApproveApplication(Guid id, string comment);
        Task<Application> RejectApplication(Guid id, string comment);
    }
}
