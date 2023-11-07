using KeyboardVN.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KeyboardVN.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<User> _userManager;

        public HomeController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [Area("Admin")]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "ManageUser");
        }
    }
}
