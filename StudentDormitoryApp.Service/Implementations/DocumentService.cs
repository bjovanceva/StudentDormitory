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
    public class DocumentService : IDocumentService
    {
        private readonly IRepository<Document> _documentRepository;

        public DocumentService(IRepository<Document> documentRepository)
        {
            _documentRepository = documentRepository;
        }
        public Document Add(Document document)
        {
            document.Id = Guid.NewGuid();
            return _documentRepository.Insert(document);
        }
        public Document Delete(Document document)
        {
            return _documentRepository.Delete(document);
        }
    }
}
