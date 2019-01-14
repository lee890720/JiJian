using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Personnel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Personnel.Controllers
{
    [Area("Personnel")]
    [Authorize(Roles = "Admins,人事")]
    public class PsnAddressAccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public PsnAddressAccountController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
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
            var psnAddressAccount = await _context.PsnAddressAccount.ToListAsync();
            return View(psnAddressAccount);
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

            return PartialView("~/Areas/Personnel/Views/PsnAddressAccount/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AddressAccountId,AccountName,Manager,Department,BelongTo")] PsnAddressAccount psnAddressAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(psnAddressAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Personnel/Views/PsnAddressAccount/CreateEdit.cshtml", psnAddressAccount);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var psnAddressAccount = await _context.PsnAddressAccount.SingleOrDefaultAsync(m => m.AddressAccountId == id);
            if (psnAddressAccount == null)
            {
                return NotFound();
            }
            var list_department = _identityContext.UserDepartment.ToList();
            var list_belongto = _identityContext.UserBelongTo.ToList();
            ViewData["Department"] = new SelectList(list_department, "DepartmentName", "DepartmentName", psnAddressAccount.Department);
            ViewData["BelongTo"] = new SelectList(list_belongto, "BelongToName", "BelongToName", psnAddressAccount.BelongTo);

            return PartialView("~/Areas/Personnel/Views/PsnAddressAccount/CreateEdit.cshtml", psnAddressAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AddressAccountId,AccountName,Manager,Department,BelongTo")] PsnAddressAccount psnAddressAccount)
        {
            if (id != psnAddressAccount.AddressAccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(psnAddressAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PsnAddressAccountExists(psnAddressAccount.AddressAccountId))
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
            return PartialView("~/Areas/Personnel/Views/PsnAddressAccount/CreateEdit.cshtml", psnAddressAccount);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var psnAddressAccount = await _context.PsnAddressAccount
                .SingleOrDefaultAsync(m => m.AddressAccountId == id);
            if (psnAddressAccount == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Personnel/Views/PsnAddressAccount/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var psnAddressAccount = await _context.PsnAddressAccount.SingleOrDefaultAsync(m => m.AddressAccountId == id);
            _context.PsnAddressAccount.Remove(psnAddressAccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PsnAddressAccountExists(int id)
        {
            return _context.PsnAddressAccount.Any(e => e.AddressAccountId == id);
        }
    }
}
