using Microsoft.AspNetCore.Mvc;

namespace EduCore.Controllers
{
    public class AcademicController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}