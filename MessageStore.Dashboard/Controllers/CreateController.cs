using Microsoft.AspNetCore.Mvc;

namespace MessageStore.Dashboard.Controllers
{
    public class CreateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}