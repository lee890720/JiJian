using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,财务")]
    public class FncImprestRecordController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public FncImprestRecordController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index(int? id)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Branch"] = _user.Branch;
            ViewData["ImprestAccountsId"] = id;

            var brhImprestAccounts = await _context.BrhImprestRecord.Include(b => b.BrhImprestAccounts).Where(x=>x.ImprestAccountsId==id&&!x.IsMove).ToListAsync();

            return View(brhImprestAccounts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(List<int> ids)
        {
            int id = 0;
            if (ids.Count > 0)
            {
                _context.BrhImprestRecord.Where(x => ids.Contains(x.ImprestRecordId) && !x.IsFinance).ToList().ForEach(x =>
                {
                    id = x.ImprestAccountsId;
                    x.IsFinance = true;
                    _context.Update(x);
                });
                _context.SaveChanges();

                var total = _context.BrhImprestRecord.Where(x => x.ImprestAccountsId == id && !x.IsMove).Sum(x => x.Amount);
                var brhImprestAccount = _context.BrhImprestAccounts.SingleOrDefault(x => x.ImprestAccountsId == id);
                brhImprestAccount.Equity = brhImprestAccount.Balance - total;

                _context.Update(brhImprestAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Move(int? id)
        {
            _context.BrhImprestRecord.Include(b => b.BrhImprestAccounts).Where(x => x.ImprestAccountsId == id && x.IsFinance && !x.IsMove).ToList().ForEach(x=>
            {
                x.IsMove = true;
                _context.Update(x);
                var expendRecord = new BrhExpendRecord();
                expendRecord.EnteringDate = x.EnteringDate;
                expendRecord.ExpendType = x.ExpendType;
                expendRecord.Purpose = x.Purpose;
                expendRecord.Amount = x.Amount;
                expendRecord.PaymentType = x.PaymentType;
                expendRecord.ConnectNumber = x.ConnectNumber;
                expendRecord.Branch = x.Branch;
                expendRecord.EnteringStaff = x.EnteringStaff;
                expendRecord.IsFinance = x.IsFinance;
                expendRecord.Note = "备用金转入-" + x.Note;
                _context.Add(expendRecord);
            });
            _context.SaveChanges();

            var total = _context.BrhImprestRecord.Where(x => x.ImprestAccountsId == id && !x.IsMove).Sum(x => x.Amount);
            var brhImprestAccount = _context.BrhImprestAccounts.SingleOrDefault(x => x.ImprestAccountsId == id);
            brhImprestAccount.Equity = brhImprestAccount.Balance - total;

            _context.Update(brhImprestAccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = id });

        }
    }
}
