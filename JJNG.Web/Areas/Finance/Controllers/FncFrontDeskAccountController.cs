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
    public class FncFrontDeskAccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public FncFrontDeskAccountController(AppDbContext context, AppIdentityDbContext identityContext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identityContext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index(string branch)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["BelongTo"] = _user.BelongTo;

            var list_branch = _identityContext.UserBelongTo.Where(x => x.BelongToName != "运营中心"&& x.BelongToName != "町隐学院").ToList();
            List<BrhFrontDeskAccounts> brhFrontDeskAccounts = new List<BrhFrontDeskAccounts>();

            if (string.IsNullOrEmpty(branch))
                brhFrontDeskAccounts = await _context.BrhFrontDeskAccounts.ToListAsync();
            else
                brhFrontDeskAccounts = await _context.BrhFrontDeskAccounts.Where(x => x.Branch == branch).ToListAsync();

            return View(Tuple.Create<List<BrhFrontDeskAccounts>, List<UserBelongTo>>(brhFrontDeskAccounts, list_branch));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(List<long> ids)
        {
            if (ids.Count > 0)
            {
                _context.BrhFrontDeskAccounts.Where(x => ids.Contains(x.FrontDeskAccountsId) && !x.IsFinance).ToList().ForEach(x =>
                  {
                      x.IsFinance = true;
                      _context.Update(x);
                  });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DetialList(long? id)
        {
            var brhFrontPaymentDetials = _context.BrhFrontPaymentDetials.Where(x => x.FrontDeskAccountsId == id).ToList();

            return PartialView("~/Areas/Finance/Views/FncFrontDeskAccount/DetialList.cshtml", brhFrontPaymentDetials);
        }
    }
}
