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
    [Authorize(Roles = "Admins,管理员")]
    public class UserPositionController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public UserPositionController(AppIdentityDbContext context)
        {
            _context = context;
        }

        // GET: AppIdentity/UserPosition
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserPosition.ToListAsync());
        }

        public IActionResult Create()
        {
            return PartialView("~/Areas/AppIdentity/Views/UserPosition/CreateEdit.cshtml");
        }

        // POST: AppIdentity/UserPosition/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PositionId,PositionName")] UserPosition userPosition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userPosition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/AppIdentity/Views/UserPosition/CreateEdit.cshtml", userPosition);
        }

        // GET: AppIdentity/UserPosition/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPosition = await _context.UserPosition.SingleOrDefaultAsync(m => m.PositionId == id);
            if (userPosition == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/AppIdentity/Views/UserPosition/CreateEdit.cshtml", userPosition);
        }

        // POST: AppIdentity/UserPosition/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PositionId,PositionName")] UserPosition userPosition)
        {
            if (id != userPosition.PositionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userPosition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPositionExists(userPosition.PositionId))
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
            return PartialView("~/Areas/AppIdentity/Views/UserPosition/CreateEdit.cshtml", userPosition);
        }

        // GET: AppIdentity/UserPosition/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userPosition = await _context.UserPosition
                .SingleOrDefaultAsync(m => m.PositionId == id);
            if (userPosition == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/AppIdentity/Views/UserPosition/Delete.cshtml",userPosition.PositionName);
        }

        // POST: AppIdentity/UserPosition/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var userPosition = await _context.UserPosition.SingleOrDefaultAsync(m => m.PositionId == id);
            _context.UserPosition.Remove(userPosition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserPositionExists(int id)
        {
            return _context.UserPosition.Any(e => e.PositionId == id);
        }
    }
}
