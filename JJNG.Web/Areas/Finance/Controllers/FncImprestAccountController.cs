using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,财务")]
    public class FncImprestAccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public FncImprestAccountController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["BelongTo"] = _user.BelongTo;
            ViewData["Department"] = _user.Department;
            var brhImprestAccounts = await _context.BrhImprestAccounts.ToListAsync();
            ViewData["Total"] = brhImprestAccounts.Sum(x => x.Balance);
            return View( brhImprestAccounts);
        }

        public async Task<IActionResult> Create()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["BelongTo"] = _user.BelongTo;

            var list_department = _identityContext.UserDepartment.ToList();
            var list_belongto = _identityContext.UserBelongTo.ToList();
            ViewData["Department"] = new SelectList(list_department, "DepartmentName", "DepartmentName");
            ViewData["BelongTo"] = new SelectList(list_belongto, "BelongToName", "BelongToName");

            return PartialView("~/Areas/Finance/Views/FncImprestAccount/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImprestAccountsId,ImprestAccountsName,Balance,Equity,Manager,Department,BelongTo")] BrhImprestAccounts brhImprestAccounts)
        {
            if (ModelState.IsValid)
            {
                brhImprestAccounts.Equity = brhImprestAccounts.Balance;
                _context.Add(brhImprestAccounts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Finance/Views/FncImprestAccount/CreateEdit.cshtml", brhImprestAccounts);
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
            var list_department = _identityContext.UserDepartment.ToList();
            var list_belongto = _identityContext.UserBelongTo.ToList();
            ViewData["Department"] = new SelectList(list_department, "DepartmentName", "DepartmentName", brhImprestAccounts.Department);
            ViewData["BelongTo"] = new SelectList(list_belongto, "BelongToName", "BelongToName", brhImprestAccounts.BelongTo);

            return PartialView("~/Areas/Finance/Views/FncImprestAccount/CreateEdit.cshtml", brhImprestAccounts);
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
                    var total = _context.BrhImprestRecord.Where(x => x.ImprestAccountsId == brhImprestAccounts.ImprestAccountsId && !x.IsFinance).Sum(x => x.Amount);
                    brhImprestAccounts.Equity = brhImprestAccounts.Balance - total;
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
            return PartialView("~/Areas/Finance/Views/FncImprestAccount/CreateEdit.cshtml", brhImprestAccounts);
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

            return PartialView("~/Areas/Finance/Views/FncImprestAccount/Delete.cshtml", brhImprestAccounts.ImprestAccountsName);
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
