using KeyboardVN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyboardVN.Areas.Guest.Controllers
{
    public class HomeController : Controller
    {
        private KeyboardVNContext context = new KeyboardVNContext();
        private IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
        [Area("Guest")]
        public IActionResult Index()
        {
            List<Category> categories = new List<Category>();
            List<Brand> brands = new List<Brand>();
            List<Product> products = new List<Product>();
            categories = context.Categories.ToList();
            brands = context.Brands.ToList();
            products = context.Products.OrderBy(p => p.UnitInStock).Take(3).ToList();
            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            ViewBag.Products = products;
            int productInCart = 0;
            if (httpContextAccessor.HttpContext.Session.GetInt32("userId") != null)
            {
                productInCart = context.CartItems.Where(ci => ci.CartId == context.Carts.FirstOrDefault(c => c.UserId == httpContextAccessor.HttpContext.Session.GetInt32("userId")).Id).Count();
            }
            ViewBag.productInCart = productInCart;
            return View();
        }
    }
}
