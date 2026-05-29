using Microsoft.EntityFrameworkCore;
using StudentDormitoryApp.Domain.DomainModels;
using StudentDormitoryApp.Repository.Interfaces;
using StudentDormitoryApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Service.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly IRepository<Room> _roomRepository;

        public RoomService(IRepository<Room> roomRepository)
        {
            _roomRepository = roomRepository;
        }
        public Room Add(Room room)
        {
            room.Id = Guid.NewGuid();
            return _roomRepository.Insert(room);
        }

        public Room DeleteById(Guid id)
        {
            Room room = GetById(id);
            return _roomRepository.Delete(room);
        }

        public List<Room> GetAll()
        {
            return _roomRepository.GetAll(selector: x=>x, include: x=>x.Include(y=>y.RoomImages).Include(y=>y.StudentDormitory).ThenInclude(z=>z.Documents).Include(y=>y.StudentDormitory).ThenInclude(z=>z.Users)).ToList();
        }

        public List<Room> GetAllByStudentDormitoryId(Guid studentDormitoryId)
        {
            return _roomRepository.GetAll(selector: x=>x, predicate: x=>x.StudentDormitoryId == studentDormitoryId, include: x => x.Include(y => y.RoomImages).Include(y => y.StudentDormitory).ThenInclude(z => z.Documents).Include(y => y.StudentDormitory).ThenInclude(z => z.Users)).ToList();
        }

        public Room? GetById(Guid id)
        {
            return _roomRepository.Get(selector: x=>x, predicate: x=>x.Id == id, include: x=>x.Include(y=>y.RoomImages).Include(y=>y.StudentDormitory));
        }

        public Room Update(Room room)
        {
            return _roomRepository.Update(room);
        }
    }
}
