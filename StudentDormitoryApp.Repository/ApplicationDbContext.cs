using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentDormitoryApp.Domain.DomainModels;
using StudentDormitoryApp.Domain.Identity;

namespace StudentDormitoryApp.Repository
{
    public class ApplicationDbContext : IdentityDbContext<StudentDormitoryAppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //private readonly IConfiguration _configuration;

        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        //    : base(options)
        //{
        //    _configuration = configuration;
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        //        optionsBuilder.UseNpgsql(connectionString);
        //    }
        //}

        public virtual DbSet<StudentDormitory> StudentDormitories { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<RoomImage> RoomImages { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
    }
}
