using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
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

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize(Roles = "Admins,前台")]
    public class BrhFrontDeskAccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhFrontDeskAccountController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<IActionResult> Index()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["UserName"] = _user.UserName;
            ViewData["Position"] = _user.Position;
            ViewData["BelongTo"] = _user.BelongTo;

            return View(await _context.BrhFrontDeskAccounts.Where(x => x.Branch == _user.BelongTo).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(List<long> ids)
        {
            if (ids.Count > 0)
            {
                _context.BrhFrontDeskAccounts.Where(x => ids.Contains(x.FrontDeskAccountsId) && !x.IsFront).ToList().ForEach(x =>
                  {
                      x.IsFront = true;
                      _context.Update(x);
                  });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewData["UserName"] = _user.UserName;
            ViewData["BelongTo"] = _user.BelongTo;

            var belongToId = _identityContext.UserBelongTo.SingleOrDefault(x => x.BelongToName == _user.BelongTo).BelongToId;
            var list_houseNumber = _identityContext.UserBelongToDetial.Where(x => x.BelongToId == belongToId).ToList();
            ViewData["HouseNumber"] = new SelectList(list_houseNumber, "HouseNumber", "HouseNumber");

            var list_paymentType = _context.FncPaymentType.ToList();
            ViewData["PaymentType"] = new SelectList(list_paymentType, "PaymentType", "PaymentType");
            var list_channelType = _context.FncChannelType.ToList();
            ViewData["ChannelType"] = new SelectList(list_channelType, "ChannelType", "ChannelType");

            ViewData["FrontDeskAccountsId"] = belongToId.ToString() + ConvertJson.DateTimeToStamp(DateTime.Now).ToString();

            return PartialView("~/Areas/Branch/Views/BrhFrontDeskAccount/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FrontDeskAccountsId,EnteringDate,HouseNumber,CustomerName,CustomerCount,StartDate,EndDate,Channel,UnitPrice,TotalPrice,Receivable,Received,IsFinish,EnteringStaff,Steward,FrontDeskLeader,StewardLeader,RelationStaff,IsFront,IsFinance,Branch,Note")] BrhFrontDeskAccounts brhFrontDeskAccounts, string payway1, DateTime? paydate1, double? payamount1, string payway2, DateTime? paydate2, double? payamount2, string payway3, DateTime? paydate3, double? payamount3)
        {
            if (ModelState.IsValid)
            {
                BrhFrontPaymentDetial bfp1 = new BrhFrontPaymentDetial();
                BrhFrontPaymentDetial bfp2 = new BrhFrontPaymentDetial();
                BrhFrontPaymentDetial bfp3 = new BrhFrontPaymentDetial();

                if (!string.IsNullOrEmpty(payway1) && !double.IsNaN((double)payamount1))
                {
                    bfp1.FrontDeskAccountsId = brhFrontDeskAccounts.FrontDeskAccountsId;
                    bfp1.PayWay = payway1;
                    bfp1.PayDate = (DateTime)paydate1;
                    bfp1.PayAmount = (double)payamount1;
                    _context.Add(bfp1);
                }
                else
                    bfp1.PayAmount = 0;

                if (!string.IsNullOrEmpty(payway2) && !double.IsNaN((double)payamount2))
                {
                    bfp2.FrontDeskAccountsId = brhFrontDeskAccounts.FrontDeskAccountsId;
                    bfp2.PayWay = payway2;
                    bfp2.PayDate = (DateTime)paydate2;
                    bfp2.PayAmount = (double)payamount2;
                    _context.Add(bfp2);
                }
                else
                    bfp2.PayAmount = 0;

                if (!string.IsNullOrEmpty(payway3) && !double.IsNaN((double)payamount3))
                {
                    bfp3.FrontDeskAccountsId = brhFrontDeskAccounts.FrontDeskAccountsId;
                    bfp3.PayWay = payway3;
                    bfp3.PayDate = (DateTime)paydate3;
                    bfp3.PayAmount = (double)payamount3;
                    _context.Add(bfp3);
                }
                else
                    bfp3.PayAmount = 0;

                brhFrontDeskAccounts.Received = bfp1.PayAmount + bfp2.PayAmount + bfp3.PayAmount;

                if (brhFrontDeskAccounts.Receivable == brhFrontDeskAccounts.Received)
                    brhFrontDeskAccounts.IsFinish = true;

                brhFrontDeskAccounts.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

                _context.Add(brhFrontDeskAccounts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Branch/Views/BrhFrontDeskAccount/Create.cshtml", brhFrontDeskAccounts);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhFrontDeskAccounts = await _context.BrhFrontDeskAccounts.SingleOrDefaultAsync(m => m.FrontDeskAccountsId == id);
            if (brhFrontDeskAccounts == null)
            {
                return NotFound();
            }

            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);

            var belongToId = _identityContext.UserBelongTo.SingleOrDefault(x => x.BelongToName == _user.BelongTo).BelongToId;
            var list_houseNumber = _identityContext.UserBelongToDetial.Where(x => x.BelongToId == belongToId).ToList();
            ViewData["HouseNumber"] = new SelectList(list_houseNumber, "HouseNumber", "HouseNumber", brhFrontDeskAccounts.HouseNumber);

            var list_channelType = _context.FncChannelType.ToList();
            ViewData["ChannelType"] = new SelectList(list_channelType, "ChannelType", "ChannelType");

            return PartialView("~/Areas/Branch/Views/BrhFrontDeskAccount/Edit.cshtml", brhFrontDeskAccounts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("FrontDeskAccountsId,EnteringDate,HouseNumber,CustomerName,CustomerCount,StartDate,EndDate,Channel,UnitPrice,TotalPrice,Receivable,Received,IsFinish,EnteringStaff,Steward,FrontDeskLeader,StewardLeader,RelationStaff,IsFront,IsFinance,Branch,Note")] BrhFrontDeskAccounts brhFrontDeskAccounts)
        {
            if (id != brhFrontDeskAccounts.FrontDeskAccountsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (brhFrontDeskAccounts.Receivable == brhFrontDeskAccounts.Received)
                        brhFrontDeskAccounts.IsFinish = true;

                    brhFrontDeskAccounts.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

                    _context.Update(brhFrontDeskAccounts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrhFrontDeskAccountsExists(brhFrontDeskAccounts.FrontDeskAccountsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Branch/Views/BrhFrontDeskAccount/Edit.cshtml", brhFrontDeskAccounts);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhFrontDeskAccounts = await _context.BrhFrontDeskAccounts
                .SingleOrDefaultAsync(m => m.FrontDeskAccountsId == id);
            if (brhFrontDeskAccounts == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Branch/Views/BrhFrontDeskAccount/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long? id, IFormCollection form)
        {
            var brhFrontDeskAccounts = await _context.BrhFrontDeskAccounts.SingleOrDefaultAsync(m => m.FrontDeskAccountsId == id);
            _context.BrhFrontDeskAccounts.Remove(brhFrontDeskAccounts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddDetial(long? id)
        {
            ViewData["FrontDeskAccountsId"] = id;
            var list_paymentType = _context.FncPaymentType.ToList();
            ViewData["PaymentType"] = new SelectList(list_paymentType, "PaymentType", "PaymentType");

            return PartialView("~/Areas/Branch/Views/BrhFrontDeskAccount/AddDetial.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetial([Bind("FrontPaymentDetialId,FrontDeskAccountsId,PayDate,PayWay,PayAmount")] BrhFrontPaymentDetial brhFrontPaymentDetial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brhFrontPaymentDetial);
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["FrontDeskAccountsId"] = brhFrontPaymentDetial.FrontDeskAccountsId;
            return PartialView("~/Areas/Branch/Views/BrhFrontDeskAccount/AddDetial.cshtml", brhFrontPaymentDetial);
        }

        public IActionResult DetialList(long? id)
        {
            var brhFrontPaymentDetials = _context.BrhFrontPaymentDetials.Where(x => x.FrontDeskAccountsId == id).ToList();
            return PartialView("~/Areas/Branch/Views/BrhFrontDeskAccount/DetialList.cshtml", brhFrontPaymentDetials);
        }

        private bool BrhFrontDeskAccountsExists(long id)
        {
            return _context.BrhFrontDeskAccounts.Any(e => e.FrontDeskAccountsId == id);
        }
    }
}
