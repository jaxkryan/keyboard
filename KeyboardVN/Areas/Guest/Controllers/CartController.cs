using KeyboardVN.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
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
                                    int? quantityToBuy)
        {
            int? userId = httpContextAccessor.HttpContext.Session.GetInt32("userId");
            Console.WriteLine(userId);
            if (quantityToBuy == null)
            {
                quantityToBuy = 1;
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
            CartItem cartItem = context.CartItems.FirstOrDefault(x => x.CartId == cart.Id && x.ProductId == productId);
            if (quantityToBuy + (cartItem?.Quantity ?? 0) > context.Products.FirstOrDefault(p => p.Id == productId).UnitInStock)
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
                    temp.Quantity = (int)quantityToBuy;
                    temp.Product = context.Products.FirstOrDefault(p => p.Id == productId);
                    temp.Cart = cart;
                    context.CartItems.Add(temp);
                    context.SaveChanges();
                }
                else
                {
                    context.CartItems.FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId).Quantity += (int)quantityToBuy;
                    context.SaveChanges();
                }
                TempData["notification"] = "Added " + quantityToBuy + " " + context.Products.FirstOrDefault(p => p.Id == productId).Name + " to Cart!";
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
        [Area("Guest")]
        public IActionResult CartPage()
        {
            List<Category> categories = new List<Category>();
            List<Brand> brands = new List<Brand>();
            categories = context.Categories.ToList();
            brands = context.Brands.ToList();
            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            List<CartItem> cartItems = context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.CartId == context.Carts.FirstOrDefault(c => c.UserId == httpContextAccessor.HttpContext.Session.GetInt32("userId")).Id)
                .ToList(); 
            ViewBag.cartItems = cartItems;
            int productInCart = 0;
            if (httpContextAccessor.HttpContext.Session.GetInt32("userId") != null)
            {
                productInCart = context.CartItems.Where(ci => ci.CartId == context.Carts.FirstOrDefault(c => c.UserId == httpContextAccessor.HttpContext.Session.GetInt32("userId")).Id).Count();
            }
            ViewBag.productInCart = productInCart;
            return View();
        }
        [Area("Guest")]
        [HttpPost]
        public IActionResult UpdateCartQuantity()
        {
            foreach (string key in Request.Form.Keys)
            {
                if (key.StartsWith("quantity-"))
                {
                    int productId = Convert.ToInt32(key.Replace("quantity-", ""));
                    int newQuantity = Convert.ToInt32(Request.Form[key]);
                    if( newQuantity > context.Products.FirstOrDefault(p => p.Id == productId).UnitInStock)
                    {
                        TempData["notification"] = "Not enough " + context.Products.FirstOrDefault(p => p.Id == productId).Name +" in stock!";
                        TempData["notiType"] = "RED";
                        return RedirectToAction("CartPage", "Cart");
                    }
                    context.CartItems.FirstOrDefault(c => c.ProductId == productId && c.CartId == context.Carts.FirstOrDefault(c => c.UserId == httpContextAccessor.HttpContext.Session.GetInt32("userId")).Id).Quantity = newQuantity;
                    context.SaveChanges();
                }
            }
            TempData["notification"] = "Saved change!";
            TempData["notiType"] = "GREEN";
            return RedirectToAction("CartPage", "Cart");
        }
        [Area("Guest")]
        public IActionResult DeleteCartItem(int productId)
        {
            context.CartItems.Remove(context.CartItems.FirstOrDefault(c => c.ProductId == productId && c.CartId == context.Carts.FirstOrDefault(c => c.UserId == httpContextAccessor.HttpContext.Session.GetInt32("userId")).Id));
            context.SaveChanges();
            return RedirectToAction("CartPage", "Cart");
        }
    }
}
