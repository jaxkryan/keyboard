using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KeyboardVN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace KeyboardVN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManageUserController : Controller
    {
        private readonly KeyboardVNContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public ManageUserController(KeyboardVNContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: ManageUserController
        public ActionResult Index()
        {
            var users = _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToList();

            ViewData["Title"] = "Manage User";
            return View(users);
        }

        // GET: ManageUserController/Details/5
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
        public ActionResult Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = _context.Users
                ?.Include(u => u.UserRoles)
                !.ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Edit User";
            return View(user);
        }

        // POST: ManageUserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int? id, [Bind("Id, FirstName, LastName, Street, City, Province, Country, UserName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed")] User user, string userRole)
        {
            if(id != user.Id)
            {
                Console.WriteLine($"not found, id={id}, user.id={user.Id}");
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                User userInDB = await _userManager.FindByIdAsync(id.ToString());
                userInDB.FirstName = user.FirstName;
                userInDB.LastName = user.LastName;
                userInDB.Street = user.Street;
                userInDB.City = user.City;
                userInDB.Province = user.Province;
                userInDB.Country = user.Country;
                userInDB.UserName = user.UserName;
                userInDB.Email = user.Email;
                userInDB.EmailConfirmed = user.EmailConfirmed;
                userInDB.PhoneNumber = user.PhoneNumber;
                userInDB.PhoneNumberConfirmed = user.PhoneNumberConfirmed;

                var updateUserInformationResult = await _userManager.UpdateAsync(userInDB);
                var oldUserRoles = await _userManager.GetRolesAsync(userInDB);
                var removeRoleResult = await _userManager.RemoveFromRolesAsync(userInDB, oldUserRoles);
                var addRoleResult = await _userManager.AddToRoleAsync(userInDB, userRole);

                if(updateUserInformationResult.Succeeded && removeRoleResult.Succeeded && addRoleResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in updateUserInformationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                foreach (var error in removeRoleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                foreach (var error in addRoleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(user);
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
