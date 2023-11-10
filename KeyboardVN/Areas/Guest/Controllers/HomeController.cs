using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using KeyboardVN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Text.Encodings.Web;

namespace KeyboardVN.Areas.Guest.Controllers
{
    public class HomeController : Controller
    {
        private KeyboardVNContext context = new KeyboardVNContext();
        private IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();

        [Area("Guest")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            List<Category> categories = new List<Category>();
            List<Brand> brands = new List<Brand>();
            categories = context.Categories.ToList();
            brands = context.Brands.ToList();
            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            List<Product> products = new List<Product>();
            products = context.Products.OrderBy(p => p.UnitInStock).Take(3).ToList();
            ViewBag.Products = products;
            int productInCart = 0;
            if (httpContextAccessor.HttpContext.Session.GetInt32("userId") != null)
            {
                productInCart = context.CartItems.Where(ci => ci.CartId == context.Carts.FirstOrDefault(c => c.UserId == httpContextAccessor.HttpContext.Session.GetInt32("userId")).Id).Count();
            }
            ViewBag.productInCart = productInCart;
            return View();
        }
        [Area("Guest")]
        public IActionResult Details(int id)
        {
            int productInCart = 0;
            if (httpContextAccessor.HttpContext.Session.GetInt32("userId") != null)
            {
                productInCart = context.CartItems.Where(ci => ci.CartId == context.Carts.FirstOrDefault(c => c.UserId == httpContextAccessor.HttpContext.Session.GetInt32("userId")).Id).Count();
            }
            ViewBag.productInCart = productInCart;
            var product = context.Products.Include(c=>c.Category).Include(c=>c.Brand).FirstOrDefault(product => product.Id == id);
            return View(product);
        }
        [Area("Guest")]
        public IActionResult ProductFilter(String? searchName,
                                    int? categoryId,
                                    String? price,
                                    int? brandId,
                                    String? sort,
                                    int? page)
        {
            List<Category> categories = new List<Category>();
            List<Brand> brands = new List<Brand>();

            categories = context.Categories.ToList();
            brands = context.Brands.ToList();

            if (searchName == null)
            {
                searchName = "";
            }
            if (categoryId == null)
            {
                categoryId = -1;
            }
            if (brandId == null)
            {
                brandId = -1;
            }
            if (sort == null)
            {
                sort = "";
            }
            if (page == null)
            {
                page = 1;
            }
            List<Product> listP = new List<Product>();
            string ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("KeyboardVNContextConnection");
            SqlConnection conn = new SqlConnection(ConnectionString);
            double minPrice = 0;
            double maxPrice = 10000000000000;
            if (price != null && !price.Equals(""))
            {
                string[] tokens = price.Split('-');
                minPrice = double.Parse(tokens[0]);
                maxPrice = double.Parse(tokens[1]);
            }
            string sql = "";
            if (sort == "")
            {
                sql = @"select * from (select *, ROW_NUMBER() over (order by id) as r
               from product where categoryId in ("
                           + (categoryId == -1 ? "select id from Category" : "@CategoryId") + @")
               and brandid in ("
                           + (brandId == -1 ? "select id from Brand" : "@brandId") + @")
               and price between @MinPrice and @MaxPrice
               and [product].[name] like @SearchName) as x where r between 16*@Page-15 and 16*@Page";
            }
            else if (sort == "ascending")
            {
                sql = @"select * from (select *, ROW_NUMBER() over (order by id) as r
               from product where categoryId in ("
                           + (categoryId == -1 ? "select id from Category" : "@CategoryId") + @")
               and brandid in ("
                           + (brandId == -1 ? "select id from Brand" : "@brandId") + @")
               and price between @MinPrice and @MaxPrice
               and [product].[name] like @SearchName ) as x where r between 16*@Page-15 and 16*@Page order by price-discount";
            }
            else
            {
                sql = @"select * from (select *, ROW_NUMBER() over (order by id) as r
               from product where categoryId in ("
                             + (categoryId == -1 ? "select id from Category" : "@CategoryId") + @")
               and brandid in ("
                             + (brandId == -1 ? "select id from Brand" : "@brandId") + @")
               and price between @MinPrice and @MaxPrice
               and [product].[name] like @SearchName  ) as x where r between 16*@Page-15 and 16*@Page order by price-discount desc";
            }

            try
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    command.Parameters.AddWithValue("@CategoryId", categoryId);
                    command.Parameters.AddWithValue("@brandId", brandId);
                    command.Parameters.AddWithValue("@MinPrice", minPrice);
                    command.Parameters.AddWithValue("@MaxPrice", maxPrice);
                    command.Parameters.AddWithValue("@SearchName", "%" + searchName + "%");
                    command.Parameters.AddWithValue("@Page", page);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            listP.Add(context.Products.FirstOrDefault(p => p.Id == id));
                        }
                    }
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            ViewBag.categoryId = categoryId;
            ViewBag.brandId = brandId;
            ViewBag.price = price;
            ViewBag.sort = sort;
            ViewBag.searchName = searchName;
            ViewBag.page = page;
            ViewBag.Categories = categories;
            ViewBag.Brands = brands;
            ViewBag.Products = listP;
            int productInCart = 0;
            if (httpContextAccessor.HttpContext.Session.GetInt32("userId") != null)
            {
                int? userId = httpContextAccessor.HttpContext.Session.GetInt32("userId");
                ViewBag.UserId = userId;
                productInCart = context.CartItems.Where(ci => ci.CartId == context.Carts.FirstOrDefault(c => c.UserId == httpContextAccessor.HttpContext.Session.GetInt32("userId")).Id).Count();
            }
            ViewBag.productInCart = productInCart;
            ViewBag.numberOfProduct = context.Products.Count();
            return View();
        }
        [Area("Guest")]
        public ActionResult History()
        {
            int? sessionId = HttpContext.Session.GetInt32("userId");
            List<Order> orderList = (from order in context.Orders where 
                                     sessionId == order.UserId select order).ToList();
            return View(orderList);
        }

        [Area("Guest")]
        public ActionResult OrderDetail(int id)
        {
            Order userOrder = (from Order in context.Orders where id == Order.Id select Order).SingleOrDefault();
            ViewBag.userOrder = userOrder;
            List<OrderDetail> detail = context.OrderDetails
            .Where(od => od.OrderId == id)
            .Include(od => od.Product)
            .ToList();
            foreach (var item in detail)
            {
                Console.WriteLine("Qty" + item.Quantity + " " + item.Product.Name);
            }
            return View(detail);
        }
        [Area("Guest")]
        public ActionResult EditStatus(int id, string status)
        {
            var order = context.Orders.SingleOrDefault(item => item.Id == id);
            if (status == "Cancel")
            {
                order.Status = "Cancelled";
                context.SaveChanges();
            }
            else if (status == "Received")
            {
                order.Status = "Received";
                context.SaveChanges();
                createInvoieImage(order);
            }
            return RedirectToAction("History", "Home");
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
                .Add(new Paragraph("subtotal: " + Math.Round(sub,2) + "$"));

            Cell shipping = new Cell(1, 1)
                .SetFontSize(16)
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetPaddingRight(20f)
                .Add(new Paragraph("Shipping: " + Math.Round( sub * 0.1,2) + "$"));

            Cell total = new Cell(1, 1)
                .SetFontSize(32)
                .SetBorder(Border.NO_BORDER)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetPaddingRight(20f)
                .Add(new Paragraph("Total: " + Math.Round( sub * 1.1,2) + "$"));

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
