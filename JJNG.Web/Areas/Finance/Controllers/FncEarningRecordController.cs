﻿using JJNG.Data;
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
    public class FncEarningRecordController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public FncEarningRecordController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index(string branch)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;

            var list_branch = _identityContext.UserBranch.Where(x => x.BranchName != "运营中心"&& x.BranchName != "町隐学院").ToList();
            List<BrhEarningRecord> brhEarningRecord = new List<BrhEarningRecord>();

            if (string.IsNullOrEmpty(branch))
                brhEarningRecord = await _context.BrhEarningRecord.ToListAsync();
            else
                brhEarningRecord = await _context.BrhEarningRecord.Where(x => x.Branch == branch).ToListAsync();

            return View(Tuple.Create<List<BrhEarningRecord>, List<UserBranch>>(brhEarningRecord, list_branch));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(List<int> ids)
        {
            if (ids.Count > 0)
            {
                _context.BrhEarningRecord.Where(x => ids.Contains(x.EarningRecordId) && !x.IsFinance).ToList().ForEach(x =>
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