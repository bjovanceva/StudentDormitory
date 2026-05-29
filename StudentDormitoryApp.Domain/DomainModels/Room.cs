using StudentDormitoryApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Domain.DomainModels
{
    public class Room : BaseEntity
    {
        public String? Name { get; set; }
        public String? BuildingSection { get; set; }
        public bool IsDuplex { get; set; }
        public bool HasPrivateBathroom { get; set; }
        public bool HasKitchen { get; set; }
        public int BedCount { get; set; }
        public int OccupiedBeds { get; set; } = 0;
        public RoomOrientation Orientation { get; set; }
        public List<RoomImage> RoomImages { get; set; } = new List<RoomImage>();

        public List<StudentDormitoryAppUser> students = new List<StudentDormitoryAppUser>();
        public Guid StudentDormitoryId { get; set; }
        public StudentDormitory? StudentDormitory { get; set; }
    }
}