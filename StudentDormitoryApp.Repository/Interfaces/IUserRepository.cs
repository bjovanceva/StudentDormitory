using StudentDormitoryApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Repository.Interfaces
{
    public interface IUserRepository
    {
        StudentDormitoryAppUser FindUserById(string id);
        StudentDormitoryAppUser Insert(StudentDormitoryAppUser user);
    }
}
