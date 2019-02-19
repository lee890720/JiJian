using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize(Roles = "Admins,管家,管家审核")]
    public class BrhClientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identitycontext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhClientController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identitycontext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            if (_user.Position.Contains("普通"))
                return View(await _context.BrhClient.Where(x => x.Branch == _user.Branch && x.EnteringStaff == _user.UserName).ToListAsync());
            return View(await _context.BrhClient.Where(x => x.Branch == _user.Branch).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            return PartialView("~/Areas/Branch/Views/BrhClient/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,EnteringDate,Name,Follow,IsGood,IsSale,EnteringStaff,Branch,Note")] BrhClient brhClient)
        {
            if (ModelState.IsValid)
            {
                brhClient.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                _context.Add(brhClient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Branch/Views/BrhClient/CreateEdit.cshtml", brhClient);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            var brhClient = await _context.BrhClient.SingleOrDefaultAsync(m => m.ClientId == id);
            if (brhClient == null)
            {
                return NotFound();
            }
            return PartialView("~/Areas/Branch/Views/BrhClient/CreateEdit.cshtml", brhClient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,EnteringDate,Name,Follow,IsGood,IsSale,EnteringStaff,Branch,Note")] BrhClient brhClient)
        {
            if (id != brhClient.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    brhClient.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                    _context.Update(brhClient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrhClientExists(brhClient.ClientId))
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
            return PartialView("~/Areas/Branch/Views/BrhClient/CreateEdit.cshtml", brhClient);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhClient = await _context.BrhClient
                .SingleOrDefaultAsync(m => m.ClientId == id);
            if (brhClient == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Branch/Views/BrhClient/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var brhClient = await _context.BrhClient.SingleOrDefaultAsync(m => m.ClientId == id);
            _context.BrhClient.Remove(brhClient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrhClientExists(int id)
        {
            return _context.BrhClient.Any(e => e.ClientId == id);
        }
    }
}
