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
 [Authorize(Roles = "Admins,管理员,人事")]
    public class UserBranchDetialController : Controller
    {
        private readonly AppIdentityDbContext _context;

        public UserBranchDetialController(AppIdentityDbContext context)
        {
            _context = context;
        }

        // GET: AppIdentity/UserBranchDetial
        public async Task<IActionResult> Index(int? id)
        {
            ViewData["BranchId"] = id;
            var appIdentityDbContext = _context.UserBranchDetial.Include(u => u.UserBranch).Where(x=>x.BranchId==id);
            return View(await appIdentityDbContext.ToListAsync());
        }

        public IActionResult Create(int? id)
        {
            ViewData["BranchName"] = _context.UserBranch.SingleOrDefault(x => x.BranchId == id).BranchName;
            ViewData["BranchId"] = id;
            return PartialView("~/Areas/AppIdentity/Views/UserBranchDetial/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BranchDetialId,BranchId,HouseNumber")] UserBranchDetial userBranchDetial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userBranchDetial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),new { id = userBranchDetial.BranchId });
            }
            ViewData["BranchId"] = new SelectList(_context.UserBranch, "BranchId", "BranchName", userBranchDetial.BranchId);
   return PartialView("~/Areas/AppIdentity/Views/UserBranchDetial/CreateEdit.cshtml", userBranchDetial);
        }

        // GET: AppIdentity/UserBranchDetial/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBranchDetial = await _context.UserBranchDetial.SingleOrDefaultAsync(m => m.BranchDetialId == id);
            if (userBranchDetial == null)
            {
                return NotFound();
            }
            ViewData["BranchName"] = _context.UserBranch.SingleOrDefault(x => x.BranchId == userBranchDetial.BranchId).BranchName;
            return PartialView("~/Areas/AppIdentity/Views/UserBranchDetial/CreateEdit.cshtml", userBranchDetial);
        }

        // POST: AppIdentity/UserBranchDetial/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BranchDetialId,BranchId,HouseNumber")] UserBranchDetial userBranchDetial)
        {
            if (id != userBranchDetial.BranchDetialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userBranchDetial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserBranchDetialExists(userBranchDetial.BranchDetialId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),new { id=userBranchDetial.BranchId});
            }
            ViewData["BranchId"] = new SelectList(_context.UserBranch, "BranchId", "BranchId", userBranchDetial.BranchId);
   return PartialView("~/Areas/AppIdentity/Views/UserBranchDetial/CreateEdit.cshtml", userBranchDetial);
        }

        // GET: AppIdentity/UserBranchDetial/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBranchDetial = await _context.UserBranchDetial
                .Include(u => u.UserBranch)
                .SingleOrDefaultAsync(m => m.BranchDetialId == id);
            if (userBranchDetial == null)
            {
                return NotFound();
            }

      return PartialView("~/Areas/AppIdentity/Views/UserBranchDetial/Delete.cshtml", userBranchDetial.HouseNumber);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var userBranchDetial = await _context.UserBranchDetial.SingleOrDefaultAsync(m => m.BranchDetialId == id);
            _context.UserBranchDetial.Remove(userBranchDetial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index),new { id=userBranchDetial.BranchId});
        }

        private bool UserBranchDetialExists(int id)
        {
            return _context.UserBranchDetial.Any(e => e.BranchDetialId == id);
        }
    }
}
