﻿using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
using JJNG.Data.Finance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,财务")]
    public class FncStewardAccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public FncStewardAccountController(AppDbContext context, AppIdentityDbContext identityContext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identityContext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index(string branch)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;

            var list_branch = _context.FncBranch.Where(x => x.BranchName != "运营中心"&& x.BranchName != "町隐学院").ToList();
            List<BrhStewardAccounts> brhStewardAccounts = new List<BrhStewardAccounts>();

            if (string.IsNullOrEmpty(branch))
                brhStewardAccounts = await _context.BrhStewardAccounts.ToListAsync();
            else
                brhStewardAccounts = await _context.BrhStewardAccounts.Where(x => x.Branch == branch).ToListAsync();

            return View(Tuple.Create<List<BrhStewardAccounts>, List<FncBranch>>(brhStewardAccounts, list_branch));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(List<long> ids)
        {
            if (ids.Count > 0)
            {
                _context.BrhStewardAccounts.Where(x => ids.Contains(x.StewardAccountsId) && !x.IsFinance).ToList().ForEach(x =>
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
            var brhStewardPaymentDetials = _context.BrhStewardPaymentDetial.Where(x => x.StewardAccountsId == id).ToList();
            return PartialView("~/Areas/Finance/Views/FncStewardAccount/DetialList.cshtml", brhStewardPaymentDetials);
        }
    }
}
