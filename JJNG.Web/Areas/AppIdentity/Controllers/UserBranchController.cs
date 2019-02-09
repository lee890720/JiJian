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
    public class UserBranchController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public UserBranchController(AppIdentityDbContext context)
        {
            _context = context;
        }

        // GET: AppIdentity/UserBranch
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserBranch.ToListAsync());
        }

        // GET: AppIdentity/UserBranch/Create
        public IActionResult Create()
        {
            return PartialView("~/Areas/AppIdentity/Views/UserBranch/CreateEdit.cshtml");
        }

        // POST: AppIdentity/UserBranch/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BranchId,BranchName")] UserBranch userBranch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userBranch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/AppIdentity/Views/UserBranch/CreateEdit.cshtml", userBranch);
        }

        // GET: AppIdentity/UserBranch/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBranch = await _context.UserBranch.SingleOrDefaultAsync(m => m.BranchId == id);
            if (userBranch == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/AppIdentity/Views/UserBranch/CreateEdit.cshtml", userBranch);
        }

        // POST: AppIdentity/UserBranch/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BranchId,BranchName")] UserBranch userBranch)
        {
            if (id != userBranch.BranchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userBranch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserBranchExists(userBranch.BranchId))
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
            return PartialView("~/Areas/AppIdentity/Views/UserBranch/CreateEdit.cshtml", userBranch);
        }

        // GET: AppIdentity/UserBranch/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBranch = await _context.UserBranch
                .SingleOrDefaultAsync(m => m.BranchId == id);
            if (userBranch == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/AppIdentity/Views/UserBranch/Delete.cshtml",userBranch.BranchName);
        }

        // POST: AppIdentity/UserBranch/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var userBranch = await _context.UserBranch.SingleOrDefaultAsync(m => m.BranchId == id);
            _context.UserBranch.Remove(userBranch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserBranchExists(int id)
        {
            return _context.UserBranch.Any(e => e.BranchId == id);
        }
    }
}
