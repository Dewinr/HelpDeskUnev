using Microsoft.AspNetCore.Mvc;

namespace HelpDeskUnev
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
