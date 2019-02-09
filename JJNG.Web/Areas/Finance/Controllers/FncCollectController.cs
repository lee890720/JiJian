using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JJNG.Data;
using JJNG.Data.Branch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using JJNG.Data.AppIdentity;
using Microsoft.AspNetCore.Identity;
using JJNG.Web;
using JJNG.Web.Models;
using JJNG.Web.Areas.Branch.Models;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,财务")]
    public class FncCollectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public FncCollectController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
            _userManager = usrMgr;
        }

        public IActionResult Index(string branch)
        {
            //AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var list_branch = _identityContext.UserBranch.Where(x => x.BranchName != "运营中心"&& x.BranchName != "町隐学院").ToList();
            var _params = new Params();
            _params.Branch = branch;
            return View(Tuple.Create<Params, List<UserBranch>>(_params, list_branch));

        }

        public JsonResult GetGroup([FromBody]Params _params)
        {
            var branch = _params.Branch;
            List<BrhGroupModel> brhGroup = new List<BrhGroupModel>();
            List<BrhGroupModel> brhGroupY = new List<BrhGroupModel>();
            List<BrhCollectModel> brhCollectModel = new List<BrhCollectModel>();
            var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
            if (string.IsNullOrEmpty(branch))
            {
                #region BrhCollect
                var typename = "";
                decimal amount = 0.00M;
                var count = 0;
                var year = now.Year;
                var month = now.Month;
                var newdate = new DateTime(year, month, 1);
                var templist1 = _context.BrhEarningRecord.Where(x => DateTime.Compare(x.EnteringDate, newdate) >= 0).ToList();
                typename = "Earning_M";
                amount = (decimal)templist1.Sum(x => x.Amount);
                count = templist1.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
                var templist1d = _context.BrhEarningRecord.Where(x => x.EnteringDate.Date == DateTime.Now.Date).ToList();
                typename = "Earning_D";
                amount = (decimal)templist1d.Sum(x => x.Amount);
                count = templist1d.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

                var templist2 = _context.BrhExpendRecord.Where(x => DateTime.Compare(x.EnteringDate, newdate) >= 0).ToList();
                typename = "Expend_M";
                amount = (decimal)templist2.Sum(x => x.Amount);
                count = templist2.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
                var templist2d = _context.BrhExpendRecord.Where(x => x.EnteringDate.Date == DateTime.Now.Date).ToList();
                typename = "Expend_D";
                amount = (decimal)templist2d.Sum(x => x.Amount);
                count = templist2d.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

                var templist3 = _context.BrhImprestRecord.Include(x => x.BrhImprestAccounts).Where(x => !x.IsFinance && string.IsNullOrEmpty(x.BrhImprestAccounts.Manager)).ToList();
                typename = "Imprest";
                amount = (decimal)templist3.Sum(x => x.Amount);
                count = templist3.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

                var templist4 = _context.BrhFrontDeskAccounts.Where(x => DateTime.Compare(x.StartDate, newdate) >= 0).ToList();
                var templist4d = _context.BrhFrontDeskAccounts.Where(x => DateTime.Compare(x.StartDate, DateTime.Now.Date) <= 0 && DateTime.Compare(x.EndDate, DateTime.Now.Date) > 0).ToList();
                var houselist = _identityContext.UserBranchDetial.ToList();
                var housenum = houselist.Count;
                var housetotal = housenum * DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                decimal day_rate = 0;
                decimal month_rate = 0;
                if (housenum != 0)
                    day_rate = (decimal)templist4d.Count / (decimal)housenum * 100;
                if (housetotal != 0)
                    month_rate = (decimal)templist4.Count / (decimal)housetotal * 100;
                typename = "MonthRate";
                amount = Math.Round(month_rate, 2);
                count = 0;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
                typename = "DayRate";
                amount = Math.Round(day_rate, 2);
                count = 0;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

                var templist5 = _context.BrhStewardAccounts.Where(x => DateTime.Compare(x.EnteringDate, newdate) >= 0).ToList();
                typename = "Steward_M";
                amount = templist5.Sum(x => x.Receivable);
                count = templist5.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
                var templist5d = _context.BrhStewardAccounts.Where(x => DateTime.Compare(x.EnteringDate.Date, DateTime.Now.Date) == 0).ToList();
                typename = "Steward_D";
                amount = templist5d.Sum(x => x.Receivable);
                count = templist5d.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

                var templist6 = _context.BrhFrontDeskAccounts.Where(x => DateTime.Compare(x.StartDate, newdate) >= 0).ToList();
                typename = "Front_M";
                amount = templist6.Sum(x => x.TotalPrice);
                count = templist6.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
                var templist6d = _context.BrhFrontDeskAccounts.Where(x => DateTime.Compare(x.StartDate, DateTime.Now.Date) <= 0 && DateTime.Compare(x.EndDate, DateTime.Now.Date) > 0).ToList();
                typename = "Front_D";
                amount = templist6d.Sum(x => x.UnitPrice);
                count = templist6d.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

                var templist7 = _context.BrhFrontDeskAccounts.Where(x => !x.IsFinish || x.UnitPrice == 0).ToList();
                typename = "FrontIsFinish";
                amount = templist7.Sum(x => x.Receivable);
                count = templist7.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
                #endregion

                #region 今日收入
                var nowdate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time")).Date;
                var frontDetials = _context.BrhFrontPaymentDetials.Include(x => x.BrhFrontDeskAccounts).Where(x => x.PayDate.Date == nowdate).ToList();
                var frontGroup = frontDetials.GroupBy(x => new { x.PayWay }).Select(s => new
                {
                    PayWay = s.Key.PayWay,
                    FrontAmount = s.Sum(x => x.PayAmount),
                }).ToList();
                var stewardDetials = _context.BrhStewardPaymentDetial.Include(x => x.BrhStewardAccounts).Where(x => x.PayDate.Date == nowdate).ToList();
                var stewardGroup = stewardDetials.GroupBy(x => new { x.PayWay }).Select(s => new
                {
                    PayWay = s.Key.PayWay,
                    StewardAmount = s.Sum(x => x.PayAmount),
                }).ToList();
                var earningDetials = _context.BrhEarningRecord.Where(x => x.EnteringDate.Date == nowdate).ToList();
                var earningGroup = earningDetials.GroupBy(x => new { x.PaymentType }).Select(s => new
                {
                    PayWay = s.Key.PaymentType,
                    EarningAmount = s.Sum(x => x.Amount),
                }).ToList();
                foreach (var p in _context.FncPaymentType.ToList())
                {
                    var isAdd = false;
                    var brhG = new BrhGroupModel();
                    if (frontGroup.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                    {
                        var frontG = frontGroup.SingleOrDefault(x => x.PayWay == p.PaymentType);
                        brhG.PayWay = p.PaymentType;
                        brhG.FrontAmount = frontG.FrontAmount;
                        isAdd = true;
                    }
                    if (stewardGroup.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                    {
                        var stewardG = stewardGroup.SingleOrDefault(x => x.PayWay == p.PaymentType);
                        brhG.PayWay = p.PaymentType;
                        brhG.StewardAmount = stewardG.StewardAmount;
                        isAdd = true;
                    }
                    if (earningGroup.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                    {
                        var earningG = earningGroup.SingleOrDefault(x => x.PayWay == p.PaymentType);
                        brhG.PayWay = p.PaymentType;
                        brhG.EarningAmount = earningG.EarningAmount;
                        isAdd = true;
                    }
                    if (isAdd)
                    {
                        brhG.Total = brhG.FrontAmount + brhG.StewardAmount + brhG.EarningAmount;
                        brhGroup.Add(brhG);
                    }
                }
                #endregion

                #region 昨日收入
                var nowdateY = TimeZoneInfo.ConvertTime(DateTime.Now.AddDays(-1), TimeZoneInfo.FindSystemTimeZoneById("China Standard Time")).Date;
                var frontDetialsY = _context.BrhFrontPaymentDetials.Include(x => x.BrhFrontDeskAccounts).Where(x => x.PayDate.Date == nowdateY).ToList();
                var frontGroupY = frontDetialsY.GroupBy(x => new { x.PayWay }).Select(s => new
                {
                    PayWay = s.Key.PayWay,
                    FrontAmount = s.Sum(x => x.PayAmount),
                }).ToList();
                var stewardDetialsY = _context.BrhStewardPaymentDetial.Include(x => x.BrhStewardAccounts).Where(x => x.PayDate.Date == nowdateY).ToList();
                var stewardGroupY = stewardDetialsY.GroupBy(x => new { x.PayWay }).Select(s => new
                {
                    PayWay = s.Key.PayWay,
                    StewardAmount = s.Sum(x => x.PayAmount),
                }).ToList();
                var earningDetialsY = _context.BrhEarningRecord.Where(x => x.EnteringDate.Date == nowdateY).ToList();
                var earningGroupY = earningDetialsY.GroupBy(x => new { x.PaymentType }).Select(s => new
                {
                    PayWay = s.Key.PaymentType,
                    EarningAmount = s.Sum(x => x.Amount),
                }).ToList();
                foreach (var p in _context.FncPaymentType.ToList())
                {
                    var isAdd = false;
                    var brhG = new BrhGroupModel();
                    if (frontGroupY.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                    {
                        var frontG = frontGroupY.SingleOrDefault(x => x.PayWay == p.PaymentType);
                        brhG.PayWay = p.PaymentType;
                        brhG.FrontAmount = frontG.FrontAmount;
                        isAdd = true;
                    }
                    if (stewardGroupY.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                    {
                        var stewardG = stewardGroupY.SingleOrDefault(x => x.PayWay == p.PaymentType);
                        brhG.PayWay = p.PaymentType;
                        brhG.StewardAmount = stewardG.StewardAmount;
                        isAdd = true;
                    }
                    if (earningGroupY.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                    {
                        var earningG = earningGroupY.SingleOrDefault(x => x.PayWay == p.PaymentType);
                        brhG.PayWay = p.PaymentType;
                        brhG.EarningAmount = earningG.EarningAmount;
                        isAdd = true;
                    }
                    if (isAdd)
                    {
                        brhG.Total = brhG.FrontAmount + brhG.StewardAmount + brhG.EarningAmount;
                        brhGroupY.Add(brhG);
                    }
                }
                #endregion
            }
            else
            {
                #region BrhCollect
                var typename = "";
                decimal amount = 0.00M;
                var count = 0;
                var year = now.Year;
                var month = now.Month;
                var newdate = new DateTime(year, month, 1);
                var templist1 = _context.BrhEarningRecord.Where(x => x.Branch == branch && DateTime.Compare(x.EnteringDate, newdate) >= 0).ToList();
                typename = "Earning_M";
                amount = (decimal)templist1.Sum(x => x.Amount);
                count = templist1.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
                var templist1d = _context.BrhEarningRecord.Where(x => x.Branch == branch && x.EnteringDate.Date == DateTime.Now.Date).ToList();
                typename = "Earning_D";
                amount = (decimal)templist1d.Sum(x => x.Amount);
                count = templist1d.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

                var templist2 = _context.BrhExpendRecord.Where(x => x.Branch == branch && DateTime.Compare(x.EnteringDate, newdate) >= 0).ToList();
                typename = "Expend_M";
                amount = (decimal)templist2.Sum(x => x.Amount);
                count = templist2.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
                var templist2d = _context.BrhExpendRecord.Where(x => x.Branch == branch && x.EnteringDate.Date == DateTime.Now.Date).ToList();
                typename = "Expend_D";
                amount = (decimal)templist2d.Sum(x => x.Amount);
                count = templist2d.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

                var templist3 = _context.BrhImprestRecord.Include(x => x.BrhImprestAccounts).Where(x => x.Branch == branch && !x.IsFinance && string.IsNullOrEmpty(x.BrhImprestAccounts.Manager)).ToList();
                typename = "Imprest";
                amount = (decimal)templist3.Sum(x => x.Amount);
                count = templist3.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

                var templist4 = _context.BrhFrontDeskAccounts.Where(x => x.Branch == branch && DateTime.Compare(x.StartDate, newdate) >= 0).ToList();
                var templist4d = _context.BrhFrontDeskAccounts.Where(x => x.Branch == branch && DateTime.Compare(x.StartDate, DateTime.Now.Date) <= 0 && DateTime.Compare(x.EndDate, DateTime.Now.Date) > 0).ToList();
                var houselist = _identityContext.UserBranchDetial.Where(x => x.UserBranch.BranchName == branch).ToList();
                var housenum = houselist.Count;
                var housetotal = housenum * DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                decimal day_rate = 0;
                decimal month_rate = 0;
                if (housenum != 0)
                    day_rate = (decimal)templist4d.Count / (decimal)housenum * 100;
                if (housetotal != 0)
                    month_rate = (decimal)templist4.Count / (decimal)housetotal * 100;
                typename = "MonthRate";
                amount = Math.Round(month_rate, 2);
                count = 0;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
                typename = "DayRate";
                amount = Math.Round(day_rate, 2);
                count = 0;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

                var templist5 = _context.BrhStewardAccounts.Where(x => x.Branch == branch && DateTime.Compare(x.EnteringDate, newdate) >= 0).ToList();
                typename = "Steward_M";
                amount = templist5.Sum(x => x.Receivable);
                count = templist5.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
                var templist5d = _context.BrhStewardAccounts.Where(x => x.Branch == branch && DateTime.Compare(x.EnteringDate.Date, DateTime.Now.Date) == 0).ToList();
                typename = "Steward_D";
                amount = templist5d.Sum(x => x.Receivable);
                count = templist5d.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

                var templist6 = _context.BrhFrontDeskAccounts.Where(x => x.Branch == branch && DateTime.Compare(x.StartDate, newdate) >= 0).ToList();
                typename = "Front_M";
                amount = templist6.Sum(x => x.TotalPrice);
                count = templist6.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
                var templist6d = _context.BrhFrontDeskAccounts.Where(x => x.Branch == branch && DateTime.Compare(x.StartDate, DateTime.Now.Date) <= 0 && DateTime.Compare(x.EndDate, DateTime.Now.Date) > 0).ToList();
                typename = "Front_D";
                amount = templist6d.Sum(x => x.UnitPrice);
                count = templist6d.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });

                var templist7 = _context.BrhFrontDeskAccounts.Where(x => !x.IsFinish || x.UnitPrice == 0).ToList();
                typename = "FrontIsFinish";
                amount = templist7.Sum(x => x.Receivable);
                count = templist7.Count;
                brhCollectModel.Add(new BrhCollectModel { Type = typename, Amount = amount, Count = count });
                #endregion

                #region 今日收入
                var nowdate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time")).Date;
                var frontDetials = _context.BrhFrontPaymentDetials.Include(x => x.BrhFrontDeskAccounts).Where(x => x.BrhFrontDeskAccounts.Branch == branch && x.PayDate.Date == nowdate).ToList();
                var frontGroup = frontDetials.GroupBy(x => new { x.PayWay }).Select(s => new
                {
                    PayWay = s.Key.PayWay,
                    FrontAmount = s.Sum(x => x.PayAmount),
                }).ToList();
                var stewardDetials = _context.BrhStewardPaymentDetial.Include(x => x.BrhStewardAccounts).Where(x => x.BrhStewardAccounts.Branch == branch && x.PayDate.Date == nowdate).ToList();
                var stewardGroup = stewardDetials.GroupBy(x => new { x.PayWay }).Select(s => new
                {
                    PayWay = s.Key.PayWay,
                    StewardAmount = s.Sum(x => x.PayAmount),
                }).ToList();
                var earningDetials = _context.BrhEarningRecord.Where(x => x.Branch == branch && x.EnteringDate.Date == nowdate).ToList();
                var earningGroup = earningDetials.GroupBy(x => new { x.PaymentType }).Select(s => new
                {
                    PayWay = s.Key.PaymentType,
                    EarningAmount = s.Sum(x => x.Amount),
                }).ToList();
                foreach (var p in _context.FncPaymentType.ToList())
                {
                    var isAdd = false;
                    var brhG = new BrhGroupModel();
                    if (frontGroup.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                    {
                        var frontG = frontGroup.SingleOrDefault(x => x.PayWay == p.PaymentType);
                        brhG.PayWay = p.PaymentType;
                        brhG.FrontAmount = frontG.FrontAmount;
                        isAdd = true;
                    }
                    if (stewardGroup.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                    {
                        var stewardG = stewardGroup.SingleOrDefault(x => x.PayWay == p.PaymentType);
                        brhG.PayWay = p.PaymentType;
                        brhG.StewardAmount = stewardG.StewardAmount;
                        isAdd = true;
                    }
                    if (earningGroup.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                    {
                        var earningG = earningGroup.SingleOrDefault(x => x.PayWay == p.PaymentType);
                        brhG.PayWay = p.PaymentType;
                        brhG.EarningAmount = earningG.EarningAmount;
                        isAdd = true;
                    }
                    if (isAdd)
                    {
                        brhG.Total = brhG.FrontAmount + brhG.StewardAmount + brhG.EarningAmount;
                        brhGroup.Add(brhG);
                    }
                }
                #endregion

                #region 昨日收入
                var nowdateY = TimeZoneInfo.ConvertTime(DateTime.Now.AddDays(-1), TimeZoneInfo.FindSystemTimeZoneById("China Standard Time")).Date;
                var frontDetialsY = _context.BrhFrontPaymentDetials.Include(x => x.BrhFrontDeskAccounts).Where(x => x.PayDate.Date == nowdateY).ToList();
                var frontGroupY = frontDetialsY.GroupBy(x => new { x.PayWay }).Select(s => new
                {
                    PayWay = s.Key.PayWay,
                    FrontAmount = s.Sum(x => x.PayAmount),
                }).ToList();
                var stewardDetialsY = _context.BrhStewardPaymentDetial.Include(x => x.BrhStewardAccounts).Where(x => x.PayDate.Date == nowdateY).ToList();
                var stewardGroupY = stewardDetialsY.GroupBy(x => new { x.PayWay }).Select(s => new
                {
                    PayWay = s.Key.PayWay,
                    StewardAmount = s.Sum(x => x.PayAmount),
                }).ToList();
                var earningDetialsY = _context.BrhEarningRecord.Where(x => x.EnteringDate.Date == nowdateY).ToList();
                var earningGroupY = earningDetialsY.GroupBy(x => new { x.PaymentType }).Select(s => new
                {
                    PayWay = s.Key.PaymentType,
                    EarningAmount = s.Sum(x => x.Amount),
                }).ToList();
                foreach (var p in _context.FncPaymentType.ToList())
                {
                    var isAdd = false;
                    var brhG = new BrhGroupModel();
                    if (frontGroupY.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                    {
                        var frontG = frontGroupY.SingleOrDefault(x => x.PayWay == p.PaymentType);
                        brhG.PayWay = p.PaymentType;
                        brhG.FrontAmount = frontG.FrontAmount;
                        isAdd = true;
                    }
                    if (stewardGroupY.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                    {
                        var stewardG = stewardGroupY.SingleOrDefault(x => x.PayWay == p.PaymentType);
                        brhG.PayWay = p.PaymentType;
                        brhG.StewardAmount = stewardG.StewardAmount;
                        isAdd = true;
                    }
                    if (earningGroupY.Where(x => x.PayWay == p.PaymentType).Count() > 0)
                    {
                        var earningG = earningGroupY.SingleOrDefault(x => x.PayWay == p.PaymentType);
                        brhG.PayWay = p.PaymentType;
                        brhG.EarningAmount = earningG.EarningAmount;
                        isAdd = true;
                    }
                    if (isAdd)
                    {
                        brhG.Total = brhG.FrontAmount + brhG.StewardAmount + brhG.EarningAmount;
                        brhGroupY.Add(brhG);
                    }
                }
                #endregion
            }
            return Json(new { brhGroup,brhGroupY, brhCollectModel });
        }
    }
}
