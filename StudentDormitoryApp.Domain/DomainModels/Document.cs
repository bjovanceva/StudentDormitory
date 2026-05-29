using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Domain.DomainModels
{
    public class Document : BaseEntity
    {
        public String? Name { get; set; }   
        public String? FilePath { get; set; }
        public Guid StudentDormitoryId { get; set; }
        public StudentDormitory? StudentDormitory { get; set; }
        public Guid? ApplicationId { get; set; }
        public Application? Application { get; set; }
    }
}
