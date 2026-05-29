using StudentDormitoryApp.Domain.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Service.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
