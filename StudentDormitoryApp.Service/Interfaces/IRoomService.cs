using StudentDormitoryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Service.Interfaces
{
    public interface IRoomService
    {
        List<Room> GetAll();
        Room? GetById(Guid id);
        Room Add(Room room);
        Room Update(Room room);
        Room DeleteById(Guid id);

        List<Room> GetAllByStudentDormitoryId(Guid studentDormitoryId);
    }
}
