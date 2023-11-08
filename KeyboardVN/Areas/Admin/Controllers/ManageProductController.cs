using KeyboardVN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            List<Category> category = _context.Categories.ToList();
            ViewBag.Category = category;
            List<Brand> brand = _context.Brands.ToList();
            ViewBag.Brand = brand;
            return View();
        }

        // POST: ManageProductController/Create
        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("CategoryId,BrandId,Name,Image,Description,Price,Discount,UnitInStock")] Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(product);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save change");
            }
            return View(product);
        }

        // GET: ManageProductController/Edit/5
        [Area("Admin")]
        public ActionResult Edit(int id)
        {
            List<Category> category = _context.Categories.ToList();
            ViewBag.Category = category;
            List<Brand> brand = _context.Brands.ToList();
            ViewBag.Brand = brand;
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            return View(product);
        }

        // POST: ManageProductController/Edit/5
        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Id,CategoryId,BrandId,Name,Image,Description,Price,Discount,UnitInStock")] Product product)
        {
            //Console.WriteLine("run update");
            //Console.WriteLine(product.BrandId); Console.WriteLine(product.CategoryId);

            if (id != product.Id)
            {
                Console.WriteLine("ded");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    //Console.WriteLine("done update");
                    _context.Update(product);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        [Area("Admin")]
        // GET: ManageProductController/Delete/5
        public ActionResult Delete(int? id)
        {
            Console.WriteLine("get delete");
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            if (_context.Products == null)
            {
                Console.WriteLine("its null");
                return Problem("ded");
            }
            var product = _context.Products.Find(id);
            if (product != null)
            {
                Console.WriteLine("remove done");
                _context.Products.Remove(product);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // POST: ManageProductController/Delete/5
        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(int id)
        {
            return View(id);
        }
        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
