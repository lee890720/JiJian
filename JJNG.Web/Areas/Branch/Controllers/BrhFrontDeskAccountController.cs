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
            ViewData["Branch"] = _user.Branch;
            //var channel = _context.FncChannelType.ToList();
            //var front = _context.BrhFrontDeskAccounts.ToList();
            //foreach (var c in channel)
            //{
            //    foreach (var f in front)
            //    {
            //        if (c.ChannelType == f.Channel)
            //            f.Color = c.Color;
            //        _context.Update(f);
            //    }
            //}
            //await _context.SaveChangesAsync();

            return View(await _context.BrhFrontDeskAccounts.Where(x => x.Branch == _user.Branch).ToListAsync());
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
            ViewData["Branch"] = _user.Branch;

            var belongToId = _identityContext.UserBranch.SingleOrDefault(x => x.BranchName == _user.Branch).BranchId;
            var list_houseNumber = _identityContext.UserBranchDetial.Where(x => x.BranchId == belongToId).ToList();
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
        public async Task<IActionResult> Create([Bind("FrontDeskAccountsId,EnteringDate,HouseNumber,CustomerName,CustomerCount,StartDate,EndDate,Channel,UnitPrice,TotalPrice,Receivable,Received,IsFinish,EnteringStaff,Steward,FrontDeskLeader,StewardLeader,RelationStaff,IsFront,IsFinance,Branch,Note,PayWay1,PayDate1,PayAmount1,PayWay2,PayDate2,PayAmount2,PayWay3,PayDate3,PayAmount3")]BrhFrontModel brhFrontModel)
        {
            if (ModelState.IsValid)
            {
                BrhFrontPaymentDetial bfp1 = new BrhFrontPaymentDetial();
                BrhFrontPaymentDetial bfp2 = new BrhFrontPaymentDetial();
                BrhFrontPaymentDetial bfp3 = new BrhFrontPaymentDetial();

                if (brhFrontModel.PayAmount1 != 0)
                {
                    bfp1.FrontDeskAccountsId = brhFrontModel.FrontDeskAccountsId;
                    bfp1.PayWay = brhFrontModel.PayWay1;
                    bfp1.PayDate = brhFrontModel.PayDate1;
                    bfp1.PayAmount = brhFrontModel.PayAmount1;
                    _context.Add(bfp1);
                }
                else
                    bfp1.PayAmount = 0;

                if (brhFrontModel.PayAmount2 != 0)
                {
                    bfp2.FrontDeskAccountsId = brhFrontModel.FrontDeskAccountsId;
                    bfp2.PayWay = brhFrontModel.PayWay2;
                    bfp2.PayDate = brhFrontModel.PayDate2;
                    bfp2.PayAmount = brhFrontModel.PayAmount2;
                    _context.Add(bfp2);
                }
                else
                    bfp2.PayAmount = 0;

                if (brhFrontModel.PayAmount3 != 0)
                {
                    bfp3.FrontDeskAccountsId = brhFrontModel.FrontDeskAccountsId;
                    bfp3.PayWay = brhFrontModel.PayWay3;
                    bfp3.PayDate = brhFrontModel.PayDate3;
                    bfp3.PayAmount = brhFrontModel.PayAmount3;
                    _context.Add(bfp3);
                }
                else
                    bfp3.PayAmount = 0;

                BrhFrontDeskAccounts brhFrontDeskAccounts = new BrhFrontDeskAccounts();
                brhFrontDeskAccounts.HouseNumber = brhFrontModel.HouseNumber;
                brhFrontDeskAccounts.Branch = brhFrontModel.Branch;
                brhFrontDeskAccounts.Channel = brhFrontModel.Channel;
                brhFrontDeskAccounts.CustomerName = brhFrontModel.CustomerName;
                brhFrontDeskAccounts.CustomerCount = brhFrontModel.CustomerCount;
                brhFrontDeskAccounts.EndDate = brhFrontModel.EndDate;
                brhFrontDeskAccounts.EnteringStaff = brhFrontModel.EnteringStaff;
                brhFrontDeskAccounts.FrontDeskAccountsId = brhFrontModel.FrontDeskAccountsId;
                brhFrontDeskAccounts.FrontDeskLeader = brhFrontModel.FrontDeskLeader;
                brhFrontDeskAccounts.HouseNumber = brhFrontModel.HouseNumber;
                brhFrontDeskAccounts.Note = brhFrontModel.Note;
                brhFrontDeskAccounts.Receivable = brhFrontModel.Receivable;
                brhFrontDeskAccounts.RelationStaff = brhFrontModel.RelationStaff;
                brhFrontDeskAccounts.StartDate = brhFrontModel.StartDate;
                brhFrontDeskAccounts.Steward = brhFrontModel.Steward;
                brhFrontDeskAccounts.StewardLeader = brhFrontModel.StewardLeader;
                brhFrontDeskAccounts.TotalPrice = brhFrontModel.TotalPrice;
                brhFrontDeskAccounts.UnitPrice = brhFrontModel.UnitPrice;

                brhFrontDeskAccounts.Received = bfp1.PayAmount + bfp2.PayAmount + bfp3.PayAmount;

                if (brhFrontDeskAccounts.Receivable == brhFrontDeskAccounts.Received)
                    brhFrontDeskAccounts.IsFinish = true;

                brhFrontDeskAccounts.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));

                var channel = _context.FncChannelType.ToList();
                foreach (var c in channel)
                {
                    if (c.ChannelType == brhFrontDeskAccounts.Channel)
                    {
                        brhFrontDeskAccounts.Color = c.Color;
                        break;
                    }
                }

                _context.Add(brhFrontDeskAccounts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView("~/Areas/Branch/Views/BrhFrontDeskAccount/Create.cshtml", brhFrontModel);
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

            var belongToId = _identityContext.UserBranch.SingleOrDefault(x => x.BranchName == _user.Branch).BranchId;
            var list_houseNumber = _identityContext.UserBranchDetial.Where(x => x.BranchId == belongToId).ToList();
            ViewData["HouseNumber"] = new SelectList(list_houseNumber, "HouseNumber", "HouseNumber", brhFrontDeskAccounts.HouseNumber);

            var list_channelType = _context.FncChannelType.ToList();
            ViewData["ChannelType"] = new SelectList(list_channelType, "ChannelType", "ChannelType", brhFrontDeskAccounts.Channel);

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

                    //brhFrontDeskAccounts.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                    var channel = _context.FncChannelType.ToList();
                    foreach (var c in channel)
                    {
                        if (c.ChannelType == brhFrontDeskAccounts.Channel)
                        {
                            brhFrontDeskAccounts.Color = c.Color;
                            break;
                        }
                    }

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
                brhFrontPaymentDetial.PayAmount = brhFrontPaymentDetial.PayAmount;
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
