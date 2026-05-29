using Microsoft.AspNetCore.Http;
using StudentDormitoryApp.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Service.Interfaces
{
    public interface IAdminService
    {
        Task<bool> AddStudentsCSVFileAsync(IFormFile csvFile);

        Task RegisterReferentUser(RegisterReferentUser registerReferentUser);
    }
}
