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
  [Authorize(Roles = "Admins,前台")]
    public class BrhImprestRecordController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identitycontext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhImprestRecordController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identitycontext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index()
        {
    AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["BelongTo"] = _user.BelongTo;
            var appDbContext = _context.BrhImprestRecord.Include(b => b.BrhImprestAccounts);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
          AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["BelongTo"] = _user.BelongTo;
            ViewData["ImprestAccountsId"] = new SelectList(_context.BrhImprestAccounts, "ImprestAccountsId", "ImprestAccountsId");
      return PartialView("~/Areas/Branch/Views/BrhImprestRecord/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImprestRecordId,ImprestAccountsId,EnteringDate,ExpendType,Purpose,Amount,PaymentType,ConnectNumber,EnteringStaff,IsFinance,Branch,Note")] BrhImprestRecord brhImprestRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brhImprestRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ImprestAccountsId"] = new SelectList(_context.BrhImprestAccounts, "ImprestAccountsId", "ImprestAccountsId", brhImprestRecord.ImprestAccountsId);
         return PartialView("~/Areas/Branch/Views/BrhImprestRecord/CreateEdit.cshtml",brhImprestRecord);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhImprestRecord = await _context.BrhImprestRecord.SingleOrDefaultAsync(m => m.ImprestRecordId == id);
            if (brhImprestRecord == null)
            {
                return NotFound();
            }
            ViewData["ImprestAccountsId"] = new SelectList(_context.BrhImprestAccounts, "ImprestAccountsId", "ImprestAccountsId", brhImprestRecord.ImprestAccountsId);
    return PartialView("~/Areas/Branch/Views/BrhImprestRecord/CreateEdit.cshtml",brhImprestRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImprestRecordId,ImprestAccountsId,EnteringDate,ExpendType,Purpose,Amount,PaymentType,ConnectNumber,EnteringStaff,IsFinance,Branch,Note")] BrhImprestRecord brhImprestRecord)
        {
            if (id != brhImprestRecord.ImprestRecordId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brhImprestRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrhImprestRecordExists(brhImprestRecord.ImprestRecordId))
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
            ViewData["ImprestAccountsId"] = new SelectList(_context.BrhImprestAccounts, "ImprestAccountsId", "ImprestAccountsId", brhImprestRecord.ImprestAccountsId);
           return PartialView("~/Areas/Branch/Views/BrhImprestRecord/CreateEdit.cshtml",brhImprestRecord);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhImprestRecord = await _context.BrhImprestRecord
                .Include(b => b.BrhImprestAccounts)
                .SingleOrDefaultAsync(m => m.ImprestRecordId == id);
            if (brhImprestRecord == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Branch/Views/BrhImprestRecord/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var brhImprestRecord = await _context.BrhImprestRecord.SingleOrDefaultAsync(m => m.ImprestRecordId == id);
            _context.BrhImprestRecord.Remove(brhImprestRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrhImprestRecordExists(int id)
        {
            return _context.BrhImprestRecord.Any(e => e.ImprestRecordId == id);
        }
    }
}
