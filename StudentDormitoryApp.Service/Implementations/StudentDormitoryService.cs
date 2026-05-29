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
    public class StudentDormitoryService : IStudentDormitoryService
    {
        private readonly IRepository<StudentDormitory> _studentDormitoryRepository;

        public StudentDormitoryService(IRepository<StudentDormitory> studentDormitoryRepository)
        {
            _studentDormitoryRepository = studentDormitoryRepository;
        }
        public StudentDormitory Add(StudentDormitory studentDormitory)
        {
            studentDormitory.Id = Guid.NewGuid();
            return _studentDormitoryRepository.Insert(studentDormitory);
        }

        public StudentDormitory DeleteById(Guid id)
        {
            StudentDormitory studentDormitory = GetById(id);
            if (studentDormitory == null)
            {
                throw new Exception("Student dormitory not found");
            }
            return _studentDormitoryRepository.Delete(studentDormitory);
        }

        public List<StudentDormitory> GetAll()
        {
            return _studentDormitoryRepository.GetAll(selector: x=>x, include: x => x.Include(y => y.Users).Include(y => y.Documents).ThenInclude(z => z.Application)).ToList();
        }

        public StudentDormitory? GetById(Guid id)
        {
            return _studentDormitoryRepository.Get(selector: x => x, predicate: x => x.Id == id, include: x => x.Include(y => y.Documents));
        }

        public StudentDormitory Update(StudentDormitory travelPackage)
        {
            return _studentDormitoryRepository.Update(travelPackage);
        }

        public StudentDormitory FindByName(string name)
        {
            return _studentDormitoryRepository.Get(selector: x => x, predicate: x => x.Name.Equals(name));
        }
    }
}
