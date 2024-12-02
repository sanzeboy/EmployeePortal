using EmployeePortal.Application.Events;
using EmployeePortal.Application.Exceptions;
using EmployeePortal.Application.Extensions;
using EmployeePortal.Application.Infastructures;
using EmployeePortal.Application.Infrastructures;
using EmployeePortal.Application.Models;
using EmployeePortal.Application.Services.Employees.Events;
using EmployeePortal.Application.Services.Employees.Models.Commands;
using EmployeePortal.Application.Services.Employees.Models.Dtos;
using EmployeePortal.Application.Services.Employees.Models.Filters;
using EmployeePortal.Domain.Entities.Employees;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.Application.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IApplicationDbContext _context;
        private readonly IExcelService _excelService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPdfService _pdfService;
        private readonly ICsvService _csvService;
        private readonly IEventPublisher _eventPublisher;

        public EmployeeService(IApplicationDbContext context,
            IExcelService excelService,
            ICurrentUserService currentUserService,
            IDateTime dateTime,
            IHostingEnvironment hostingEnvironment,
            IHttpClientFactory httpClientFactory,
            IPdfService pdfService,
            ICsvService csvService,
            IEventPublisher eventPublisher
            )
        {
            _context = context;
            _excelService = excelService;
            _currentUserService = currentUserService;
            _dateTime = dateTime;
            _hostingEnvironment = hostingEnvironment;
            _httpClientFactory = httpClientFactory;
            _pdfService = pdfService;
            _csvService = csvService;
            _eventPublisher = eventPublisher;
        }


        public async Task<int> InsertUpdateEmployee(InsertUpdateEmployee request)
        {
            if (!_currentUserService.IsAuthenticated)
                throw new UnAuthorizedException("User is not authorized");

            Employee employee;
            if (request.Id == 0)
            {
                employee = new Employee();
                _context.Employees.Add(employee);
            }
            else
            {
                employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == request.Id);
                if (employee == null)
                    throw new NotFoundException("Employee not found");
            }

            employee.Designation = request.Designation;
            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.DOB = request.DOB;
            employee.Gender = request.Gender;
            employee.Salary = request.Salary;

            await _context.SaveChangesAsync();
            return employee.Id;

        }

        public async Task<PagedResult<EmployeeDto>> GetEmployees(EmployeeFilter filter)
        {
            var employees = _context.Employees
                .Select(e => new EmployeeDto
                {
                    Designation = e.Designation,
                    Salary = e.Salary,
                    DOB = e.DOB,
                    FirstName = e.FirstName,
                    Gender = e.Gender,
                    Id = e.Id,
                    LastName = e.LastName,
                    ProfileImage = e.ProfileImage,
                })
                .AsQueryable();

            if (filter != null)
            {

                if (filter.Gender.HasValue)
                    employees = employees.Where(p => p.Gender == filter.Gender);


                if (filter.DobFrom.HasValue)
                {
                    if (!filter.DobTo.HasValue)
                        filter.DobTo = _dateTime.Now;
                    employees = employees.Where(a => a.DOB.Date >= filter.DobFrom.Value.Date && a.DOB.Date <= filter.DobTo.Value.Date);
                }

                if (filter.SalaryFrom.HasValue)
                    employees = employees.Where(a => a.Salary >= filter.SalaryFrom);


                if (filter.SalaryTo.HasValue)
                    employees = employees.Where(a => a.Salary <= filter.SalaryTo);


                // Search by ticket
                if (!string.IsNullOrWhiteSpace(filter.Search))
                {
                    filter.Search = filter.Search.Trim();
                    employees = employees.Where(c =>
                               c.FirstName.Contains(filter.Search)
                              || c.LastName.Contains(filter.Search)
                              || (c.FirstName + " " + c.LastName).Contains(filter.Search)
                              || c.Designation.Contains(filter.Search)
                               );

                }

            }
            return await employees
                 .GetPagedAsync(filter);

        }

        public async Task<EmployeeDto> GetEmployeeById(int id)
        {

            var employee = await _context.Employees
                .FirstOrDefaultAsync(c => c.Id == id);

            if (employee == null)
                throw new NotFoundException("Employee Not Found");
            return new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                ProfileImage = employee.ProfileImage,
                Designation = employee.Designation,
                DOB = employee.DOB,
                Salary = employee.Salary,
            };

        }

        public async Task<List<EmployeeDto>> GetEmployeesByIds(List<int> employeeIds)
        {
            if (employeeIds == null || !employeeIds.Any())
                return null;

            return await _context.Employees
                    .Where(e => employeeIds.Contains(e.Id))
                    .Select(e => new EmployeeDto
                    {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Gender = e.Gender,
                        ProfileImage = e.ProfileImage,
                        Designation = e.Designation,
                        DOB = e.DOB,
                        Salary = e.Salary,
                    }).ToListAsync();

        }

        public async Task<bool> InsertEmployeeProfileImage(InsertEmployeeProfileImage profileImage)
        {

            if (profileImage == null || profileImage.Image == null)
                throw new BadRequestException("No Image");

            if (!profileImage.Image.IsImage())
                throw new BadRequestException("Invalid Image, Only image files are supported",
                    "Attempt to upload invalid Image");

            var employee = await _context.Employees
                 .FirstOrDefaultAsync(c => c.Id == profileImage.Id);

            if (employee == null)
                throw new NotFoundException("Employee not found");


            try
            {
                var fileExtension = Path.GetExtension(profileImage.Image.FileName);
                var fileName = $"{profileImage.Image.FileName.SafeSubstring(15)}{DateTime.Now.Ticks}{fileExtension}";

                string folderName = $"Images";
                string webRootPath = _hostingEnvironment.ContentRootPath;
                string folderPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var localFilePath = Path.Combine(folderPath, fileName);

                var stream = profileImage.Image.OpenReadStream();
                using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                {
                    stream.CopyTo(fileStream);
                }

                if (!File.Exists(localFilePath))
                    throw new BadRequestException("Error", "Unable to save the file.");

                employee.ProfileImage = folderName + "/" + fileName;

            }
            catch (Exception)
            {

                throw;
            }
            return (await _context.SaveChangesAsync()) > 0;
        }


        #region Employee Import export

        public async Task<ImportResponse<ImportExportEmployee>> PreviewImport(IFormFile file)
        {

            ImportResponse<ImportExportEmployee> importResponse;

            // Check file type and call respective import methods
            if (file.IsExcelFile())
                importResponse = _excelService.Import<ImportExportEmployee>(file);
            else if (file.IsCsvFile())
                importResponse = _csvService.Import<ImportExportEmployee>(file);
            else
                importResponse = new ImportResponse<ImportExportEmployee>
                {
                    Success = false,
                    ErrorMessages = new List<string> { "Invalid file format" }
                };

            // Check if importResponse is null
            if (importResponse == null)
                return new ImportResponse<ImportExportEmployee>
                {
                    Success = false,
                    ErrorMessages = new List<string> { "Unable to process the file" }
                };

            //// If the import is not successful, return the error response
            //if (!importResponse.Success)
            //    return importResponse;

            int rowNumber = 1;  // Track skipped rows

            foreach (var employeeItem in importResponse.Data.ToList())
            {
                bool hasError = false;

                if (importResponse.SkippedRows.Contains(rowNumber))
                {
                    rowNumber++;
                    importResponse.Data.Remove(employeeItem);
                    continue; // Row is already marked as skipped
                }

                // Validate employee fields
                if (string.IsNullOrWhiteSpace(employeeItem.FirstName))
                {
                    importResponse.ErrorMessages.Add($"Row {rowNumber} - FirstName is required");
                    hasError = true;
                }

                if (string.IsNullOrWhiteSpace(employeeItem.LastName))
                {
                    importResponse.ErrorMessages.Add($"Row {rowNumber} - LastName is required");
                    hasError = true;
                }

                if (string.IsNullOrWhiteSpace(employeeItem.Designation))
                {
                    importResponse.ErrorMessages.Add($"Row {rowNumber} - Designation is required");
                    hasError = true;
                }

                if (employeeItem.DOB == null || employeeItem.DOB == new DateTime())
                {
                    importResponse.ErrorMessages.Add($"Row {rowNumber} - DOB is required");
                    hasError = true;
                }

                if (string.IsNullOrWhiteSpace(employeeItem.Gender) || !Enum.TryParse<GenderEnum>(employeeItem.Gender, out var gender))
                {
                    importResponse.ErrorMessages.Add($"Row {rowNumber} - Gender is required");
                    hasError = true;
                }

                if (employeeItem.Salary == 0)
                {
                    importResponse.ErrorMessages.Add($"Row {rowNumber} - Salary is required");
                    hasError = true;
                }

                // If there are any errors, add to skipped rows
                if (hasError)
                {
                    importResponse.Data.Remove(employeeItem);
                    importResponse.SkippedRows.Add(rowNumber); // Add row number to skipped rows
                }

                rowNumber++;


            }

            // Successfully imported if no errors
            importResponse.Success = importResponse.ErrorMessages.Count == 0;

            return importResponse;
        }

        public async Task<ImportResponse<ImportExportEmployee>> SaveImport(IFormFile file)
        {
            // Call PreviewImport to validate the file and get the preview data
            var importResponse = await PreviewImport(file);

            // If preview import is not successful, return the same response
            if (importResponse.Data == null || importResponse.Data.Count == 0)
            {
                return importResponse;
            }

            // Add employee to the database
            _context.Employees.AddRange(importResponse.Data.Select(employeeItem =>
            {
                return new Employee
                {
                    FirstName = employeeItem.FirstName,
                    LastName = employeeItem.LastName,
                    Designation = employeeItem.Designation,
                    DOB = employeeItem.DOB,
                    Salary = employeeItem.Salary,
                    Gender = Enum.TryParse<GenderEnum>(employeeItem.Gender, out var gender) ? gender : GenderEnum.Other,
                };
            }));


            importResponse.Success = (await _context.SaveChangesAsync() > 0);

            return importResponse;
        }


        public async Task<FileDetail> Export(ExportRequest request)
        {
            if (!_currentUserService.IsAuthenticated)
            {
                await _eventPublisher.PublishAsync(new UnauthorizedExportEvent
                {
                    AttemptedOn = DateTime.Now,
                    Message = "User is not authorized"
                });
                throw new UnAuthorizedException("Unauthorized");
            }

            if (request == null || !request.Ids.Any())
                throw new BadRequestException("Ids is required");

            var employees = await GetEmployeesByIds(request.Ids);

            if (employees == null)
                throw new BadRequestException("Employees not found");

            switch (request.Format.ToLower())
            {
                case "excel":
                    return _excelService.Export(employees, "employee");
                case "csv":
                    return _csvService.Export(employees, "employees");
                case "pdf":
                    // TODO : Generate pdf content from list
                    return _pdfService.Export("pdf content", "employees");
                default:
                    throw new BadRequestException("Invalid export format.");
            }
        }
        public FileDetail SampleImportEmployeeFile()
        {
            var employeeList = new List<ImportExportEmployee>
            {
                new ImportExportEmployee
                {
                    Id = 1,
                    FirstName = "FirstName" ,
                    LastName ="lastName"  ,
                    Gender ="Male",
                    Designation = "Manager",
                    DOB = new DateTime(1990,1,1),
                    Salary = 100000m
                },
            };

            return _excelService.Export<ImportExportEmployee>(employeeList, "Employees", true);
        }

        #endregion

        public async Task<string> GetEmployeeDataFromExternalApi()
        {
            var uri = "http://dummy.restapiexample.com/api/v1/employees";
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("Cookie", "humans_21909=1"); // this site requires this cookie
            HttpResponseMessage response = await client.GetAsync(
                           uri);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return null;
        }

    }
}


