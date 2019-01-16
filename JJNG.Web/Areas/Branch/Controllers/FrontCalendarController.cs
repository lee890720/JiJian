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
    [Authorize(Roles = "Admins,前台")]
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

        public async Task<ActionResult> Index2()
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
            //return Json(new { frontdata, branchdata });
            return View(Tuple.Create<List<string>,List<BrhFrontDeskAccounts>,AppIdentityUser>(houseNumbers,frontdata,_user));
        }

        public async Task<JsonResult> Create([FromBody]BrhFrontModel brhFrontModel)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);

            var belongToId = _identityContext.UserBelongTo.SingleOrDefault(x => x.BelongToName == _user.BelongTo).BelongToId;
            var frontId = Convert.ToInt64(belongToId.ToString() + ConvertJson.DateTimeToStamp(DateTime.Now).ToString());
            brhFrontModel.FrontDeskAccountsId = frontId;
            BrhFrontDeskAccounts brhFrontDeskAccounts = new BrhFrontDeskAccounts();
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
            }
            else
                bfp2.PayAmount = 0;

            if (brhFrontModel.PayAmount3 != 0)
            {
                bfp3.FrontDeskAccountsId = brhFrontModel.FrontDeskAccountsId;
                bfp3.PayWay = brhFrontModel.PayWay3;
                bfp3.PayDate = brhFrontModel.PayDate3;
                bfp3.PayAmount = brhFrontModel.PayAmount3;
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
            brhFrontDeskAccounts.Received = bfp1.PayAmount + bfp2.PayAmount + bfp3.PayAmount;
            brhFrontDeskAccounts.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
            if (brhFrontDeskAccounts.Receivable == brhFrontDeskAccounts.Received)
                brhFrontDeskAccounts.IsFinish = true;
            _context.Add(brhFrontDeskAccounts);
            await _context.SaveChangesAsync();

            return Json(new { brhFrontDeskAccounts });
        }

        public async Task<JsonResult> CreateDetial([FromBody]BrhFrontModel brhFrontModel)
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

            if (brhFrontModel.PayAmount2 != 0)
            {
                bfp2.FrontDeskAccountsId = brhFrontModel.FrontDeskAccountsId;
                bfp2.PayWay = brhFrontModel.PayWay2;
                bfp2.PayDate = brhFrontModel.PayDate2;
                bfp2.PayAmount = brhFrontModel.PayAmount2;
                _context.Add(bfp2);
            }

            if (brhFrontModel.PayAmount3 != 0)
            {
                bfp3.FrontDeskAccountsId = brhFrontModel.FrontDeskAccountsId;
                bfp3.PayWay = brhFrontModel.PayWay3;
                bfp3.PayDate = brhFrontModel.PayDate3;
                bfp3.PayAmount = brhFrontModel.PayAmount3;
                _context.Add(bfp3);
            }

            await _context.SaveChangesAsync();

            return Json(new { bfp1, bfp2, bfp3 });
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

            return View(Tuple.Create<AppIdentityUser,List<FncChannelType>>(_user,channel));
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
            return Json(new { frontdata,branchdata});
        }
    }
}
