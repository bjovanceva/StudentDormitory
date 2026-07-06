using StudentDormitoryApp.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Service.Interfaces
{
    public interface IDocumentService
    {
        Document Add(Document document);
        Document Delete(Document document);
    }
}
