using StudentDormitoryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Service.Interfaces
{
    public interface IStudentDormitoryService
    {
        List<StudentDormitory> GetAll();
        StudentDormitory? GetById(Guid id);
        StudentDormitory Add(StudentDormitory studentDormitory);
        StudentDormitory Update(StudentDormitory travelPackage);
        StudentDormitory DeleteById(Guid id);

        StudentDormitory FindByName(String name);
    }
}
