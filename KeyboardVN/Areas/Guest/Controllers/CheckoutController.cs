using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using KeyboardVN.Models;
using KeyboardVN.Util.EmailSender;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace KeyboardVN.Areas.Guest.Controllers
{
    public class CheckoutController : Controller
    {
        private KeyboardVNContext context;
        private IHttpContextAccessor httpContextAccessor;
        private readonly ISendMailService _sendMailService;

        public CheckoutController(KeyboardVNContext context, IHttpContextAccessor httpContextAccessor, ISendMailService sendMailService)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
            _sendMailService = sendMailService;
        }

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
            createInvoieImage(order);

            string imgpath = @"wwwroot\invoice_order_" + order.Id + ".jpg";

            MailContent content = new MailContent
            {
                To = context.Users.FirstOrDefault(u => u.Id == order.UserId).Email, // Mail received address
                Subject = "Invoice for order " + order.Id, // Subject
                Body = "<p><strong style='font-size:24px'>Invoice for your order</strong></p> " +
                "<div> Thank you for using our service, here are invoice for your placed order, please check it carefully.</div>"
                + "<div> If anything work wrong please contact: 0868390784 </div>",
                ImagePath = imgpath
            };

            _sendMailService.SendMail(content);// Send mail
            
            TempData["notification"] = "Place order successfully!";
            TempData["notiType"] = "GREEN";
            return RedirectToAction("Index", "Home");
        }
        private void createInvoieImage(Order order)
        {
            String path = @"wwwroot\invoice_order_" + order.Id + ".pdf";
            String imagepath = @"wwwroot\logo.jpg";
            PdfWriter writer = new PdfWriter(path);
            iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);
            iText.Layout.Document doc = new iText.Layout.Document(pdf);

            iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(imagepath));
            logo.SetHeight(60);
            logo.SetWidth(60);
            logo.SetFixedPosition(40, 740);

            float col = 300f;
            float[] colwidth = { col, col };

            Table table = new Table(colwidth);


            Cell cell11 = new Cell(1, 1)
                .SetFontSize(28)
                .SetBold()
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.LEFT)
                .Add(logo);

            Cell cell12 = new Cell(1, 1)
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFontSize(52)
                .Add(new Paragraph("Invoice"));

            Table receiverInfoTable = new Table(colwidth).SetMarginTop(20f);

            Cell cell31 = new Cell(1, 1)
                .SetFontSize(16)
                .SetBorder(Border.NO_BORDER)
                .Add(new Paragraph("Receiver:\n" +
                order.Receiver));

            Cell cell32 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(16)
                .Add(new Paragraph("Invoice No." + order.Id));

            Cell cell41 = new Cell(1, 1)
                .SetFontSize(16)
                .SetBorder(Border.NO_BORDER)
                .Add(new Paragraph("Phone:\n" +
                order.ShipPhone));

            Cell cell42 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(16)
                .Add(new Paragraph("Date: " + order.CreatedTime));

            Cell adress = new Cell(1, 1)
                .SetFontSize(16)
                .SetBorder(Border.NO_BORDER)
                .Add(new Paragraph("Addess:\n" +
                order.ShipStreet + ", " + order.ShipCity + ", " + order.ShipProvince + ", " + order.ShipCountry));

            float[] itemWidth = { 120f, 120f, 120f, 120f, 120f };

            Table itemTable = new Table(itemWidth).SetMarginTop(20f);

            Cell cell51 = new Cell(1, 2).
                SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("Product"));

            Cell cell52 = new Cell(1, 1).
                SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("Unit Price"));

            Cell cell53 = new Cell(1, 1).
                SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("Quantity"));

            Cell cell54 = new Cell(1, 1).
                SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph("Amount"));

            itemTable.AddCell(cell51);
            itemTable.AddCell(cell52);
            itemTable.AddCell(cell53);
            itemTable.AddCell(cell54);

            List<OrderDetail> orderDetailList = context.OrderDetails.Include(od => od.Product).Where(od => od.OrderId == order.Id).ToList();
            Console.WriteLine(order.Id);
            double sub = 0;

            foreach (OrderDetail orderDetail in orderDetailList)
            {

                Cell item1 = new Cell(1, 2)
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .Add(new Paragraph(orderDetail.Product.Name));

                Cell item2 = new Cell(1, 1)
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph("" + orderDetail.Price + "$"));

                Cell item3 = new Cell(1, 1)
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph("" + orderDetail.Quantity));

                Cell item4 = new Cell(1, 1)
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph("" + orderDetail.Price * orderDetail.Quantity + "$"));
                sub += orderDetail.Price * orderDetail.Quantity;

                itemTable.AddCell(item1);
                itemTable.AddCell(item2);
                itemTable.AddCell(item3);
                itemTable.AddCell(item4);
            }

            float[] totalWidth = { 600f };
            Table totalTable = new Table(totalWidth).SetMarginTop(20f);

            Cell subtotal = new Cell(1, 1)
                .SetFontSize(24)
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetPaddingRight(20f)
                .Add(new Paragraph("Subtotal: " + Math.Round(sub, 2) + "$"));

            Cell shipping = new Cell(1, 1)
                .SetFontSize(16)
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetPaddingRight(20f)
                .Add(new Paragraph("Shipping: " + Math.Round(sub * 0.1, 2) + "$"));

            Cell total = new Cell(1, 1)
                .SetFontSize(32)
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetPaddingRight(20f)
                .Add(new Paragraph("Grand Total: " + Math.Round(sub * 1.1, 2) + "$"));

            totalTable.AddCell(subtotal);
            totalTable.AddCell(shipping);
            totalTable.AddCell(total);

            table.AddCell(cell11);
            table.AddCell(cell12);

            receiverInfoTable.AddCell(cell31);
            receiverInfoTable.AddCell(cell32);
            receiverInfoTable.AddCell(cell41);
            receiverInfoTable.AddCell(cell42);
            receiverInfoTable.AddCell(adress);


            doc.Add(table);
            doc.Add(receiverInfoTable);
            doc.Add(itemTable);
            doc.Add(totalTable);

            doc.Close();
            string imgpath = @"wwwroot\invoice_order_" + order.Id + ".jpg";
            using (PdfiumViewer.PdfDocument document = PdfiumViewer.PdfDocument.Load(path))
            {
                for (int i = 0; i < document.PageCount; i++)
                {
                    using (Bitmap img = (Bitmap)document.Render(i, 300, 300, true))
                    {
                        img.Save(imgpath);
                    }
                }
            }
        }

    }
}
