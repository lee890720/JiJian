using JJNG.Data.AppIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.AppIdentity.Controllers
{
    [Area("AppIdentity")]
 [Authorize(Roles = "Admins,管理员")]
    public class UserBelongToDetialController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public UserBelongToDetialController(AppIdentityDbContext context)
        {
            _context = context;
        }

        // GET: AppIdentity/UserBelongToDetial
        public async Task<IActionResult> Index(int? id)
        {
            ViewData["BelongToId"] = id;
            var appIdentityDbContext = _context.UserBelongToDetial.Include(u => u.UserBelongTo).Where(x=>x.BelongToId==id);
            return View(await appIdentityDbContext.ToListAsync());
        }

        public IActionResult Create(int? id)
        {
            ViewData["BelongToName"] = _context.UserBelongTo.SingleOrDefault(x => x.BelongToId == id).BelongToName;
            ViewData["BelongToId"] = id;
            return PartialView("~/Areas/AppIdentity/Views/UserBelongToDetial/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BelongToDetialId,BelongToId,HouseNumber")] UserBelongToDetial userBelongToDetial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userBelongToDetial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),new { id = userBelongToDetial.BelongToId });
            }
            ViewData["BelongToId"] = new SelectList(_context.UserBelongTo, "BelongToId", "BelongToName", userBelongToDetial.BelongToId);
   return PartialView("~/Areas/AppIdentity/Views/UserBelongToDetial/CreateEdit.cshtml", userBelongToDetial);
        }

        // GET: AppIdentity/UserBelongToDetial/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBelongToDetial = await _context.UserBelongToDetial.SingleOrDefaultAsync(m => m.BelongToDetialId == id);
            if (userBelongToDetial == null)
            {
                return NotFound();
            }
            ViewData["BelongToName"] = _context.UserBelongTo.SingleOrDefault(x => x.BelongToId == userBelongToDetial.BelongToId).BelongToName;
            return PartialView("~/Areas/AppIdentity/Views/UserBelongToDetial/CreateEdit.cshtml", userBelongToDetial);
        }

        // POST: AppIdentity/UserBelongToDetial/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BelongToDetialId,BelongToId,HouseNumber")] UserBelongToDetial userBelongToDetial)
        {
            if (id != userBelongToDetial.BelongToDetialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userBelongToDetial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserBelongToDetialExists(userBelongToDetial.BelongToDetialId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),new { id=userBelongToDetial.BelongToId});
            }
            ViewData["BelongToId"] = new SelectList(_context.UserBelongTo, "BelongToId", "BelongToId", userBelongToDetial.BelongToId);
   return PartialView("~/Areas/AppIdentity/Views/UserBelongToDetial/CreateEdit.cshtml", userBelongToDetial);
        }

        // GET: AppIdentity/UserBelongToDetial/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBelongToDetial = await _context.UserBelongToDetial
                .Include(u => u.UserBelongTo)
                .SingleOrDefaultAsync(m => m.BelongToDetialId == id);
            if (userBelongToDetial == null)
            {
                return NotFound();
            }

      return PartialView("~/Areas/AppIdentity/Views/UserBelongToDetial/Delete.cshtml", userBelongToDetial.HouseNumber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var userBelongToDetial = await _context.UserBelongToDetial.SingleOrDefaultAsync(m => m.BelongToDetialId == id);
            _context.UserBelongToDetial.Remove(userBelongToDetial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index),new { id=userBelongToDetial.BelongToId});
        }

        private bool UserBelongToDetialExists(int id)
        {
            return _context.UserBelongToDetial.Any(e => e.BelongToDetialId == id);
        }
    }
}
