using EmployeePortal.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePortal.UI.Controllers
{
    public class ErrorController : Controller
    {
        // Error action that will handle general errors
        public IActionResult Error(int? statusCode)
        {
            var errorDetails = new ErrorViewModel
            {
                StatusCode = statusCode ?? 500,
                Message = "An unexpected error occurred.",
                Description = "Please try again later or contact support."
            };

            // You can customize error messages based on the statusCode
            if (statusCode == 404)
            {
                errorDetails.Message = "The page you're looking for was not found.";
            }
            else if (statusCode == 500)
            {
                errorDetails.Message = "We're sorry, but something went wrong on our end.";
            }

            return View("Error", errorDetails);  // Return the common error page
        }

        public IActionResult NotFound()
        {
            var errorDetails = new ErrorViewModel
            {
                StatusCode = 404,
                Message = "Page not found.",
                Description = "The page you are looking for does not exist."
            };

            return View( errorDetails);
        }

        public IActionResult Unauthorized()
        {
            var errorDetails = new ErrorViewModel
            {
                StatusCode = 401,
                Message = "Unauthorized Access.",
                Description = "You do not have permission to access this page."
            };

            return View( errorDetails);
        }

        public IActionResult BadRequest()
        {
            var errorDetails = new ErrorViewModel
            {
                StatusCode = 401,
                Message = "Bad request.",
                Description = "Invalid data."
            };

            return View( errorDetails);
        }

    }

}
