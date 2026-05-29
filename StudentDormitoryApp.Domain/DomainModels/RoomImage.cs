using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Domain.DomainModels
{
    public class RoomImage : BaseEntity
    {
        public string ImageUrl { get; set; } = null!;

        public Guid RoomId { get; set; }
        public Room? Room { get; set; }
    }
}
