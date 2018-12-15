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
    public class BrhImprestAccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identitycontext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhImprestAccountController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
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
            return View(await _context.BrhImprestAccounts.ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["BelongTo"] = _user.BelongTo;
            var list_department = _identitycontext.UserDepartment.ToList();
            var list_belongto = _identitycontext.UserBelongTo.ToList();
            ViewData["Department"] = new SelectList(list_department, "DepartmentName", "DepartmentName");
            ViewData["BelongTo"] = new SelectList(list_belongto, "BelongToName", "BelongToName");
            return PartialView("~/Areas/Branch/Views/BrhImprestAccount/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImprestAccountsId,ImprestAccountsName,Balance,Equity,Manager,Department,BelongTo")] BrhImprestAccounts brhImprestAccounts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brhImprestAccounts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
      return PartialView("~/Areas/Branch/Views/BrhImprestAccount/CreateEdit.cshtml",brhImprestAccounts);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhImprestAccounts = await _context.BrhImprestAccounts.SingleOrDefaultAsync(m => m.ImprestAccountsId == id);
            if (brhImprestAccounts == null)
            {
                return NotFound();
            }
            var list_department = _identitycontext.UserDepartment.ToList();
            var list_belongto = _identitycontext.UserBelongTo.ToList();
            ViewData["Department"] = new SelectList(list_department, "DepartmentName", "DepartmentName",brhImprestAccounts.Department);
            ViewData["BelongTo"] = new SelectList(list_belongto, "BelongToName", "BelongToName",brhImprestAccounts.BelongTo);
            return PartialView("~/Areas/Branch/Views/BrhImprestAccount/CreateEdit.cshtml",brhImprestAccounts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImprestAccountsId,ImprestAccountsName,Balance,Equity,Manager,Department,BelongTo")] BrhImprestAccounts brhImprestAccounts)
        {
            if (id != brhImprestAccounts.ImprestAccountsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brhImprestAccounts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrhImprestAccountsExists(brhImprestAccounts.ImprestAccountsId))
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
  return PartialView("~/Areas/Branch/Views/BrhImprestAccount/CreateEdit.cshtml",brhImprestAccounts);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhImprestAccounts = await _context.BrhImprestAccounts
                .SingleOrDefaultAsync(m => m.ImprestAccountsId == id);
            if (brhImprestAccounts == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Branch/Views/BrhImpresetAccount/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var brhImprestAccounts = await _context.BrhImprestAccounts.SingleOrDefaultAsync(m => m.ImprestAccountsId == id);
            _context.BrhImprestAccounts.Remove(brhImprestAccounts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrhImprestAccountsExists(int id)
        {
            return _context.BrhImprestAccounts.Any(e => e.ImprestAccountsId == id);
        }
    }
}
