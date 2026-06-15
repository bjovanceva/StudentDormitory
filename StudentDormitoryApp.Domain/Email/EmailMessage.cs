using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Domain.Email
{
    public class EmailMessage
    {
        public string? MailTo { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public byte[] Attachment { get; set; }
        public string AttachmentName { get; set; }
    }
}
