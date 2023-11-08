using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KeyboardVN.Models;

namespace KeyboardVN.Areas.Admin.Controllers
{
    public class ManageUserController : Controller
    {
        private readonly KeyboardVNContext _context;

        public ManageUserController(KeyboardVNContext context)
        {
            _context = context;
        }

        // GET: ManageUserController
        [Area("Admin")]
        public ActionResult Index()
        {
            var users = _context.Users;


            ViewData["Title"] = "Manage User";
            return View(users);
        }

        // GET: ManageUserController/Details/5
        [Area("Admin")]
        public ActionResult Details(int? id)
        {
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity", userId = id });
        }

        // GET: ManageUserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageUserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManageUserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ManageUserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ManageUserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManageUserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
