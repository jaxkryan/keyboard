using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using KeyboardVN.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using KeyboardVN.Util.EmailSender;
using static System.Net.WebRequestMethods;

namespace KeyboardVN.Areas.Seller.Controllers
{
    [Area("Seller")]
    [Authorize(Roles = "Seller")]
    public class ManageOrderController : Controller
    {

        private readonly KeyboardVNContext _keyboardVN;
        private readonly ISendMailService _sendMailService;
        

        public ManageOrderController(KeyboardVNContext keyboardVN, ISendMailService sendMailService)
        {
            _keyboardVN = keyboardVN;
            _sendMailService = sendMailService;
        }

        // GET: ManageOrderController/SendMail
        // Demo using gmail
        public async Task<IActionResult> SendMail()
        {
            MailContent content = new MailContent
            {
                To = "hungnthe176686@fpt.edu.vn", // Mail received address
                Subject = "Thu gui a Nam", // Subject
                Body = "<p><strong>Hello sent from controller</strong></p> " +
                        "<a href='https://down-vn.img.susercontent.com/file/vn-11134207-7qukw-lh52ju4vvcj6cc'> giff </a>",
                ImagePath = @"wwwroot/armored-core-vi-fires-of-rubicon-reveal-trailer-e.webp"

            };

            await _sendMailService.SendMail(content);// Send mail
            return View();

        }
        // GET: ManageOrderController
        public ActionResult Index()
        {
            var orderList = _keyboardVN.Orders.ToList();
            return View(orderList);
        }

        // GET: ManageOrderController/Details/5
        public async Task<IActionResult> ViewDetails(int? id)
        {
            if (id == null || _keyboardVN.OrderDetails == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var orderDetail = await _keyboardVN.Orders.Include(x => x.OrderDetails).ThenInclude(d => d.Product).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (orderDetail == null)
            {
                return RedirectToAction(nameof(Index));

            }
            return View(orderDetail);
        }

        // GET: ManageOrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageOrderController/Create
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

        // GET: ManageOrderController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _keyboardVN.Orders == null)
            {
                return View("Index");
                Console.WriteLine("null id");
            }
            var editOrder = _keyboardVN.Orders.Include(o => o.User).FirstOrDefault(a => a.Id == id);
            if (editOrder == null)
            {
                return View("Index");
                Console.WriteLine("null order");

            }
            return View(editOrder);
        }

        // POST: ManageOrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrder(int id, [Bind("Id,UserId,Receiver,ShipStreet,ShipCity,ShipProvince,ShipCountry,ShipEmail,ShipPhone,Status,CreatedTime")] Order order)
        {
            Console.WriteLine("run update");

            if (id != order.Id)
            {
                //return View("Index");
                Console.WriteLine("not found");
                

            }

            if (ModelState.IsValid)
            {
                try
                {
                    Order oldOrder = _keyboardVN.Orders.Where(o => o.Id == order.Id).FirstOrDefault();                   
                    Console.WriteLine("Update success");
                    if (oldOrder.Status != order.Status)
                    {
                        MailContent content = new MailContent
                        {
                            To = order.ShipEmail, // Mail received address
                            Subject = "Order Infomation", // Subject
                            Body = $"Your Order Status is {order.Status} now" // content
                        };
                        _sendMailService.SendMailWithoutImage(content);
                    }
                    _keyboardVN.Update(order);
                    await _keyboardVN.SaveChangesAsync();
                    
                }
                catch
                {
                    if (!OrderExists(order.Id))
                    {
                        Console.WriteLine("not found");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Console.WriteLine("not update yet");
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("not valid model");
                return RedirectToAction(nameof(Index));

            }
            return RedirectToAction(nameof(Index));

        }

        // GET: ManageOrderController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _keyboardVN.Orders == null)
            {
                return RedirectToAction(nameof(Index));
            }
            if (_keyboardVN.Orders == null)
            {
                Console.WriteLine("its null");
                return RedirectToAction(nameof(Index));
            }
            var deleteOrder = _keyboardVN.Orders.FirstOrDefault(c => c.Id == id);
            if (deleteOrder != null)
            {
                Console.WriteLine("remove done");
                _keyboardVN.Orders.Remove(deleteOrder);
            }
            _keyboardVN.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // POST: ManageOrderController/Delete/5
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
        private bool OrderExists(int Id)
        {
            return (_keyboardVN.Orders?.Any(o => o.Id == Id)).GetValueOrDefault();
        }
        //public static async Task<String> SendMail(string _form, string _to, string _subject, string _body)
        //{
        //    MailMessage message = new MailMessage(_form, _to, _subject, _body);
        //    message.BodyEncoding = System.Text.Encoding.UTF8;
        //    message.SubjectEncoding = System.Text.Encoding.UTF8;
        //    message.IsBodyHtml = true; // accept content is HTML

        //    message.Sender = new MailAddress(_form);
        //    using var smtpClient = new SmtpClient("localhost");//name of host
        //    try
        //    {
        //        await smtpClient.SendMailAsync(message);
        //        return "Send email successfully!";
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return "Send email fail!";
        //    }
        //}
        //public static async Task<String> SendGmail(string _form, string _to, string _subject, string _body, string _gmail, string _password)
        //{
        //    MailMessage message = new MailMessage(_form, _to, _subject, _body);
        //    message.BodyEncoding = System.Text.Encoding.UTF8;
        //    message.SubjectEncoding = System.Text.Encoding.UTF8;
        //    message.IsBodyHtml = true; // accept content is HTML

        //    message.Sender = new MailAddress(_form);
        //    using var smtpClient = new SmtpClient("smtp.gmail.com");
        //    smtpClient.Port = 587;
        //    smtpClient.EnableSsl = true;
        //    smtpClient.Credentials = new NetworkCredential(_gmail, _password);
        //    try
        //    {
        //        await smtpClient.SendMailAsync(message);
        //        return "Send email successfully!";
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return "Send email fail!";
        //    }
        //}
    }
}
