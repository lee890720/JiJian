using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JJNG.Data.AppIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace JJNG.Web.Areas.AppIdentity.Controllers
{
    [Area("AppIdentity")]
    [Authorize(Roles = "Admins,管理员,人事")]
    public class UserDepartmentController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public UserDepartmentController(AppIdentityDbContext context)
        {
            _context = context;
        }

        // GET: AppIdentity/UserDepartment
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserDepartment.ToListAsync());
        }

        public IActionResult Create()
        {
            return PartialView("~/Areas/AppIdentity/Views/UserDepartment/CreateEdit.cshtml");
        }

        // POST: AppIdentity/UserDepartment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentId,DepartmentName")] UserDepartment userDepartment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userDepartment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/AppIdentity/Views/UserDepartment/CreateEdit.cshtml", userDepartment);
        }

        // GET: AppIdentity/UserDepartment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDepartment = await _context.UserDepartment.SingleOrDefaultAsync(m => m.DepartmentId == id);
            if (userDepartment == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/AppIdentity/Views/UserDepartment/CreateEdit.cshtml", userDepartment);
        }

        // POST: AppIdentity/UserDepartment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentId,DepartmentName")] UserDepartment userDepartment)
        {
            if (id != userDepartment.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userDepartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDepartmentExists(userDepartment.DepartmentId))
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
            return PartialView("~/Areas/AppIdentity/Views/UserDepartment/CreateEdit.cshtml", userDepartment);
        }

        // GET: AppIdentity/UserDepartment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDepartment = await _context.UserDepartment
                .SingleOrDefaultAsync(m => m.DepartmentId == id);
            if (userDepartment == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/AppIdentity/Views/UserDepartment/Delete.cshtml",userDepartment.DepartmentName);
        }

        // POST: AppIdentity/UserDepartment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var userDepartment = await _context.UserDepartment.SingleOrDefaultAsync(m => m.DepartmentId == id);
            _context.UserDepartment.Remove(userDepartment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserDepartmentExists(int id)
        {
            return _context.UserDepartment.Any(e => e.DepartmentId == id);
        }
    }
}
