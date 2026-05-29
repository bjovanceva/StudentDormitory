using Microsoft.EntityFrameworkCore;
using StudentDormitoryApp.Domain.Identity;
using StudentDormitoryApp.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly DbSet<StudentDormitoryAppUser> entities;

        public UserRepository(ApplicationDbContext context) { 
       
            _context = context;
            this.entities = _context.Set<StudentDormitoryAppUser>();
        }

        public StudentDormitoryAppUser? FindUserById(string id)
        {
            return entities
                .Include(u => u.StudentDormitory) 
                .FirstOrDefault(u => u.Id == id);
        }

        public StudentDormitoryAppUser Insert(StudentDormitoryAppUser user)
        {
            _context.Add(user);
            _context.SaveChanges();
            return user;
        }
    }
}
