using KeyboardVN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KeyboardVN.Areas.Admin.Controllers
{
    public class ManageProductController : Controller
    {
        private readonly KeyboardVNContext _context;

        public ManageProductController(KeyboardVNContext context)
        {
            _context = context;
        }
        // GET: ManageProductController
        [Area("Admin")]
        public ActionResult Index()
        {
            var products = _context.Products.Include(c => c.Category).Include(b => b.Brand).ToList();
            ViewData["Title"] = "Manage Product";
            return View(products);
        }

        // GET: ManageProductController/Details/5
        [Area("Admin")]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManageProductController/Create
        [Area("Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageProductController/Create
        [Area("Admin")]
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

        // GET: ManageProductController/Edit/5
        [Area("Admin")]
        public ActionResult Edit(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            return View(product);
        }

        // POST: ManageProductController/Edit/5
        [Area("Admin")]
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
        [Area("Admin")]
        // GET: ManageProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ManageProductController/Delete/5
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
