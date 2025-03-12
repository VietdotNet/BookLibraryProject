using Microsoft.AspNetCore.Mvc;

namespace BookLibraryProject.Controllers
{
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
