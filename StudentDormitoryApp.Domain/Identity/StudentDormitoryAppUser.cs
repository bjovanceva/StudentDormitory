using Microsoft.AspNetCore.Identity;
using StudentDormitoryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Domain.Identity
{
    public class StudentDormitoryAppUser : IdentityUser
    {
        public String? FirstName { get; set; }
        public String? LastName { get; set; }
        public String? Address { get; set; }
        public String? Photo { get; set; }


        public String? Faculty { get; set; }
        public int? StudyYear { get; set; }
        public String? MONApplicationId { get; set; }

        public Guid? StudentDormitoryId { get; set; }
      
        public StudentDormitory? StudentDormitory { get; set; }

        public Guid? RoomId { get; set; }
        public Room? Room { get; set; }

        public bool ProfileCompleted { get; set; } = false;

    }
}
