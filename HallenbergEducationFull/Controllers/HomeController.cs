using HallenbergEducationFull.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace HallenbergEducationFull.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string baseUrl = "https://localhost:44302/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Courses()
        {
            var courseInfo = new List<CourseViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var res = await client.GetAsync("api/Course");

                if (res.IsSuccessStatusCode)
                {
                    var courseResponse = res.Content.ReadAsStringAsync().Result;

                    courseInfo = JsonConvert.DeserializeObject<List<CourseViewModel>>(courseResponse);
                }
            }

            var sortedCourseInfo = courseInfo.OrderBy(o => o.CourseName).ToList();

            return View(sortedCourseInfo);
            
        }

        public async Task<IActionResult> AddCourses()
        {
            ViewBag.TypeDropDown = await LoadTeachersAsync();

            return View();                    
        }

        [HttpPost]
        public async Task<IActionResult> AddCourses(AddCourseViewModel obj)
        {
            ViewBag.TypeDropDown = await LoadTeachersAsync();

            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(obj);

                var data = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient()) { client.BaseAddress = new Uri(baseUrl); HttpResponseMessage response = await client.PostAsync("api/Course", data); }

                return RedirectToAction("Courses");
            }
            else
            {
                return View();
            }      
        }

        public async Task<List<SelectListItem>> LoadTeachersAsync()
        {
            var teachers = new List<AddCourseTeacherViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var res = await client.GetAsync("api/Teacher");

                if (res.IsSuccessStatusCode)
                {
                    var teacherResponse = res.Content.ReadAsStringAsync().Result;

                    teachers = JsonConvert.DeserializeObject<List<AddCourseTeacherViewModel>>(teacherResponse);
                }
            }

            var typDropDown = teachers.ConvertAll(a => new SelectListItem()
            {
                Text = a.Alias.ToString(),
                Value = a.Id.ToString(),
                Selected = false
            });

            return typDropDown;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}