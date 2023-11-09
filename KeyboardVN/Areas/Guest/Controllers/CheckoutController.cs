using KeyboardVN.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KeyboardVN.Areas.Guest.Controllers
{
    public class CheckoutController : Controller
    {
        private KeyboardVNContext context = new KeyboardVNContext();
        private IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
        [Area("Guest")]
        public IActionResult CheckoutPage()
        {
            User user = context.Users.FirstOrDefault(u => u.Id == httpContextAccessor.HttpContext.Session.GetInt32("userId"));
            ViewBag.user = user;
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
        public IActionResult CreateOrder()
        {
            Order order = new Order();
            order.UserId = (int) httpContextAccessor.HttpContext.Session.GetInt32("userId");
            order.Receiver = Request.Form["firstName"] + " " + Request.Form["lastName"];
            order.Status = "Processing";
            order.ShipEmail = Request.Form["email"];
            order.ShipStreet = Request.Form["street"];
            order.ShipProvince = Request.Form["province"];
            order.ShipCity = Request.Form["city"];
            order.ShipCountry = Request.Form["country"];
            order.ShipPhone = Request.Form["phone"];
            order.CreatedTime = DateTime.Now;
            context.Orders.Add(order);
            context.SaveChanges();
            List<CartItem> cartItems = context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.CartId == context.Carts.FirstOrDefault(c => c.UserId == httpContextAccessor.HttpContext.Session.GetInt32("userId")).Id)
            .ToList();
            foreach (CartItem item in cartItems)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.OrderId = order.Id;
                orderDetail.ProductId = item.ProductId;
                orderDetail.Price = (double) (item.Product.Price - item.Product.Discount);
                orderDetail.Quantity =item.Quantity;
                context.OrderDetails.Add(orderDetail);
            }
            foreach (CartItem item in cartItems)
            {
                context.CartItems.Remove(item);
            }
            context.SaveChanges();
            TempData["notification"] = "Place order successfully!";
            TempData["notiType"] = "GREEN";
            return RedirectToAction("Index", "Home");
        }
    }
}
