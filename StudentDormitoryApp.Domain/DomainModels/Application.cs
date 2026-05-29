using StudentDormitoryApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Domain.DomainModels
{
    public class Application : BaseEntity
    {
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        public String? Comment { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? ApplicationDate { get; set; }
        public Guid RoomId { get; set; }
        public Room? Room { get; set; }

        public List<Document>? Documents { get; set; } = new List<Document>();

        public Guid StudentDormitoryAppUserId { get; set; }
        public StudentDormitoryAppUser? StudentDormitoryAppUser { get; set; }
    }
}
