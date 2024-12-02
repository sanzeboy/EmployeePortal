using EmployeePortal.Application.Models;

namespace EmployeePortal.Application.Infrastructures
{
    public interface IPdfService
    {
        public FileDetail Export(string pdfContent, string fileName);

    }
}
