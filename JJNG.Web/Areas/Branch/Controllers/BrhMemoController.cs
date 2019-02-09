using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JJNG.Data;
using JJNG.Data.Branch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using JJNG.Data.AppIdentity;
using Microsoft.AspNetCore.Identity;
using JJNG.Web;

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize(Roles = "Admins,前台,管家")]
    public class BrhMemoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identitycontext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhMemoController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
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
            return View(await _context.BrhMemo.Where(x => x.Branch == _user.Branch).ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            return PartialView("~/Areas/Branch/Views/BrhMemo/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemoId,EnteringDate,Memo,IsFinish,EnteringStaff,Branch,Note")] BrhMemo brhMemo)
        {
            if (ModelState.IsValid)
            {
                brhMemo.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                _context.Add(brhMemo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Branch/Views/BrhMemo/CreateEdit.cshtml", brhMemo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhMemo = await _context.BrhMemo.SingleOrDefaultAsync(m => m.MemoId == id);
            if (brhMemo == null)
            {
                return NotFound();
            }
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            return PartialView("~/Areas/Branch/Views/BrhMemo/CreateEdit.cshtml", brhMemo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemoId,EnteringDate,Memo,EnteringStaff,Branch,Note")] BrhMemo brhMemo)
        {
            if (id != brhMemo.MemoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    brhMemo.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                    _context.Update(brhMemo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrhMemoExists(brhMemo.MemoId))
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
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            return PartialView("~/Areas/Branch/Views/BrhMemo/CreateEdit.cshtml", brhMemo);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhMemo = await _context.BrhMemo
                .SingleOrDefaultAsync(m => m.MemoId == id);
            if (brhMemo == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Branch/Views/BrhMemo/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var brhMemo = await _context.BrhMemo.SingleOrDefaultAsync(m => m.MemoId == id);
            _context.BrhMemo.Remove(brhMemo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrhMemoExists(int id)
        {
            return _context.BrhMemo.Any(e => e.MemoId == id);
        }
    }
}
