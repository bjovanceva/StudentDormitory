using StudentDormitoryApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StudentDormitoryApp.Domain.DomainModels;

namespace StudentDormitoryApp.Service.Implementations
{
    public class ApplicationPdfService : IApplicationPdfService
    {
        public byte[] Generate(Application app)
        {
            return QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("Student Dormitory Application")
                        .FontSize(20)
                        .Bold();

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Application ID: {app.Id}");
                        col.Item().Text($"Status: {app.Status}");
                        col.Item().Text($"Comment: {app.Comment}");
                        col.Item().Text($"Date: {app.ApplicationDate}");
                        col.Item().Text($"Room ID: {app.RoomId}");
                        col.Item().Text($"User ID: {app.StudentDormitoryAppUserId}");
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text("Student Dormitory System");
                });
            }).GeneratePdf();
        }
    }
}
