using StudentDormitoryApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Domain.DomainModels
{
    public class StudentDormitory : BaseEntity
    {
        public String? Name { get; set; }
        public String? Photo { get; set; }
        public String? Address { get; set; }
        public int RoomCount { get; set; }
        public int BedCount { get; set; }
        public bool HasReadingRoom { get; set; }
        public bool HasCafeteria { get; set; }
        public decimal RentPerMonth { get; set; }
        public decimal FirstMonthRent { get; set; }
        public List<Document> Documents { get; set; } = new List<Document>();
        public List<StudentDormitoryAppUser> Users { get; set; } = new List<StudentDormitoryAppUser>();
    }
}
