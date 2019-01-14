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
    public class UserBelongToController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public UserBelongToController(AppIdentityDbContext context)
        {
            _context = context;
        }

        // GET: AppIdentity/UserBelongTo
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserBelongTo.ToListAsync());
        }

        // GET: AppIdentity/UserBelongTo/Create
        public IActionResult Create()
        {
            return PartialView("~/Areas/AppIdentity/Views/UserBelongTo/CreateEdit.cshtml");
        }

        // POST: AppIdentity/UserBelongTo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BelongToId,BelongToName")] UserBelongTo userBelongTo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userBelongTo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/AppIdentity/Views/UserBelongTo/CreateEdit.cshtml", userBelongTo);
        }

        // GET: AppIdentity/UserBelongTo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBelongTo = await _context.UserBelongTo.SingleOrDefaultAsync(m => m.BelongToId == id);
            if (userBelongTo == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/AppIdentity/Views/UserBelongTo/CreateEdit.cshtml", userBelongTo);
        }

        // POST: AppIdentity/UserBelongTo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BelongToId,BelongToName")] UserBelongTo userBelongTo)
        {
            if (id != userBelongTo.BelongToId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userBelongTo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserBelongToExists(userBelongTo.BelongToId))
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
            return PartialView("~/Areas/AppIdentity/Views/UserBelongTo/CreateEdit.cshtml", userBelongTo);
        }

        // GET: AppIdentity/UserBelongTo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBelongTo = await _context.UserBelongTo
                .SingleOrDefaultAsync(m => m.BelongToId == id);
            if (userBelongTo == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/AppIdentity/Views/UserBelongTo/Delete.cshtml",userBelongTo.BelongToName);
        }

        // POST: AppIdentity/UserBelongTo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var userBelongTo = await _context.UserBelongTo.SingleOrDefaultAsync(m => m.BelongToId == id);
            _context.UserBelongTo.Remove(userBelongTo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserBelongToExists(int id)
        {
            return _context.UserBelongTo.Any(e => e.BelongToId == id);
        }
    }
}
