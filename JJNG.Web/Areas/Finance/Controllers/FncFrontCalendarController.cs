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
using Newtonsoft.Json;

namespace JJNG.Web.Areas.Finance.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,财务")]
    public class FncFrontCalendarController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public FncFrontCalendarController(AppDbContext context, AppIdentityDbContext identitycontext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identitycontext;
            _userManager = usrMgr;
        }

        public async Task<ActionResult> Index(string branch = "既见·南国", int branchId = 2)
        {
            #region 前台账目去重
            //var templist = _context.BrhFrontDeskAccounts.ToList();
            //var grouplist=templist.Where((x, i) => templist.FindIndex(z => z.HouseNumber == x.HouseNumber&&z.StartDate==x.StartDate&&z.EndDate==x.EndDate&&z.State==x.State) == i).ToList();
            //foreach(var t in templist)
            //{
            //    if(grouplist.Find(x=>x.FrontDeskAccountsId==t.FrontDeskAccountsId)==null)
            //    {
            //        _context.Remove(t);
            //        _context.SaveChanges();
            //    }
            //}
            #endregion

            #region 云掌柜导入后台
            //var templist = _context.BrhYun.ToList();
            //foreach (var t in templist)
            //{
            //    BrhFrontDeskAccounts brhFrontDeskAccounts = new BrhFrontDeskAccounts();
            //    brhFrontDeskAccounts.Branch = "既见·南国";
            //    brhFrontDeskAccounts.Channel = t.订单来源;
            //    brhFrontDeskAccounts.CustomerName = t.客人;
            //    brhFrontDeskAccounts.CustomerCount = 1;
            //    brhFrontDeskAccounts.EndDate = t.退房时间;
            //    brhFrontDeskAccounts.EnteringStaff = "李辉";
            //    brhFrontDeskAccounts.FrontDeskAccountsId = t.系统订单号;
            //    brhFrontDeskAccounts.FrontDeskLeader = "";
            //    string[] ts = t.房间号.Split(".");
            //    brhFrontDeskAccounts.HouseNumber = ts[0];
            //    brhFrontDeskAccounts.Note = t.订单备注;
            //    brhFrontDeskAccounts.Receivable = t.房费;
            //    brhFrontDeskAccounts.RelationStaff = "";
            //    brhFrontDeskAccounts.StartDate = t.到店时间;
            //    brhFrontDeskAccounts.Steward = "";
            //    brhFrontDeskAccounts.StewardLeader = "";
            //    brhFrontDeskAccounts.TotalPrice = t.房费;
            //    brhFrontDeskAccounts.UnitPrice = t.房费 / t.间夜;
            //    brhFrontDeskAccounts.Count = t.间夜;
            //    foreach (StateType f in Enum.GetValues(typeof(StateType)))
            //    {
            //        if (t.订单状态 == Enum.GetName(typeof(StateType), f))
            //        {
            //            brhFrontDeskAccounts.State = f;
            //        }
            //    }

            //    brhFrontDeskAccounts.Received = t.房费;

            //    brhFrontDeskAccounts.IsFinance = true;
            //    brhFrontDeskAccounts.IsFront = true;
            //    brhFrontDeskAccounts.IsFinish = true;

            //    brhFrontDeskAccounts.EnteringDate = t.预订日期;

            //    var channel1 = _context.FncChannelType.ToList();
            //    var test = 0;
            //    foreach (var c in channel1)
            //    {
            //        if (c.ChannelType == brhFrontDeskAccounts.Channel)
            //        {
            //            brhFrontDeskAccounts.Color = c.Color;
            //            test++;
            //            break;
            //        }
            //    }
            //    if (test == 0)
            //        brhFrontDeskAccounts.Color = "yellow";
            //    if (_context.BrhFrontDeskAccounts.Find(brhFrontDeskAccounts.FrontDeskAccountsId) == null)
            //    {
            //        _context.Add(brhFrontDeskAccounts);
            //        await _context.SaveChangesAsync();
            //    }
            //}
            #endregion

            #region 计算间夜和订单状态
            //var templist = _context.BrhFrontDeskAccounts.Where(x=>DateTime.Compare(x.EndDate, Convert.ToDateTime("2018-11-26"))<=0&&x.Color=="yellow").ToList();
            //foreach (var t in templist)
            //{
            //t.State = StateType.已退房;
            //t.Count = (t.EndDate - t.StartDate).Days;
            //x.Count != (x.EndDate - x.StartDate).Days && (x.EndDate - x.StartDate).Days != 0
            //var count = (t.EndDate - t.StartDate).Days;
            //    t.Count = count;
            //    t.UnitPrice = t.TotalPrice / count;
            //if (t.Color == "yellow")
            //{
            //t.Color = "gray";
            //}
            //_context.Update(t);
            //_context.SaveChanges();
            //}

            #endregion

            ViewData["BranchId"] = branchId;
            var brhfront = new BrhFrontModel();
            brhfront.Branch = branch;
            brhfront.BranchId = branchId;
            var list_branch = _identityContext.UserBranch.Where(x => x.BranchName != "运营中心" && x.BranchName != "町隐学院").ToList();

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
            return View(Tuple.Create<List<FncChannelType>, BrhFrontModel>(channel, brhfront));
        }

        public JsonResult Edit([FromBody]Event eve)
        {
            var brhFrontDeskAccounts = _context.BrhFrontDeskAccounts.SingleOrDefault(x => x.FrontDeskAccountsId ==eve.FrontDeskAccountsId);
            brhFrontDeskAccounts.IsFinance = eve.IsFinance;
            _context.Update(brhFrontDeskAccounts);
            _context.SaveChanges();

            return Json(new { eve });
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
            var list2 = _context.BrhFrontPaymentDetials2.Where(x => x.FrontDeskAccountsId == brhFrontModel.FrontDeskAccountsId).ToList();
            return Json(new { list1, list2 });
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
            brhFrontDeskAccounts.TotalPrice = brhFrontDeskAccounts.UnitPrice * days;
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

        //public async Task<JsonResult> GetCalendarData([FromBody]BrhFrontModel brhFrontModel)
        //{
        //    var startDate = DateTime.Now.AddDays(-30);
        //    AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
        //    var branch = brhFrontModel.Branch;
        //    var branchId = brhFrontModel.BranchId;
        //    var frontdata = _context.BrhFrontDeskAccounts.Where(x => DateTime.Compare(DateTime.Now.AddDays(-90), x.StartDate) <= 0 && x.Branch == branch && x.State != StateType.已删除).ToList();
        //    var list_branch = _identityContext.UserBranchDetial.Include(x => x.UserBranch).GroupBy(x => new
        //    {
        //        x.BranchId,
        //        x.UserBranch.BranchName,
        //        x.HouseNumber,
        //    }).Select(x => new
        //    {
        //        BranchId = x.Key.BranchId,
        //        Branch = x.Key.BranchName,
        //        HouseNumber = x.Key.HouseNumber,
        //    }).ToList();

        //    var houseNumbers = list_branch.Where(x => x.BranchId == branchId).Select(x => x.HouseNumber).ToList();
        //    var branchdata = houseNumbers;

        //    Branch brData = new Branch();
        //    List<RoomType> roomTypeList = new List<RoomType>();
        //    var fncBranch = _context.FncBranch.SingleOrDefault(x => x.BranchId == 2);
        //    var fncHouseTypeList = _context.FncHouseType.Include(x => x.FncHouseNumber).Where(x => x.BranchId == 2).ToList();
        //    var typeCollet = fncHouseTypeList.Select(x => x.HouseTypeId).ToArray();
        //    var fncHouseNumberList = _context.FncHouseNumber.Where(x => typeCollet.Contains(x.HouseTypeId)).ToList();
        //    foreach (var fncHouseType in fncHouseTypeList)
        //    {
        //        var roomType = new RoomType();
        //        var roomList = new List<Room>();
        //        roomType.id = fncHouseType.HouseTypeId.ToString();
        //        roomType.title = fncHouseType.HouseType;
        //        roomType.order = fncHouseType.Order;
        //        foreach (var fncHouseNumber in fncHouseNumberList)
        //        {
        //            var room = new Room();
        //            if (fncHouseType.HouseTypeId == fncHouseNumber.HouseTypeId)
        //            {
        //                room.id = fncHouseNumber.HouseNumberId;
        //                room.title = fncHouseNumber.HouseNumber;
        //                roomList.Add(room);
        //            }
        //        }
        //        roomType.children = roomList;
        //        roomTypeList.Add(roomType);
        //    }

        //    brData.id = fncBranch.BranchName;
        //    brData.title = fncBranch.BranchName;
        //    brData.children = roomTypeList;

        //    List<Event> events = new List<Event>();
        //    for (var i = 0; i < 30; i++)
        //    {
        //        var eventTotal = new Event();
        //        var tempDate = startDate.AddDays(i);
        //        var templist = frontdata.Where(x => DateTime.Compare(x.StartDate, tempDate) <= 0 && DateTime.Compare(tempDate, x.EndDate) < 0).ToList();
        //        eventTotal.id = brData.id;
        //        eventTotal.resourceId = brData.id;
        //        var total = fncHouseNumberList.Count - templist.Count;
        //        if (total == 0)
        //        {
        //            eventTotal.title = "满房";
        //            eventTotal.className = "fullBranch1";
        //        }
        //        else
        //        {
        //            eventTotal.title = total.ToString() + " 间";
        //            eventTotal.className = "fullBranch2";
        //        }
        //        eventTotal.allDay = true;
        //        eventTotal.start = tempDate.Date.ToString();
        //        eventTotal.end = tempDate.AddDays(1).Date.ToString();
        //        eventTotal.editable = false;
        //        events.Add(eventTotal);
        //        foreach (var br in brData.children)
        //        {
        //            var tempevent = new Event();
        //            tempevent.id = br.id;
        //            tempevent.resourceId = br.id;
        //            var j = 0;
        //            foreach (var ff in templist)
        //            {
        //                if (br.children.Select(x => x.id).Contains(ff.HouseNumber))
        //                {
        //                    j++;
        //                }
        //            }
        //            if (br.children.Count - j == 0)
        //            {
        //                tempevent.title = "无房";
        //                tempevent.className = "fullHouse1";
        //            }
        //            else
        //            {
        //                tempevent.title = (br.children.Count - j).ToString() + " 间";
        //                tempevent.className = "fullHouse2";
        //            }
        //            tempevent.allDay = true;
        //            tempevent.start = tempDate.Date.ToString();
        //            tempevent.end = tempDate.AddDays(1).Date.ToString();
        //            tempevent.editable = false;
        //            events.Add(tempevent);
        //        }
        //    }

        //    return Json(new { frontdata, branchdata, brData, events });
        //}

        public async Task<JsonResult> GetCalendarData([FromBody]BrhFrontModel brhFrontModel)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);

            #region GetEvents
            var startDate = brhFrontModel.StartDate;
            var endDate = brhFrontModel.EndDate;
            var branch = brhFrontModel.Branch;
            var branchId = brhFrontModel.BranchId;
            var frontData = _context.BrhFrontDeskAccounts.Where(x => DateTime.Compare(startDate, x.StartDate) <= 0 && DateTime.Compare(x.StartDate, endDate) < 0 && x.Branch == branch && x.State != StateType.已删除).ToList();

            Branch resources1 = new Branch();
            Branch2 resources2 = new Branch2();
            List<RoomType> roomTypeList = new List<RoomType>();
            var fncBranch = _context.FncBranch.SingleOrDefault(x => x.BranchId == 2);
            var fncHouseTypeList = _context.FncHouseType.Include(x => x.FncHouseNumber).Where(x => x.BranchId == 2).ToList();
            var typeCollet = fncHouseTypeList.Select(x => x.HouseTypeId).ToArray();
            var fncHouseNumberList = _context.FncHouseNumber.Where(x => typeCollet.Contains(x.HouseTypeId)).ToList();
            foreach (var fncHouseType in fncHouseTypeList)
            {
                var roomType = new RoomType();
                var roomList = new List<Room>();
                roomType.id = fncHouseType.HouseTypeId.ToString();
                roomType.title = fncHouseType.HouseType;
                roomType.order = fncHouseType.Order;
                foreach (var fncHouseNumber in fncHouseNumberList)
                {
                    var room = new Room();
                    if (fncHouseType.HouseTypeId == fncHouseNumber.HouseTypeId)
                    {
                        room.id = fncHouseNumber.HouseNumberId;
                        room.title = fncHouseNumber.HouseNumber;
                        roomList.Add(room);
                    }
                }
                roomType.children = roomList;
                roomTypeList.Add(roomType);
            }
            List<Room> roomNumberList = new List<Room>();
            foreach (var fncHouseNumber in fncHouseNumberList)
            {
                var room = new Room();
                room.id = fncHouseNumber.HouseNumberId;
                room.title = fncHouseNumber.HouseNumber;
                roomNumberList.Add(room);
            }

            resources1.id = fncBranch.BranchName;
            resources1.title = fncBranch.BranchName;
            resources1.children = roomTypeList;
            resources2.id = fncBranch.BranchName;
            resources2.title = fncBranch.BranchName;
            resources2.children = roomNumberList;

            List<Event> events = new List<Event>();
            for (var i = 0; i < 30; i++)
            {
                var eventTotal = new Event();
                var tempDate = startDate.AddDays(i);
                var templist = frontData.Where(x => DateTime.Compare(x.StartDate, tempDate) <= 0 && DateTime.Compare(tempDate, x.EndDate) < 0).ToList();
                eventTotal.id = resources1.id;
                eventTotal.resourceId = resources1.id;
                var total = fncHouseNumberList.Count - templist.Count;
                if (total == 0)
                {
                    eventTotal.title = "满房";
                    eventTotal.className = "fullbranch";
                }
                else
                {
                    eventTotal.title = total.ToString() + " 间";
                    eventTotal.className = "emptybranch";
                }
                eventTotal.allDay = true;
                eventTotal.isTitle = true;
                eventTotal.start = tempDate.Date.ToString();
                eventTotal.end = tempDate.AddDays(1).Date.ToString();
                //eventTotal.color = "darkblue";
                eventTotal.editable = false;
                events.Add(eventTotal);
                foreach (var br in resources1.children)
                {
                    var tempevent = new Event();
                    tempevent.id = br.id;
                    tempevent.resourceId = br.id;
                    var j = 0;
                    foreach (var ff in templist)
                    {
                        if (br.children.Select(x => x.id).Contains(ff.HouseNumber))
                        {
                            j++;
                        }
                    }
                    if (br.children.Count - j == 0)
                    {
                        tempevent.title = "无房";
                        tempevent.className = "fullhouse";
                    }
                    else
                    {
                        tempevent.title = (br.children.Count - j).ToString() + " 间";
                        tempevent.className = "emptyhouse";
                    }
                    tempevent.allDay = true;
                    tempevent.isTitle = true;
                    tempevent.start = tempDate.Date.ToString();
                    tempevent.end = tempDate.AddDays(1).Date.ToString();
                    //tempevent.color = "gray";
                    tempevent.editable = false;
                    events.Add(tempevent);
                }
            }
            foreach (var f in frontData)
            {
                var tempevent = new Event();
                tempevent.id = f.FrontDeskAccountsId.ToString();
                tempevent.resourceId = f.HouseNumber;
                tempevent.title = f.CustomerName + " " + f.Channel;
                tempevent.allDay = true;
                tempevent.start = f.StartDate.Date.ToString();
                tempevent.end = f.EndDate.Date.ToString();
                tempevent.Color = f.Color;
                tempevent.IsFinance = f.IsFinance;
                tempevent.IsFinish = f.IsFinish;
                tempevent.IsFront = f.IsFront;
                tempevent.EnteringDate = f.EnteringDate;
                tempevent.Branch = f.Branch;
                tempevent.Count = f.Count;
                tempevent.Channel = f.Channel;
                tempevent.CustomerName = f.CustomerName;
                tempevent.CustomerCount = f.CustomerCount;
                tempevent.EndDate = f.EndDate;
                tempevent.EnteringStaff = f.EnteringStaff;
                tempevent.FrontDeskAccountsId = f.FrontDeskAccountsId;
                tempevent.FrontDeskLeader = f.FrontDeskLeader;
                tempevent.HouseNumber = f.HouseNumber;
                tempevent.Note = f.Note;
                tempevent.Phone = f.Phone;
                tempevent.Receivable = f.Receivable;
                tempevent.Received = f.Received;
                tempevent.RelationStaff = f.RelationStaff;
                tempevent.State = f.State;
                tempevent.StartDate = f.StartDate;
                tempevent.Steward = f.Steward;
                tempevent.StewardLeader = f.StewardLeader;
                tempevent.TotalPrice = f.TotalPrice;
                tempevent.UnitPrice = f.UnitPrice;

                events.Add(tempevent);
            }
            #endregion

            return Json(new {events,resources1,resources2,frontData});
        }

        //public async Task<JsonResult> GetResources([FromBody]BrhFrontModel brhFrontModel)
        //{
        //    AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);

        //    #region GetResources
        //    var branch = brhFrontModel.Branch;
        //    var branchId = brhFrontModel.BranchId;

        //    Branch resources1 = new Branch();
        //    List<RoomType> roomTypeList = new List<RoomType>();
        //    var fncBranch = _context.FncBranch.SingleOrDefault(x => x.BranchId == 2);
        //    var fncHouseTypeList = _context.FncHouseType.Include(x => x.FncHouseNumber).Where(x => x.BranchId == 2).ToList();
        //    var typeCollet = fncHouseTypeList.Select(x => x.HouseTypeId).ToArray();
        //    var fncHouseNumberList = _context.FncHouseNumber.Where(x => typeCollet.Contains(x.HouseTypeId)).ToList();
        //    foreach (var fncHouseType in fncHouseTypeList)
        //    {
        //        var roomType = new RoomType();
        //        var roomList = new List<Room>();
        //        roomType.id = fncHouseType.HouseTypeId.ToString();
        //        roomType.title = fncHouseType.HouseType;
        //        roomType.order = fncHouseType.Order;
        //        foreach (var fncHouseNumber in fncHouseNumberList)
        //        {
        //            var room = new Room();
        //            if (fncHouseType.HouseTypeId == fncHouseNumber.HouseTypeId)
        //            {
        //                room.id = fncHouseNumber.HouseNumberId;
        //                room.title = fncHouseNumber.HouseNumber;
        //                roomList.Add(room);
        //            }
        //        }
        //        roomType.children = roomList;
        //        roomTypeList.Add(roomType);
        //    }

        //    #endregion

        //    return Json(new {resources1});
        //}
    }
    public class Branch
    {
        public string id { get; set; }
        public string title { get; set; }
        public List<RoomType> children { get; set; }
    }
    public class Branch2
    {
        public string id { get; set; }
        public string title { get; set; }
        public List<Room> children { get; set; }
    }
    public class RoomType
    {
        public string id { get; set; }
        public string title { get; set; }
        public string order { get; set; }
        public List<Room> children { get; set; }
    }
    public class Room
    {
        public string id { get; set; }
        public string title { get; set; }
    }
    public class Event:BrhFrontDeskAccounts
    {
        public string id { get; set; }
        public string resourceId { get; set; }
        public string title { get; set; }
        public bool allDay { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        //public string color { get; set; }
        public string className { get; set; }
        public bool editable { get; set; }
        public bool isTitle { get; set; }
        //public bool isFinance { get; set; }
        //public bool isFront { get; set; }
        //public bool isFinish { get; set; }
    }
}
