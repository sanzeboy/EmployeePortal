using EmployeePortal.Application.Infrastructures;
using EmployeePortal.Application.Models;

namespace EmployeePortal.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        public FileDetail Export(string pdfContent,string fileName)
        {
            // Implement Logic to generate pdf from pdf content
            var pdf = File.ReadAllBytes("SamplePdf.pdf");

            return new FileDetail
            {
                FileName = fileName +".pdf",
                ContentType = "application/pdf",
                FileContents = pdf
            };
        }
    }
}
