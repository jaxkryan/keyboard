using KeyboardVN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KeyboardVN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ManageRoleController : Controller
    {
        private readonly KeyboardVNContext _context;
        private readonly RoleManager<Role> _roleManager;

        public ManageRoleController(KeyboardVNContext context, RoleManager<Role> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        // GET: ManageRoleController
        
        public ActionResult Index()
        {
            var roles = _context.Roles;

            ViewData["Title"] = "Manage Role";
            return View(roles);
        }

        // GET: ManageRoleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ManageRoleController/Create
        public ActionResult Create()
        {
            ViewData["Title"] = "Add New Role";
            return View();
        }

        // POST: ManageRoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("Name")] Role role)
        {
            if (ModelState.IsValid)
            {
                if (role != null)
                {
                    var result = await _roleManager.CreateAsync(new Role(role.Name));

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(role);
        }

        // GET: ManageRoleController/Edit/5
        public async Task<ActionResult> EditAsync(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (role == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Edit Role";
            return View(role);
        }

        // POST: ManageRoleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, [Bind("Id, Name")] Role role)
        {
            if (id != role.Id)
            {
                Console.WriteLine($"not found, id={id}, role.id={role.Id}");
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(role);
        }

        // GET: ManageRoleController/Delete/5
        public async Task<ActionResult> DeleteAsync(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (role == null)
            {
                return NotFound();
            }
            ViewData["Title"] = "Edit Role";
            return View(role);
        }

        // POST: ManageRoleController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync(int id)
        {
            if (_roleManager.Roles == null)
            {
                return Problem("Entity set 'Roles' is null.");
            }
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role != null)
            {
                _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }
    }
}
