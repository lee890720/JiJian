using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
using JJNG.Data.Finance;
using JJNG.Web.Models;
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
    [Authorize(Roles = "Admins,前台,管家")]
    public class FrontCalendarController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

       public FrontCalendarController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<ActionResult> Index()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var belongToId = _identityContext.UserBelongTo.SingleOrDefault(x => x.BelongToName == _user.BelongTo).BelongToId;
            var list_houseNumber = _identityContext.UserBelongToDetial.Where(x => x.BelongToId == belongToId).ToList();
            ViewData["HouseNumber"] = new SelectList(list_houseNumber, "HouseNumber", "HouseNumber");

            var list_paymentType = _context.FncPaymentType.ToList();
            ViewData["PaymentType"] = new SelectList(list_paymentType, "PaymentType", "PaymentType");
            var list_channelType = _context.FncChannelType.ToList();
            ViewData["ChannelType"] = new SelectList(list_channelType, "ChannelType", "ChannelType");

            var channel = _context.FncChannelType.ToList();

            var d_list = _context.BrhFrontPaymentDetials2.ToList();
            if (d_list.Count > 0)
                foreach (var d in d_list)
                {
                    var test = _context.BrhFrontDeskAccounts.Where(x => x.FrontDeskAccountsId == d.FrontDeskAccountsId).Count();
                    if (test > 0)
                    {
                        BrhFrontPaymentDetial brf = new BrhFrontPaymentDetial();
                        brf.FrontDeskAccountsId = d.FrontDeskAccountsId;
                        brf.PayAmount = d.PayAmount;
                        brf.PayDate = d.PayDate;
                        brf.PayWay = d.PayWay;
                        _context.Add(brf);
                    }
                }
            _context.RemoveRange(d_list);
            await _context.SaveChangesAsync();

            return View(Tuple.Create<AppIdentityUser, List<FncChannelType>>(_user, channel));
        }

        public async Task<JsonResult> Create([FromBody]BrhFrontModel brhFrontModel)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);

            var belongToId = _identityContext.UserBelongTo.SingleOrDefault(x => x.BelongToName == _user.BelongTo).BelongToId;
            var frontId = Convert.ToInt64(belongToId.ToString() + ConvertJson.DateTimeToStamp(DateTime.Now).ToString());
            brhFrontModel.FrontDeskAccountsId = frontId;
            BrhFrontDeskAccounts brhFrontDeskAccounts = new BrhFrontDeskAccounts();
            BrhFrontPaymentDetial2 bfp1 = new BrhFrontPaymentDetial2();
            BrhFrontPaymentDetial2 bfp2 = new BrhFrontPaymentDetial2();
            BrhFrontPaymentDetial2 bfp3 = new BrhFrontPaymentDetial2();

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

            brhFrontDeskAccounts.FrontDeskAccountsId = brhFrontModel.FrontDeskAccountsId;
            brhFrontDeskAccounts.HouseNumber = brhFrontModel.HouseNumber;
            brhFrontDeskAccounts.Branch = brhFrontModel.Branch;
            brhFrontDeskAccounts.Channel = brhFrontModel.Channel;
            brhFrontDeskAccounts.CustomerName = brhFrontModel.CustomerName;
            brhFrontDeskAccounts.CustomerCount = brhFrontModel.CustomerCount;
            brhFrontDeskAccounts.EndDate = brhFrontModel.EndDate;
            brhFrontDeskAccounts.EnteringStaff = brhFrontModel.EnteringStaff;
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
            brhFrontDeskAccounts.Color = brhFrontModel.Color;
            brhFrontDeskAccounts.IsFront = brhFrontModel.IsFront;
            brhFrontDeskAccounts.IsFinance = brhFrontModel.IsFinance;
            brhFrontDeskAccounts.Received = bfp1.PayAmount + bfp2.PayAmount + bfp3.PayAmount;
            brhFrontDeskAccounts.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
            if (brhFrontDeskAccounts.Receivable == brhFrontDeskAccounts.Received)
                brhFrontDeskAccounts.IsFinish = true;
            _context.Add(brhFrontDeskAccounts);
            _context.SaveChanges();

            return Json(new { brhFrontDeskAccounts });
        }

        public JsonResult Edit([FromBody]BrhFrontModel brhFrontModel)
        {
            BrhFrontDeskAccounts brhFrontDeskAccounts = new BrhFrontDeskAccounts();
            BrhFrontPaymentDetial2 bfp1 = new BrhFrontPaymentDetial2();
            BrhFrontPaymentDetial2 bfp2 = new BrhFrontPaymentDetial2();
            BrhFrontPaymentDetial2 bfp3 = new BrhFrontPaymentDetial2();
            var rtotal = _context.BrhFrontPaymentDetials.Where(x => x.FrontDeskAccountsId == brhFrontModel.FrontDeskAccountsId).Sum(x => x.PayAmount);
            rtotal += _context.BrhFrontPaymentDetials2.Where(x => x.FrontDeskAccountsId == brhFrontModel.FrontDeskAccountsId).Sum(x => x.PayAmount);
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

            brhFrontDeskAccounts.FrontDeskAccountsId = brhFrontModel.FrontDeskAccountsId;
            brhFrontDeskAccounts.HouseNumber = brhFrontModel.HouseNumber;
            brhFrontDeskAccounts.Branch = brhFrontModel.Branch;
            brhFrontDeskAccounts.Channel = brhFrontModel.Channel;
            brhFrontDeskAccounts.CustomerName = brhFrontModel.CustomerName;
            brhFrontDeskAccounts.CustomerCount = brhFrontModel.CustomerCount;
            brhFrontDeskAccounts.EndDate = brhFrontModel.EndDate;
            brhFrontDeskAccounts.EnteringStaff = brhFrontModel.EnteringStaff;
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
            brhFrontDeskAccounts.Color = brhFrontModel.Color;
            brhFrontDeskAccounts.IsFront = brhFrontModel.IsFront;
            brhFrontDeskAccounts.IsFinance = brhFrontModel.IsFinance;
            brhFrontDeskAccounts.Received =rtotal+ bfp1.PayAmount + bfp2.PayAmount + bfp3.PayAmount;
            brhFrontDeskAccounts.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
            if (brhFrontDeskAccounts.Receivable == brhFrontDeskAccounts.Received)
                brhFrontDeskAccounts.IsFinish = true;
            _context.Update(brhFrontDeskAccounts);
            _context.SaveChanges();

            return Json(new { brhFrontDeskAccounts });
        }

        public async Task<JsonResult> Delete([FromBody]BrhFrontModel brhFrontModel)
        {
            var brhFrontDeskAccounts = await _context.BrhFrontDeskAccounts.SingleOrDefaultAsync(m => m.FrontDeskAccountsId == brhFrontModel.FrontDeskAccountsId);
            _context.BrhFrontDeskAccounts.Remove(brhFrontDeskAccounts);
            await _context.SaveChangesAsync();
            return Json(new { brhFrontDeskAccounts });
        }

        public JsonResult List([FromBody]BrhFrontModel brhFrontModel)
        {
            var list1 = _context.BrhFrontPaymentDetials.Where(x => x.FrontDeskAccountsId == brhFrontModel.FrontDeskAccountsId).ToList();
            var list2= _context.BrhFrontPaymentDetials2.Where(x => x.FrontDeskAccountsId == brhFrontModel.FrontDeskAccountsId).ToList();
            return Json(new { list1,list2 });
        }

        public async Task<JsonResult> Drop([FromBody]BrhFrontModel brhFrontModel)
        {
            var brhFrontDeskAccounts = await _context.BrhFrontDeskAccounts.SingleOrDefaultAsync(m => m.FrontDeskAccountsId == brhFrontModel.FrontDeskAccountsId);
            brhFrontDeskAccounts.HouseNumber = brhFrontModel.HouseNumber;
            brhFrontDeskAccounts.StartDate = brhFrontModel.StartDate;
            brhFrontDeskAccounts.EndDate = brhFrontModel.EndDate;
            _context.Update(brhFrontDeskAccounts);
            await _context.SaveChangesAsync();
            return Json(new { brhFrontDeskAccounts });
        }

        public async Task<JsonResult> Resize([FromBody]BrhFrontModel brhFrontModel)
        {
            var brhFrontDeskAccounts = await _context.BrhFrontDeskAccounts.SingleOrDefaultAsync(m => m.FrontDeskAccountsId == brhFrontModel.FrontDeskAccountsId);
            brhFrontDeskAccounts.StartDate = brhFrontModel.StartDate;
            brhFrontDeskAccounts.EndDate = brhFrontModel.EndDate;

            TimeSpan sp = brhFrontDeskAccounts.EndDate.Subtract(brhFrontDeskAccounts.StartDate);
            int days = sp.Days;
            brhFrontDeskAccounts.TotalPrice = brhFrontDeskAccounts.UnitPrice*days;
            if (brhFrontDeskAccounts.Receivable != 0)
                brhFrontDeskAccounts.Receivable = brhFrontDeskAccounts.TotalPrice;
            if (brhFrontDeskAccounts.Receivable == brhFrontDeskAccounts.Received)
                brhFrontDeskAccounts.IsFinish = true;
            else
                brhFrontDeskAccounts.IsFinish = false;
            _context.Update(brhFrontDeskAccounts);
            await _context.SaveChangesAsync();
            return Json(new { brhFrontDeskAccounts });
        }
        public async Task<JsonResult> GetCalendarData()
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var branch = _user.BelongTo;
            var frontdata = _context.BrhFrontDeskAccounts.Where(x => x.Branch == branch).ToList();
            var list_branch = _identityContext.UserBelongToDetial.Include(x => x.UserBelongTo).GroupBy(x => new
            {
                x.BelongToId,
                x.UserBelongTo.BelongToName,
                x.HouseNumber,
            }).Select(x => new
            {
                BranchId = x.Key.BelongToId,
                Branch = x.Key.BelongToName,
                HouseNumber = x.Key.HouseNumber,
            }).ToList();

            var houseNumbers = list_branch.Where(x => x.Branch == branch).Select(x => x.HouseNumber).ToList();


            var branchdata = houseNumbers;
            return Json(new { frontdata, branchdata });
        }
    }
}
