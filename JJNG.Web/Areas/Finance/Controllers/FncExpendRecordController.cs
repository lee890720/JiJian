using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,财务")]
    public class FncExpendRecordController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public FncExpendRecordController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index(string branch)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["BelongTo"] = _user.BelongTo;

            var list_branch = _identityContext.UserBelongTo.Where(x => x.BelongToName != "运营中心").ToList();
            List<BrhExpendRecord> brhExpendRecord = new List<BrhExpendRecord>();

            if (string.IsNullOrEmpty(branch))
                brhExpendRecord = await _context.BrhExpendRecord.ToListAsync();
            else
                brhExpendRecord = await _context.BrhExpendRecord.Where(x => x.Branch == branch).ToListAsync();

            return View(Tuple.Create<List<BrhExpendRecord>, List<UserBelongTo>>(brhExpendRecord, list_branch));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(List<int> ids)
        {
            if (ids.Count > 0)
            {
                _context.BrhExpendRecord.Where(x => ids.Contains(x.ExpendRecordId) && !x.IsFinance).ToList().ForEach(x =>
                {
                    x.IsFinance = true;
                    _context.Update(x);
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
