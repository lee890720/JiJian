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
using JJNG.Web.Areas.Branch.Models;

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize(Roles = "Admins,前台")]
    public class BrhStewardAccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public BrhStewardAccountController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
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

            return View(await _context.BrhStewardAccounts.Where(x => x.Branch == _user.BelongTo).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(List<long> ids)
        {
            if (ids.Count > 0)
            {
                _context.BrhStewardAccounts.Where(x => ids.Contains(x.StewardAccountsId) && !x.IsSteward).ToList().ForEach(x =>
                  {
                      x.IsSteward = true;
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

            var list_paymentType = _context.FncPaymentType.ToList();
            ViewData["PaymentType"] = new SelectList(list_paymentType, "PaymentType", "PaymentType");

            ViewData["StewardAccountsId"] = belongToId.ToString() + ConvertJson.DateTimeToStamp(DateTime.Now).ToString();

            return PartialView("~/Areas/Branch/Views/BrhStewardAccount/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StewardAccountsId,EnteringDate,HouseNumber,CustomerName,ProductType,Product,Cost,Amount,Profit,Receivable,Received,IsFinish,EnteringStaff,FrontDesk,FrontDeskLeader,StewardLeader,RelationStaff,IsSteward,IsFinance,Branch,Note,PayWay1,PayDate1,PayAmount1,PayWay2,PayDate2,PayAmount2,PayWay3,PayDate3,PayAmount3")]BrhStewardModel brhStewardModel)
        {
            if (ModelState.IsValid)
            {
                BrhStewardPaymentDetial bfp1 = new BrhStewardPaymentDetial();
                BrhStewardPaymentDetial bfp2 = new BrhStewardPaymentDetial();
                BrhStewardPaymentDetial bfp3 = new BrhStewardPaymentDetial();

                if (brhStewardModel.PayAmount1!=0)
                {
                    bfp1.StewardAccountsId = brhStewardModel.StewardAccountsId;
                    bfp1.PayWay = brhStewardModel.PayWay1;
                    bfp1.PayDate = brhStewardModel.PayDate1;
                    bfp1.PayAmount = brhStewardModel.PayAmount1;
                    _context.Add(bfp1);
                }
                else
                    bfp1.PayAmount = 0;

                if (brhStewardModel.PayAmount2 != 0)
                {
                    bfp2.StewardAccountsId = brhStewardModel.StewardAccountsId;
                    bfp2.PayWay = brhStewardModel.PayWay2;
                    bfp2.PayDate = brhStewardModel.PayDate2;
                    bfp2.PayAmount = brhStewardModel.PayAmount2;
                    _context.Add(bfp2);
                }
                else
                    bfp2.PayAmount = 0;

                if (brhStewardModel.PayAmount3 != 0)
                {
                    bfp3.StewardAccountsId = brhStewardModel.StewardAccountsId;
                    bfp3.PayWay = brhStewardModel.PayWay3;
                    bfp3.PayDate = brhStewardModel.PayDate3;
                    bfp3.PayAmount = brhStewardModel.PayAmount3;
                    _context.Add(bfp3);
                }
                else
                    bfp3.PayAmount = 0;

                BrhStewardAccounts brhStewardAccounts = new BrhStewardAccounts();
                brhStewardAccounts.Amount = brhStewardModel.Amount;
                brhStewardAccounts.Branch = brhStewardModel.Branch;
                brhStewardAccounts.Cost = brhStewardModel.Cost;
                brhStewardAccounts.CustomerName = brhStewardModel.CustomerName;
                brhStewardAccounts.EnteringStaff = brhStewardModel.EnteringStaff;
                brhStewardAccounts.FrontDesk = brhStewardModel.FrontDesk;
                brhStewardAccounts.FrontDeskLeader = brhStewardModel.FrontDeskLeader;
                brhStewardAccounts.HouseNumber = brhStewardModel.HouseNumber;
                brhStewardAccounts.Note = brhStewardModel.Note;
                brhStewardAccounts.Product = brhStewardModel.Product;
                brhStewardAccounts.ProductType = brhStewardModel.ProductType;
                brhStewardAccounts.Profit = brhStewardModel.Profit;
                brhStewardAccounts.Receivable = brhStewardModel.Receivable;
                brhStewardAccounts.RelationStaff = brhStewardModel.RelationStaff;
                brhStewardAccounts.StewardAccountsId = brhStewardModel.StewardAccountsId;
                brhStewardAccounts.StewardLeader = brhStewardModel.StewardLeader;

                brhStewardAccounts.Received = bfp1.PayAmount + bfp2.PayAmount + bfp3.PayAmount;

                if (brhStewardAccounts.Receivable == brhStewardAccounts.Received)
                    brhStewardAccounts.IsFinish = true;

                brhStewardAccounts.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

                _context.Add(brhStewardAccounts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Branch/Views/BrhStewardAccount/Create.cshtml",brhStewardModel);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhStewardAccounts = await _context.BrhStewardAccounts.SingleOrDefaultAsync(m => m.StewardAccountsId == id);
            if (brhStewardAccounts == null)
            {
                return NotFound();
            }

            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);

            var belongToId = _identityContext.UserBelongTo.SingleOrDefault(x => x.BelongToName == _user.BelongTo).BelongToId;

            return PartialView("~/Areas/Branch/Views/BrhStewardAccount/Edit.cshtml", brhStewardAccounts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("StewardAccountsId,EnteringDate,HouseNumber,CustomerName,ProductType,Product,Cost,Amount,Profit,Receivable,Received,IsFinish,EnteringStaff,FrontDesk,FrontDeskLeader,StewardLeader,RelationStaff,IsSteward,IsFinance,Branch,Note")] BrhStewardAccounts brhStewardAccounts)
        {
            if (id != brhStewardAccounts.StewardAccountsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (brhStewardAccounts.Receivable == brhStewardAccounts.Received)
                        brhStewardAccounts.IsFinish = true;

                    brhStewardAccounts.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

                    _context.Update(brhStewardAccounts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrhStewardAccountsExists(brhStewardAccounts.StewardAccountsId))
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
            return PartialView("~/Areas/Branch/Views/BrhStewardAccount/Edit.cshtml", brhStewardAccounts);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brhStewardAccounts = await _context.BrhStewardAccounts
                .SingleOrDefaultAsync(m => m.StewardAccountsId == id);
            if (brhStewardAccounts == null)
            {
                return NotFound();
            }

            return PartialView("~/Areas/Branch/Views/BrhStewardAccount/Delete.cshtml", "这条记录");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long? id, IFormCollection form)
        {
            var brhStewardAccounts = await _context.BrhStewardAccounts.SingleOrDefaultAsync(m => m.StewardAccountsId == id);
            _context.BrhStewardAccounts.Remove(brhStewardAccounts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddDetial(long? id)
        {
            ViewData["StewardAccountsId"] = id;
            var list_paymentType = _context.FncPaymentType.ToList();
            ViewData["PaymentType"] = new SelectList(list_paymentType, "PaymentType", "PaymentType");

            return PartialView("~/Areas/Branch/Views/BrhStewardAccount/AddDetial.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetial([Bind("StewardPaymentDetialId,StewardAccountsId,PayDate,PayWay,PayAmount")] BrhStewardPaymentDetial brhStewardPaymentDetial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brhStewardPaymentDetial);
                await _context.SaveChangesAsync();

                var total = _context.BrhStewardPaymentDetial.Where(x => x.StewardAccountsId == brhStewardPaymentDetial.StewardAccountsId).Sum(x => x.PayAmount);
                var brhaccount = _context.BrhStewardAccounts.SingleOrDefault(x => x.StewardAccountsId == brhStewardPaymentDetial.StewardAccountsId);
                brhaccount.Received = total;
                if (brhaccount.Received == brhaccount.Receivable)
                    brhaccount.IsFinish = true;
                else
                    brhaccount.IsFinish = false;

                _context.Update(brhaccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StewardAccountsId"] = brhStewardPaymentDetial.StewardAccountsId;
            return PartialView("~/Areas/Branch/Views/BrhStewardAccount/AddDetial.cshtml", brhStewardPaymentDetial);
        }

        public IActionResult DetialList(long? id)
        {
            var brhStewardPaymentDetials = _context.BrhStewardPaymentDetial.Where(x => x.StewardAccountsId == id).ToList();
            return PartialView("~/Areas/Branch/Views/BrhStewardAccount/DetialList.cshtml", brhStewardPaymentDetials);
        }

        private bool BrhStewardAccountsExists(long id)
        {
            return _context.BrhStewardAccounts.Any(e => e.StewardAccountsId == id);
        }
    }
}
