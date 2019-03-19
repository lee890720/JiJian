using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
using JJNG.Data.Finance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
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

        public async Task<ActionResult> Index(string branchName = "既见·南国", int branchId = 2, int count = 15)
        {
            ViewData["BranchId"] = branchId;
            var fncBranch = new FncBranch();
            fncBranch.BranchName = branchName;
            fncBranch.BranchId = branchId;
            fncBranch.Count = count;
            var list_branch = await _context.FncBranch.Where(x => x.BranchName != "运营中心" && x.BranchName != "町隐学院").ToListAsync();
            return View(Tuple.Create<FncBranch, List<FncBranch>>(fncBranch, list_branch));
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
        public async Task<JsonResult> UpdateList([FromBody]FDAParams fdaParams)
        {
            if (fdaParams.Ids.Count > 0)
            {
                _context.BrhFrontDeskAccounts.Where(x => fdaParams.Ids.Contains(x.FrontDeskAccountsId) && !x.IsFinance).ToList().ForEach(x =>
                {
                    x.IsFinance = true;
                    _context.Update(x);
                });
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            var frontList = await _context.BrhFrontDeskAccounts.Include(x=>x.BrhFrontPaymentDetial).Where(x => x.Branch == fdaParams.BranchName && x.State != StateType.已删除 && DateTime.Compare(fdaParams.StartDate, x.EndDate) <= 0 && DateTime.Compare(x.StartDate, fdaParams.EndDate) < 0).ToListAsync();
            return Json(new { frontList });
            //return RedirectToAction(nameof(Index));
        }

        public IActionResult DetialList(long? id)
        {
            var brhFrontPaymentDetials = _context.BrhFrontPaymentDetials.Where(x => x.FrontDeskAccountsId == id).ToList();

            return PartialView("~/Areas/Finance/Views/FncFrontDeskAccount/DetialList.cshtml", brhFrontPaymentDetials);
        }

        public async Task<JsonResult> GetFrontList([FromBody]FDAParams fdaParams)
        {
            var frontList =await _context.BrhFrontDeskAccounts.Include(x=>x.BrhFrontPaymentDetial).Where(x => x.Branch == fdaParams.BranchName && x.State != StateType.已删除 && DateTime.Compare(fdaParams.StartDate, x.EndDate) <= 0 && DateTime.Compare(x.StartDate, fdaParams.EndDate) < 0).ToListAsync();
            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.MaxDepth = 2;
            //settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; //设置不处理循环引用
            return Json(new { frontList });
        }
    }

    public class FDAParams:FncBranch
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<long> Ids { get; set; }
    }
}
