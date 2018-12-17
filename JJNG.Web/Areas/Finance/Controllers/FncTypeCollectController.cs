using JJNG.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,财务")]

    public class FncTypeCollectController : Controller
    {
        private readonly AppDbContext _context;

        public FncTypeCollectController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(Tuple.Create(await _context.FncChannelType.ToListAsync(), await _context.FncPaymentType.ToListAsync(),
                await _context.FncEarningType.ToListAsync(), await _context.FncExpendType.ToListAsync()));
        }
    }
}