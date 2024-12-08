using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolApp.DTO;
using SchoolApp.Models;
using SchoolApp.Services;

namespace SchoolApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly IApplicationService _applicationService;
        public List<Error> ErrorArray { get; set; } = new();

        public TeacherController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(TeacherSignupDTO teacherSignupDTO)
        {
            //if (!ModelState.IsValid)
            //{
            //    foreach (var entry in ModelState.Values)
            //    {
            //        foreach (var error in entry.Errors)
            //        {
            //            ErrorArray.Add(new Error("", error.ErrorMessage, ""));
            //            ViewData["ErrorArray"] = ErrorArray;
            //        }
            //    }
            //    return View(teacherSignupDTO);
            //}
            if (!ModelState.IsValid)
            {
                return View(teacherSignupDTO);
            }

            try
            {
                await _applicationService.TeacherService.SignUpUserAsync(teacherSignupDTO);
                return RedirectToAction("Login", "User");
            }
            catch (Exception ex)
            {
                ErrorArray.Add(new Error("", ex.Message, ""));
                ViewData["ErrorArray"] = ErrorArray;
                return View();
            }
        }
    }
}
