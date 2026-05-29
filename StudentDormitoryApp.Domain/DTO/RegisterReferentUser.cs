using StudentDormitoryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Domain.DTO
{
    public class RegisterReferentUser
    {
        [Required(ErrorMessage = "Името е задолжително")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Презимето е задолжително")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Адресата на живеење е задолжителна")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Е-маил адресата е задолжителна")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Студентскиот дом е задолжителен")]
        public Guid StudentDormitoryId { get; set; } 
        public StudentDormitory StudentDormitory { get; set; }

    }
}
