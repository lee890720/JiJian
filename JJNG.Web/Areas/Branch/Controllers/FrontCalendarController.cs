using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
using JJNG.Data.Finance;
using JJNG.Web.Areas.Branch.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admins,前台,管家,前台审核,管家审核")]
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
            ViewData["BranchId"] = _user.BranchId;
            var list_paymentType = _context.FncPaymentType.ToList();
            ViewData["PaymentType"] = new SelectList(list_paymentType, "PaymentType", "PaymentType");
            var list_channelType = _context.FncChannelType.ToList();
            ViewData["ChannelType"] = new SelectList(list_channelType, "ChannelType", "ChannelType");

            return View(_user);
        }

        public async Task<JsonResult> Create([FromBody]BranchModel branchModel)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (branchModel.StartDate.Month == branchModel.EndDate.AddDays(-1).Month)
            {
                var frontId = Convert.ToInt64(_user.BranchId.ToString() + ConvertJson.DateTimeToStamp(DateTime.Now).ToString());
                branchModel.FrontDeskAccountsId = frontId;
                BrhFrontDeskAccounts brhFrontDeskAccounts = new BrhFrontDeskAccounts();
                BrhFrontPaymentDetial bfp = new BrhFrontPaymentDetial();

                if (branchModel.PayAmount != 0)
                {
                    bfp.FrontDeskAccountsId = branchModel.FrontDeskAccountsId;
                    bfp.PayWay = branchModel.PayWay;
                    bfp.PayDate = branchModel.PayDate;
                    bfp.PayAmount = branchModel.PayAmount;
                }
                else
                    bfp.PayAmount = 0;
                branchModel.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                branchModel.Received = bfp.PayAmount;
                if (branchModel.Receivable == branchModel.Received)
                    branchModel.IsFinish = true;

                var ParentType = typeof(BrhFrontDeskAccounts);
                var Properties = ParentType.GetProperties();
                foreach (var Propertie in Properties)
                {
                    if (Propertie.CanRead && Propertie.CanWrite)
                    {
                        Propertie.SetValue(brhFrontDeskAccounts, Propertie.GetValue(branchModel, null), null);
                    }
                }
                if (branchModel.PayAmount != 0)
                    brhFrontDeskAccounts.BrhFrontPaymentDetial.Add(bfp);
                _context.Add(brhFrontDeskAccounts);
                _context.SaveChanges();

                return Json(new
                {
                    id = brhFrontDeskAccounts.FrontDeskAccountsId,
                    enteringDate = brhFrontDeskAccounts.EnteringDate,
                    state = branchModel.State,
                    received = brhFrontDeskAccounts.Received,
                    isFinish = brhFrontDeskAccounts.IsFinish
                });
            }
            else
            {
               // branchModel.StartDate.Month == branchModel.EndDate.AddDays(-1).Month;
                var count1 = (branchModel.EndDate.AddDays(1-branchModel.EndDate.Day)-branchModel.StartDate).Days;
                var count2 = (branchModel.EndDate- branchModel.EndDate.AddDays(1 - branchModel.EndDate.Day)).Days;
                var total = (branchModel.EndDate-branchModel.StartDate).Days;
                var frontId1 = Convert.ToInt64(_user.BranchId.ToString() + ConvertJson.DateTimeToStamp(DateTime.Now).ToString());
                BrhFrontDeskAccounts brhFrontDeskAccounts1 = new BrhFrontDeskAccounts();
                BrhFrontPaymentDetial bfp1 = new BrhFrontPaymentDetial();
                var frontId2 = Convert.ToInt64(_user.BranchId.ToString() + ConvertJson.DateTimeToStamp(DateTime.Now).ToString())+1;
                BrhFrontDeskAccounts brhFrontDeskAccounts2 = new BrhFrontDeskAccounts();
                BrhFrontPaymentDetial bfp2 = new BrhFrontPaymentDetial();

                if (branchModel.PayAmount != 0)
                {
                    bfp1.FrontDeskAccountsId = frontId1;
                    bfp1.PayWay = branchModel.PayWay;
                    bfp1.PayDate = branchModel.PayDate;
                    bfp1.PayAmount = branchModel.PayAmount / total * count1;
                    bfp2.FrontDeskAccountsId = frontId2;
                    bfp2.PayWay = branchModel.PayWay;
                    bfp2.PayDate = branchModel.PayDate;
                    bfp2.PayAmount = branchModel.PayAmount / total * count2;
                }
                else
                {
                    bfp1.PayAmount = 0;
                    bfp2.PayAmount = 0;
                }
                branchModel.EnteringDate = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
                var totalPrice = branchModel.TotalPrice;
                var receivable = branchModel.Receivable;
                var received = branchModel.Received;
                var startDate = branchModel.StartDate;
                var endDate = branchModel.EndDate;

                branchModel.FrontDeskAccountsId = frontId1;
                branchModel.TotalPrice = totalPrice / total * count1;
                branchModel.Receivable = receivable / total * count1;
                branchModel.Received =bfp1.PayAmount;
                if (branchModel.Receivable == branchModel.Received)
                    branchModel.IsFinish = true;
                else
                    branchModel.IsFinish = false;
                branchModel.Count = count1;
                branchModel.StartDate = startDate;
                branchModel.EndDate = endDate.AddDays(1 - endDate.Day);

                var ParentType = typeof(BrhFrontDeskAccounts);
                var Properties = ParentType.GetProperties();
                foreach (var Propertie in Properties)
                {
                    if (Propertie.CanRead && Propertie.CanWrite)
                    {
                        Propertie.SetValue(brhFrontDeskAccounts1, Propertie.GetValue(branchModel, null), null);
                    }
                }
                if (branchModel.PayAmount != 0)
                {
                    bfp1.FrontDeskAccountsId = frontId1;
                    brhFrontDeskAccounts1.BrhFrontPaymentDetial.Add(bfp1);
                }
                _context.Add(brhFrontDeskAccounts1);
                _context.SaveChanges();


                branchModel.FrontDeskAccountsId = frontId2;
                branchModel.TotalPrice = totalPrice / total * count2;
                branchModel.Receivable = receivable / total * count2;
                branchModel.Received = bfp2.PayAmount;
                if (branchModel.Receivable == branchModel.Received)
                    branchModel.IsFinish = true;
                else
                    branchModel.IsFinish = false;
                branchModel.Count = count2;
                branchModel.StartDate = endDate.AddDays(1 - endDate.Day);
                branchModel.EndDate = endDate;

                foreach (var Propertie in Properties)
                {
                    if (Propertie.CanRead && Propertie.CanWrite)
                    {
                        Propertie.SetValue(brhFrontDeskAccounts2, Propertie.GetValue(branchModel, null), null);
                    }
                }
                if (branchModel.PayAmount != 0)
                {
                    bfp2.FrontDeskAccountsId = frontId2;
                    brhFrontDeskAccounts2.BrhFrontPaymentDetial.Add(bfp2);
                }
                _context.Add(brhFrontDeskAccounts2);
                _context.SaveChanges();

                return Json(new
                {
                    id1 = brhFrontDeskAccounts1.FrontDeskAccountsId,
                    enteringDate1 = brhFrontDeskAccounts1.EnteringDate,
                    state1 = branchModel.State,
                    received1 = brhFrontDeskAccounts1.Received,
                    isFinish1 = brhFrontDeskAccounts1.IsFinish,
                    id2 = brhFrontDeskAccounts2.FrontDeskAccountsId,
                    enteringDate2 = brhFrontDeskAccounts2.EnteringDate,
                    state2 = branchModel.State,
                    received2 = brhFrontDeskAccounts2.Received,
                    isFinish2 = brhFrontDeskAccounts2.IsFinish,
                });
            }
        }


        public async Task<JsonResult> Edit([FromBody]BranchModel branchModel)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            BrhFrontDeskAccounts brhFrontDeskAccounts = new BrhFrontDeskAccounts();
            BrhFrontPaymentDetial bfp = new BrhFrontPaymentDetial();
            var rtotal = _context.BrhFrontPaymentDetials.Where(x => x.FrontDeskAccountsId == branchModel.FrontDeskAccountsId).Sum(x => x.PayAmount);
            if (branchModel.PayAmount != 0)
            {
                bfp.FrontDeskAccountsId = branchModel.FrontDeskAccountsId;
                bfp.PayWay = branchModel.PayWay;
                bfp.PayDate = branchModel.PayDate;
                bfp.PayAmount = branchModel.PayAmount;
            }
            else
                bfp.PayAmount = 0;

            branchModel.Received = rtotal + bfp.PayAmount;
            if (branchModel.Receivable == branchModel.Received)
                branchModel.IsFinish = true;
            else
                branchModel.IsFinish = false;

            var ParentType = typeof(BrhFrontDeskAccounts);
            var Properties = ParentType.GetProperties();
            foreach (var Propertie in Properties)
            {
                //循环遍历属性
                if (Propertie.CanRead && Propertie.CanWrite)
                {
                    //进行属性拷贝
                    Propertie.SetValue(brhFrontDeskAccounts, Propertie.GetValue(branchModel, null), null);
                }
            }
            if (branchModel.PayAmount != 0)
                brhFrontDeskAccounts.BrhFrontPaymentDetial.Add(bfp);
            _context.Update(brhFrontDeskAccounts);
            _context.SaveChanges();
            return Json(new
            {
                state = branchModel.State,
                received = brhFrontDeskAccounts.Received,
                isFinish = brhFrontDeskAccounts.IsFinish
            });
        }

        public async Task<JsonResult> EditBranch([FromBody]FncBranch fncBranch)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var brh = _context.FncBranch.SingleOrDefault(x => x.BranchId == fncBranch.BranchId);
            brh.IsType = fncBranch.IsType;
            _context.Update(brh);
            _context.SaveChanges();
            return Json(new { brh });
        }

        public async Task<JsonResult> EditHouseNumber([FromBody]Room temp)
        {
            var room = _context.FncHouseNumber.SingleOrDefault(x => x.HouseNumber == temp.id && x.HouseTypeId == temp.typeId);
            room.isClean = temp.isClean;
            _context.Update(room);
            await _context.SaveChangesAsync();
            return Json(new { room });
        }

        public async Task<JsonResult> Delete([FromBody]BranchModel branchModel)
        {
            var brhFrontDeskAccounts = await _context.BrhFrontDeskAccounts.SingleOrDefaultAsync(m => m.FrontDeskAccountsId == branchModel.FrontDeskAccountsId);
            _context.BrhFrontDeskAccounts.Remove(brhFrontDeskAccounts);
            await _context.SaveChangesAsync();
            return Json(new { brhFrontDeskAccounts });
        }

        public async Task<JsonResult> List([FromBody]BranchModel branchModel)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var list = _context.BrhFrontPaymentDetials.Where(x => x.FrontDeskAccountsId == branchModel.FrontDeskAccountsId).ToList();
            return Json(new { list});
        }

        public async Task<JsonResult> Drop([FromBody]Event eve)
        {
            var brhFrontDeskAccounts = await _context.BrhFrontDeskAccounts.SingleOrDefaultAsync(m => m.FrontDeskAccountsId == eve.FrontDeskAccountsId);
            brhFrontDeskAccounts.HouseNumber = eve.HouseNumber;
            brhFrontDeskAccounts.StartDate = eve.StartDate;
            brhFrontDeskAccounts.EndDate = eve.EndDate;
            _context.Update(brhFrontDeskAccounts);
            await _context.SaveChangesAsync();
            return Json(new { eve });
        }

        public async Task<JsonResult> Resize([FromBody]Event eve)
        {
            eve.Count = (eve.EndDate - eve.StartDate).Days;
            eve.TotalPrice = eve.UnitPrice * eve.Count;
            eve.Receivable = eve.TotalPrice;
            if (eve.Receivable == eve.Received)
            {
                eve.IsFinish = true;
            }
            else
            {
                eve.IsFinish = false;
            }

            var brhFrontDeskAccounts = await _context.BrhFrontDeskAccounts.SingleOrDefaultAsync(m => m.FrontDeskAccountsId == eve.FrontDeskAccountsId);
            brhFrontDeskAccounts.StartDate = eve.StartDate;
            brhFrontDeskAccounts.EndDate = eve.EndDate;
            brhFrontDeskAccounts.Count = eve.Count;
            brhFrontDeskAccounts.IsFinish = eve.IsFinish;
            brhFrontDeskAccounts.TotalPrice = eve.TotalPrice;
            brhFrontDeskAccounts.Receivable = eve.Receivable;
            _context.Update(brhFrontDeskAccounts);
            await _context.SaveChangesAsync();
            return Json(new { eve });
        }

        public async Task<JsonResult> GetCalendarData([FromBody]BranchModel branchModel)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var channel = _context.FncChannelType.ToList();

            #region GetEvents
            var startDate = branchModel.StartDate;
            var endDate = branchModel.EndDate;
            var branch = _user.Branch;
            var frontData = _context.BrhFrontDeskAccounts.Where(x => DateTime.Compare(startDate, x.EndDate) <= 0 && DateTime.Compare(x.StartDate, endDate) < 0 && x.Branch == branch && x.State != StateType.已删除).ToList();
            var todayData = _context.BrhFrontDeskAccounts.Where(x => DateTime.Compare(x.StartDate, DateTime.Now) <= 0 && DateTime.Compare(DateTime.Now, x.EndDate) < 0 && x.Branch == branch && x.State != StateType.已删除).ToList();
            Branch1 resources1 = new Branch1();
            Branch2 resources2 = new Branch2();
            List<RoomType> roomTypeList = new List<RoomType>();
            var fncBranch = _context.FncBranch.SingleOrDefault(x => x.BranchName == branch);
            var fncHouseTypeList = _context.FncHouseType.Include(x => x.FncHouseNumber).Where(x => x.BranchId == fncBranch.BranchId).ToList();
            var typeCollet = fncHouseTypeList.Select(x => x.HouseTypeId).ToArray();
            var fncHouseNumberList = _context.FncHouseNumber.Where(x => typeCollet.Contains(x.HouseTypeId)).ToList();
            List<Room> roomNumberList = new List<Room>();
            foreach (var fncHouseNumber in fncHouseNumberList)
            {
                var room = new Room();
                room.id = fncHouseNumber.HouseNumber;
                if (fncHouseNumber.isClean)
                    room.title = fncHouseNumber.HouseNumber;
                else
                    room.title = fncHouseNumber.HouseNumber + " 脏";
                room.typeId = fncHouseNumber.HouseTypeId;
                room.isClean = fncHouseNumber.isClean;
                foreach (var fh in todayData)
                {
                    if (fh.HouseNumber == fncHouseNumber.HouseNumber)
                    {
                        room.state = Enum.GetName(typeof(StateType), (int)fh.State);
                        break;
                    }
                }
                if (string.IsNullOrEmpty(room.state))
                    room.state = "空";
                roomNumberList.Add(room);
            }
            foreach (var fncHouseType in fncHouseTypeList)
            {
                var count1 = 0;
                var count2 = 0;
                var roomType = new RoomType();
                var roomList = new List<Room>();
                roomType.id = fncHouseType.Order.ToString();
                roomType.title = fncHouseType.HouseType;
                roomType.order = fncHouseType.Order;
                foreach (var rrr in roomNumberList)
                {
                    var room = new Room();
                    if (fncHouseType.HouseTypeId == rrr.typeId)
                    {
                        count1++;
                        room.id = rrr.id;
                        room.title = rrr.title;
                        room.state = rrr.state;
                        room.typeId = rrr.typeId;
                        room.isClean = rrr.isClean;
                        if (room.state != "空")
                            count2++;
                        roomList.Add(room);
                    }
                }
                if (count1 != count2)
                    roomType.state = (count1 - count2).ToString() + " 间";
                else
                    roomType.state = "无房";
                roomType.children = roomList;
                roomTypeList.Add(roomType);
            }

            resources1.id = fncBranch.BranchName;
            resources1.title = fncBranch.BranchName;
            resources1.isType = fncBranch.IsType;
            resources1.children = roomTypeList;
            resources2.id = fncBranch.BranchName;
            resources2.title = fncBranch.BranchName;
            resources2.isType = fncBranch.IsType;
            resources2.children = roomNumberList;
            if (fncHouseNumberList.Count != todayData.Count)
            {
                resources1.state = (fncHouseNumberList.Count - todayData.Count).ToString() + " 间";
                resources2.state = (fncHouseNumberList.Count - todayData.Count).ToString() + " 间";
            }
            else
            {
                resources1.state = "满房";
                resources2.state = "满房";
            }

            List<Event> events = new List<Event>();
            for (var i = 0; i < 31; i++)
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
                if (tempevent.IsFinance)
                    tempevent.editable = true;  //临时
                else
                    tempevent.editable = true;

                events.Add(tempevent);
            }
            #endregion

            return Json(new { events, resources1, resources2, channel });
        }

        public async Task<JsonResult> GetResources([FromBody]BranchModel branchModel)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var channel = _context.FncChannelType.ToList();

            #region GetEvents
            var branch = _user.Branch;
            var todayData = _context.BrhFrontDeskAccounts.Where(x => DateTime.Compare(x.StartDate, DateTime.Now) <= 0 && DateTime.Compare(DateTime.Now, x.EndDate) < 0 && x.Branch == branch && x.State != StateType.已删除).ToList();
            Branch1 resources1 = new Branch1();
            Branch2 resources2 = new Branch2();
            List<RoomType> roomTypeList = new List<RoomType>();
            var fncBranch = _context.FncBranch.SingleOrDefault(x => x.BranchName == branch);
            var fncHouseTypeList = _context.FncHouseType.Include(x => x.FncHouseNumber).Where(x => x.BranchId == fncBranch.BranchId).ToList();
            var typeCollet = fncHouseTypeList.Select(x => x.HouseTypeId).ToArray();
            var fncHouseNumberList = _context.FncHouseNumber.Where(x => typeCollet.Contains(x.HouseTypeId)).ToList();
            List<Room> roomNumberList = new List<Room>();
            foreach (var fncHouseNumber in fncHouseNumberList)
            {
                var room = new Room();
                room.id = fncHouseNumber.HouseNumber;
                if (fncHouseNumber.isClean)
                    room.title = fncHouseNumber.HouseNumber;
                else
                    room.title = fncHouseNumber.HouseNumber + " 脏";
                room.typeId = fncHouseNumber.HouseTypeId;
                room.isClean = fncHouseNumber.isClean;
                foreach (var fh in todayData)
                {
                    if (fh.HouseNumber == fncHouseNumber.HouseNumber)
                    {
                        room.state = Enum.GetName(typeof(StateType), (int)fh.State);
                        break;
                    }
                }
                if (string.IsNullOrEmpty(room.state))
                    room.state = "空";
                roomNumberList.Add(room);
            }
            foreach (var fncHouseType in fncHouseTypeList)
            {
                var count1 = 0;
                var count2 = 0;
                var roomType = new RoomType();
                var roomList = new List<Room>();
                roomType.id = fncHouseType.Order.ToString();
                roomType.title = fncHouseType.HouseType;
                roomType.order = fncHouseType.Order;
                foreach (var rrr in roomNumberList)
                {
                    var room = new Room();
                    if (fncHouseType.HouseTypeId == rrr.typeId)
                    {
                        count1++;
                        room.id = rrr.id;
                        room.title = rrr.title;
                        room.state = rrr.state;
                        room.typeId = rrr.typeId;
                        room.isClean = rrr.isClean;
                        if (room.state != "空")
                            count2++;
                        roomList.Add(room);
                    }
                }
                if (count1 != count2)
                    roomType.state = (count1 - count2).ToString() + " 间";
                else
                    roomType.state = "无房";
                roomType.children = roomList;
                roomTypeList.Add(roomType);
            }

            resources1.id = fncBranch.BranchName;
            resources1.title = fncBranch.BranchName;
            resources1.isType = fncBranch.IsType;
            resources1.children = roomTypeList;
            resources2.id = fncBranch.BranchName;
            resources2.title = fncBranch.BranchName;
            resources2.isType = fncBranch.IsType;
            resources2.children = roomNumberList;
            if (fncHouseNumberList.Count != todayData.Count)
            {
                resources1.state = (fncHouseNumberList.Count - todayData.Count).ToString() + " 间";
                resources2.state = (fncHouseNumberList.Count - todayData.Count).ToString() + " 间";
            }
            else
            {
                resources1.state = "满房";
                resources2.state = "满房";
            }
            #endregion

            return Json(new { resources1, resources2, channel });
        }

        public static TChild AutoCopy<TParent, TChild>(TParent parent) where TChild : TParent, new()
        {
            TChild child = new TChild();
            var ParentType = typeof(TParent);
            var Properties = ParentType.GetProperties();
            foreach (var Propertie in Properties)
            {
                //循环遍历属性
                if (Propertie.CanRead && Propertie.CanWrite)
                {
                    //进行属性拷贝
                    Propertie.SetValue(child, Propertie.GetValue(parent, null), null);
                }
            }
            return child;
        }
    }
}
