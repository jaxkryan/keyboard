using Microsoft.AspNetCore.Mvc;

namespace KeyboardVN.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "ManageUser");
        }
    }
}
