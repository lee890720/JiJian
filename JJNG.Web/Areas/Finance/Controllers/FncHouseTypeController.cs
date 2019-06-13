using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JJNG.Data;
using JJNG.Data.Finance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,管理员,人事,财务")]
    public class FncHouseTypeController : Controller
    {
        private readonly AppDbContext _context;

        public FncHouseTypeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id)
        {
            var fncHouseType = _context.FncHouseType.Include(f => f.FncBranch).Include(x => x.FncHouseNumber).Where(x => x.BranchId == id).ToList();
            ViewData["BranchId"] = id;
            ViewData["Branch"] = fncHouseType[0].FncBranch.BranchName;
            return View(fncHouseType);
        }
    }
}
