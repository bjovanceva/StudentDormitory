using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Domain.Email
{
    public class MailSettings
    {
        public string? SmtpServer { get; set; }
        public int SmtpServerPort { get; set; }
        public string? EmailDisplayName { get; set; }
        public string? SenderName { get; set; }
        public string? SmtpUsername { get; set; }
        public string? SmtpPassword { get; set; }
        public Boolean EnableSsl { get; set; }
    }
}
