using EmployeePortal.Application.Services.Employees.Models.Commands;
using EmployeePortal.Application.Services.Employees.Models.Filters;
using EmployeePortal.Application.Services.Employees;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EmployeePortal.Application.Services.Employees.Models.Dtos;
using EmployeePortal.UI.Models;
using EmployeePortal.Application;
using Microsoft.AspNetCore.Authorization;

namespace EmployeePortal.UI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: Employee/Index
        public async Task<IActionResult> Index(EmployeeFilter filter)
        {
            if (filter.PageSize == null)
            {
                filter.PageSize = 5;
            };


            var employees = await _employeeService.GetEmployees(filter);


            var viewModel = new EmployeeViewModel
            {
                Filter = filter,
                Data = employees,

            };
            return View(viewModel);
        }

        // GET: Employee/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View(new InsertUpdateEmployee());
        }

        // POST: Employee/Create
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(InsertUpdateEmployee request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _employeeService.InsertUpdateEmployee(request);
            if (result > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Failed to create employee");
            return View(request);
        }

        // GET: Employee/Edit/{id}
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }



            return View(employee);
        }

        // POST: Employee/Edit/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(InsertUpdateEmployee model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Update the employee
            var result = await _employeeService.InsertUpdateEmployee(model);
            if (result > 0)
            {
                TempData["SuccessMessage"] = "Employee updated successfully!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Failed to update employee.";
            return View(model);
        }


        // POST: Employee/UploadProfileImage
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadProfileImage(InsertEmployeeProfileImage profileImage)
        {
            if (profileImage == null || profileImage.Image == null || !profileImage.Image.IsImage())
            {
                return BadRequest("Invalid file upload");
            }

            var isSuccess = await _employeeService.InsertEmployeeProfileImage(profileImage);
            return Json(new { success = isSuccess });
        }


        [HttpPost]
        public async Task<IActionResult> PreviewImport(IFormFile file)
        {
            if (file == null || !(file.IsCsvFile() || file.IsExcelFile()))
            {
                return Json(new { success = false, message = "Invalid file type. Only .csv and .xlsx files are allowed." });
            }

            var result = await _employeeService.PreviewImport(file); // Process file and return data with validation
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveImport(IFormFile file)
        {

            if (file == null || !(file.IsCsvFile() || file.IsExcelFile()))
            {
                return Json(new { success = false, message = "Invalid file type. Only .csv and .xlsx files are allowed." });
            }

            var result = await _employeeService.SaveImport(file);
            return Json(result);
        }


        // GET: Employee/SampleFile
        public IActionResult SampleFile()
        {
            var sampleFile = _employeeService.SampleImportEmployeeFile();
            return File(sampleFile.FileContents, sampleFile.ContentType, sampleFile.FileName);
        }


        [HttpPost]
        public async Task<IActionResult> Export([FromBody] ExportRequest request)
        {
            if (request == null || !request.Ids.Any())
            {
                return BadRequest("No records selected for export.");
            }
            var file = await _employeeService.Export(request);

            if(file == null)
                return BadRequest("Unable to export the file.");

            return File(file.FileContents, file.ContentType, file.FileName);


        }



        // GET: Employee/ExternalEmployee
        public async Task<IActionResult> ExternalEmployee()
        {
            return View();
        }

        public async Task<string> GetEmployeeData()
        {
            return await _employeeService.GetEmployeeDataFromExternalApi();

        }
    }
}
