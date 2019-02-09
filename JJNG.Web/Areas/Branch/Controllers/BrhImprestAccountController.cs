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
    [Authorize(Roles = "Admins,前台,管家")]
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
            ViewData["Branch"] = _user.Branch;
            ViewData["Department"] = _user.Department;
            var brhImprestAccounts = await _context.BrhImprestAccounts.Where(x => x.Branch == _user.Branch && x.Department == _user.Department&&(string.IsNullOrEmpty(x.Manager)||x.Manager==_user.UserName)).ToListAsync();
            ViewData["Total"] = brhImprestAccounts.Sum(x => x.Balance);
            return View( brhImprestAccounts);
        }
    }
}
