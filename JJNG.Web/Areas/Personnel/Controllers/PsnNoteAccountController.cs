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
    public class PsnNoteAccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public PsnNoteAccountController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            ViewData["Department"] = _user.Department;
            var psnNoteAccount = await _context.PsnNoteAccount.ToListAsync();
            return View(psnNoteAccount);
        }

        public async Task<IActionResult> Create()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;

            var list_department = _identityContext.UserDepartment.ToList();
            var list_belongto = _identityContext.UserBranch.ToList();
            ViewData["Department"] = new SelectList(list_department, "DepartmentName", "DepartmentName");
            ViewData["Branch"] = new SelectList(list_belongto, "BranchName", "BranchName");

            return PartialView("~/Areas/Personnel/Views/PsnNoteAccount/CreateEdit.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NoteAccountId,AccountName,Manager,Department,Branch")] PsnNoteAccount psnNoteAccount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(psnNoteAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Personnel/Views/PsnNoteAccount/CreateEdit.cshtml", psnNoteAccount);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var psnNoteAccount = await _context.PsnNoteAccount.SingleOrDefaultAsync(m => m.NoteAccountId == id);
            if (psnNoteAccount == null)
            {
                return NotFound();
            }
            var list_department = _identityContext.UserDepartment.ToList();
            var list_belongto = _identityContext.UserBranch.ToList();
            ViewData["Department"] = new SelectList(list_department, "DepartmentName", "DepartmentName", psnNoteAccount.Department);
            ViewData["Branch"] = new SelectList(list_belongto, "BranchName", "BranchName", psnNoteAccount.Branch);

            return PartialView("~/Areas/Personnel/Views/PsnNoteAccount/CreateEdit.cshtml", psnNoteAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NoteAccountId,AccountName,Manager,Department,Branch")] PsnNoteAccount psnNoteAccount)
        {
            if (id != psnNoteAccount.NoteAccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(psnNoteAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PsnNoteAccountExists(psnNoteAccount.NoteAccountId))
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
            return PartialView("~/Areas/Personnel/Views/PsnNoteAccount/CreateEdit.cshtml", psnNoteAccount);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var psnNoteAccount = await _context.PsnNoteAccount
                .SingleOrDefaultAsync(m => m.NoteAccountId == id);
            if (psnNoteAccount == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Personnel/Views/PsnNoteAccount/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var psnNoteAccount = await _context.PsnNoteAccount.SingleOrDefaultAsync(m => m.NoteAccountId == id);
            _context.PsnNoteAccount.Remove(psnNoteAccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PsnNoteAccountExists(int id)
        {
            return _context.PsnNoteAccount.Any(e => e.NoteAccountId == id);
        }
    }
}
