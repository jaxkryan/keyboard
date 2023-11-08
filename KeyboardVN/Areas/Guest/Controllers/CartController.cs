using KeyboardVN.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace KeyboardVN.Areas.Guest.Controllers
{
    public class CartController : Controller
    {
        private KeyboardVNContext context = new KeyboardVNContext();
        private IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
        public IActionResult Index()
        {
            return View();
        }
        [Area("Guest")]
        public IActionResult addToCart(int productId,
                                    string from,
                                    String? searchName,
                                    int? categoryId,
                                    String? price,
                                    int? brandId,
                                    String? sort,
                                    int? page,
                                    int? quantity)
        {
            int? userId = httpContextAccessor.HttpContext.Session.GetInt32("userId");
            if (productId == null)
            {
                productId = 1;
            }
            Console.WriteLine(userId);
            if (quantity == null)
            {
                quantity = 1;
            }
            if (userId == null)
            {
                TempData["notification"] = "Please login!";
                TempData["notiType"] = "RED";
                return RedirectToAction("Index", "Home");
            }
            Cart cart = context.Carts.FirstOrDefault(c => c.UserId == userId);
            Console.WriteLine(cart.Id);
            if (cart == null)
            {
                TempData["notification"] = "Cart Error!";
                TempData["notiType"] = "RED";
                return RedirectToAction("Index", "Home");
            }
            if (quantity + context.CartItems.FirstOrDefault(x => x.CartId == cart.Id && x.ProductId == productId).Quantity > context.Products.FirstOrDefault(p => p.Id == productId).UnitInStock)
            {
                TempData["notification"] = "Not Enough in stock!";
                TempData["notiType"] = "RED";
            }
            else
            {
                CartItem item = context.CartItems.FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId);
                if (item == null)
                {
                    CartItem temp = new CartItem();
                    temp.CartId = cart.Id;
                    temp.ProductId = productId;
                    temp.Quantity = (int)quantity;
                    temp.Product = context.Products.FirstOrDefault(p => p.Id == productId);
                    temp.Cart = cart;
                    context.CartItems.Add(temp);
                    context.SaveChanges();
                }
                else
                {
                    context.CartItems.FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId).Quantity += (int)quantity;
                    context.SaveChanges();
                }
                TempData["notification"] = "Added " + quantity + " " + context.Products.FirstOrDefault(p => p.Id == productId).Name + " to Cart!";
                TempData["notiType"] = "GREEN";
            }
            if (from == "home")
            {
                return RedirectToAction("Index", "Home");
            }
            else if (from == "shop")
            {
                var routeValues = new RouteValueDictionary
                {
                    { "categoryId", categoryId },
                    { "brandId", brandId },
                    { "price", price},
                    { "searchName" , searchName},
                    { "sort", sort},
                    { "page", page}
                };
                return RedirectToAction("ProductFilter", "Home", routeValues);
            }
            else if (from == "detail")
            {
                var values = new RouteValueDictionary
                {
                    { "id", productId }
                };
                return RedirectToAction("Details", "Home", values);
            }
            else
            {
                return View();
            }
        }
    }
}
