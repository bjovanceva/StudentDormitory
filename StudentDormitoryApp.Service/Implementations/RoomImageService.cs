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
    public class RoomImageService : IRoomImageService
    {
        private readonly IRepository<RoomImage> _roomImageRepository;

        public RoomImageService(IRepository<RoomImage> roomImageRepository)
        {
            _roomImageRepository = roomImageRepository;
        }
        public RoomImage Add(RoomImage roomImage)
        {
            roomImage.Id = Guid.NewGuid();
            return _roomImageRepository.Insert(roomImage);
        }

        public List<RoomImage> GetRoomImagesByRoomId(Guid id)
        {
            return _roomImageRepository.GetAll(selector: x=>x, predicate: x=>x.RoomId==id, include: x=>x.Include(y=>y.Room)).ToList();
        }
    }
}
