using JJNG.Data;
using JJNG.Data.AppIdentity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.ViewComponents
{
    public class BranchViewComponent:ViewComponent
    {
        private readonly AppIdentityDbContext _context;
        public BranchViewComponent(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list_branch = await _context.UserBranch.Where(x => x.BranchName != "运营中心" && x.BranchName != "町隐学院").ToListAsync();
            return View(list_branch);
        }
    }
}
