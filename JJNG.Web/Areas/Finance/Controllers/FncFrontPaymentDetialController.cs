using JJNG.Data;
using JJNG.Data.AppIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,财务")]
    public class FncFrontPaymentDetialController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public FncFrontPaymentDetialController(AppDbContext context, AppIdentityDbContext identityContext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identityContext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index(string branch)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Position"] = _user.Position;
            ViewData["Branch"] = _user.Branch;
            var appDbContext = _context.BrhFrontPaymentDetials.Include(b => b.BrhFrontDeskAccounts);
            return View(await appDbContext.ToListAsync());
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhFrontPaymentDetial = await _context.BrhFrontPaymentDetials
                .Include(b => b.BrhFrontDeskAccounts)
                .SingleOrDefaultAsync(m => m.FrontPaymentDetialId == id);
            if (brhFrontPaymentDetial == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Finance/Views/FncFrontPaymentDetial/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var brhFrontPaymentDetial = await _context.BrhFrontPaymentDetials.SingleOrDefaultAsync(m => m.FrontPaymentDetialId == id);
            _context.BrhFrontPaymentDetials.Remove(brhFrontPaymentDetial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrhFrontPaymentDetialExists(int id)
        {
            return _context.BrhFrontPaymentDetials.Any(e => e.FrontPaymentDetialId == id);
        }
    }
}
