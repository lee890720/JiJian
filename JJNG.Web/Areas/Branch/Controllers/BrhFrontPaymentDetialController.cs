using JJNG.Data;
using JJNG.Data.AppIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize(Roles = "Admins,前台,前台审核")]
    public class BrhFrontPaymentDetialController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhFrontPaymentDetialController(AppDbContext context, AppIdentityDbContext identityContext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identityContext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index(long id)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            var account = _context.BrhFrontDeskAccounts.SingleOrDefault(x => x.FrontDeskAccountsId == id);
            var details = _context.BrhFrontPaymentDetials.Include(b => b.BrhFrontDeskAccounts).Where(x => x.FrontDeskAccountsId == id).ToList();
            return View(details);
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

            return PartialView("~/Areas/Branch/Views/BrhFrontPaymentDetial/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection form)
        {
            var brhFrontPaymentDetial = await _context.BrhFrontPaymentDetials.SingleOrDefaultAsync(m => m.FrontPaymentDetialId == id);
            _context.BrhFrontPaymentDetials.Remove(brhFrontPaymentDetial);
            await _context.SaveChangesAsync();

            var total = _context.BrhFrontPaymentDetials.Where(x => x.FrontDeskAccountsId == brhFrontPaymentDetial.FrontDeskAccountsId).Sum(x => x.PayAmount);
            var brhaccount = _context.BrhFrontDeskAccounts.SingleOrDefault(x => x.FrontDeskAccountsId == brhFrontPaymentDetial.FrontDeskAccountsId);
            brhaccount.Received = total;
            if (brhaccount.Received == brhaccount.Receivable)
                brhaccount.IsFinish = true;
            else
                brhaccount.IsFinish = false;

            _context.Update(brhaccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = brhFrontPaymentDetial.FrontDeskAccountsId });
        }

        private bool BrhFrontPaymentDetialExists(int id)
        {
            return _context.BrhFrontPaymentDetials.Any(e => e.FrontPaymentDetialId == id);
        }
    }
}
