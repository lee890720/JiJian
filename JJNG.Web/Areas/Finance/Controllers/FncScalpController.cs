using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JJNG.Data;
using JJNG.Data.AppIdentity;
using JJNG.Data.Branch;
using JJNG.Data.Finance;
using JJNG.Web.Areas.Branch.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JJNG.Web.Areas.Branch.Controllers
{
    [Area("Finance")]
    [Authorize(Roles = "Admins,财务")]
    public class FncScalpController : Controller
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private UserManager<AppIdentityUser> _userManager;

        public FncScalpController(AppDbContext context, AppIdentityDbContext identityContext, UserManager<AppIdentityUser> usrMgr)
        {
            _context = context;
            _identityContext = identityContext;
            _userManager = usrMgr;
        }

        public async Task<ActionResult> Index(string branchName = "既见·南国", int branchId = 2)
        {
            ViewData["BranchId"] = branchId;
            var fncBranch = new FncBranch();
            fncBranch.BranchName = branchName;
            fncBranch.BranchId = branchId;

            var list_paymentType = await _context.FncPaymentType.ToListAsync();
            ViewData["PaymentType"] = new SelectList(list_paymentType, "PaymentType", "PaymentType");
            var list_channelType = await _context.FncChannelType.ToListAsync();
            ViewData["ChannelType"] = new SelectList(list_channelType, "ChannelType", "ChannelType");

            return View(fncBranch);
        }

        public async Task<JsonResult> Edit([FromBody]Event2 event2)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var brhScalp = _context.BrhScalp.SingleOrDefault(x => x.ScalpId == event2.ScalpId);
            brhScalp.IsFinance = event2.IsFinance;
            _context.Update(brhScalp);
            _context.SaveChanges();

            return Json(new { event2 });
        }



        public async Task<JsonResult> GetCalendarData([FromBody]Event2 event2)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var channel = _context.FncChannelType.ToList();

            #region GetEvents
            var startDate = event2.StartDate;
            var endDate = event2.EndDate;
            var branch = event2.Branch;
            var frontData = _context.BrhScalp.Where(x => DateTime.Compare(startDate, x.EndDate) <= 0 && DateTime.Compare(x.StartDate, endDate) < 0 && x.Branch == branch).ToList();
            var todayData = _context.BrhScalp.Where(x => DateTime.Compare(x.StartDate, DateTime.Now) <= 0 && DateTime.Compare(DateTime.Now, x.EndDate) < 0 && x.Branch == branch).ToList();
            Branch1 resources1 = new Branch1();
            Branch2 resources2 = new Branch2();
            List<RoomType> roomTypeList = new List<RoomType>();
            var fncBranch = _context.FncBranch.SingleOrDefault(x => x.BranchName == branch);
            var fncHouseTypeList = _context.FncHouseType.Include(x => x.FncHouseNumber).Where(x => x.BranchId == fncBranch.BranchId && x.IsReal).ToList();
            var typeCollet = fncHouseTypeList.Select(x => x.HouseTypeId).ToArray();
            var fncHouseNumberList = _context.FncHouseNumber.Where(x => typeCollet.Contains(x.HouseTypeId)).ToList();
            var numberCollet = fncHouseNumberList.Select(x => new { x.HouseNumberId, x.HouseNumber }).ToList();
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
                if (string.IsNullOrEmpty(room.state))
                    room.state = "空";
                foreach (var aaa in fncHouseTypeList)
                {
                    if (aaa.FncHouseNumber.Select(x => x.HouseNumber).Contains(room.title))
                    {
                        room.houseType = aaa.HouseType;
                        break;
                    }
                }
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
                        foreach (var aaa in fncHouseTypeList)
                        {
                            if (aaa.FncHouseNumber.Select(x => x.HouseNumber).Contains(room.title))
                            {
                                room.houseType = aaa.HouseType;
                                break;
                            }
                        }
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

            List<Event2> events = new List<Event2>();
            for (var i = 0; i < 31; i++)
            {
                var eventTotal = new Event2();
                var tempDate = startDate.AddDays(i);
                var templist = frontData.Where(x => DateTime.Compare(x.StartDate, tempDate) <= 0 && DateTime.Compare(tempDate, x.EndDate) < 0).ToList();
                eventTotal.id = resources1.id;
                eventTotal.resourceId = resources1.id;
                var total = templist.Count;
                if (total == fncHouseNumberList.Count)
                {
                    eventTotal.title = "满刷";
                    eventTotal.className = "fullbranch";
                }
                else
                {
                    eventTotal.title = total.ToString() + " 单";
                    eventTotal.className = "emptybranch";
                }
                eventTotal.allDay = true;
                eventTotal.isTitle = true;
                eventTotal.start = tempDate.Date.ToString();
                eventTotal.end = tempDate.AddDays(1).Date.ToString();
                eventTotal.editable = false;
                events.Add(eventTotal);
                foreach (var br in resources1.children)
                {
                    var tempevent = new Event2();
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
                var tempevent = new Event2();
                tempevent.id = f.ScalpId.ToString();
                tempevent.resourceId = f.HouseNumber;
                tempevent.title =  f.CustomerName+" "+f.Channel;
                tempevent.allDay = true;
                tempevent.start = f.StartDate.Date.ToString();
                tempevent.end = f.EndDate.Date.ToString();
                tempevent.Color = f.Color;
                tempevent.IsFinance = f.IsFinance;
                tempevent.IsFront = f.IsFront;
                tempevent.ImprestAccountsId = f.ImprestAccountsId;
                tempevent.EnteringDate = f.EnteringDate;
                tempevent.Branch = f.Branch;
                tempevent.Channel = f.Channel;
                tempevent.CustomerName = f.CustomerName;
                tempevent.EndDate = f.EndDate;
                tempevent.EnteringStaff = f.EnteringStaff;
                tempevent.ScalpId = f.ScalpId;
                tempevent.HouseNumber = f.HouseNumber;
                tempevent.Note = f.Note;
                tempevent.Settlement = f.Settlement;
                tempevent.Commission = f.Commission;
                tempevent.StartDate = f.StartDate;
                tempevent.TotalPrice = f.TotalPrice;
                tempevent.UnitPrice = f.UnitPrice;
                if (tempevent.IsFinance)
                    tempevent.editable = false;  //临时
                else
                    tempevent.editable = false;
                events.Add(tempevent);
            }
            #endregion

            return Json(new { events, resources1, resources2, channel, numberCollet });
        }

        public async Task<JsonResult> GetResources([FromBody]Event2 event2)
        {
            AppIdentityUser _user = await _userManager.FindByNameAsync(User.Identity.Name);
            var channel = _context.FncChannelType.ToList();

            #region GetEvents
            var branch = event2.Branch;
            Branch2 resources2 = new Branch2();
            List<RoomType> roomTypeList = new List<RoomType>();
            var fncBranch = _context.FncBranch.SingleOrDefault(x => x.BranchName == branch);
            var fncHouseTypeList = _context.FncHouseType.Include(x => x.FncHouseNumber).Where(x => x.BranchId == fncBranch.BranchId && x.IsReal).ToList();
            var typeCollet = fncHouseTypeList.Select(x => x.HouseTypeId).ToArray();
            var fncHouseNumberList = _context.FncHouseNumber.Where(x => typeCollet.Contains(x.HouseTypeId)).ToList();
            var numberCollet = fncHouseNumberList.Select(x => new { x.HouseNumberId, x.HouseNumber }).ToList();
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
                if (string.IsNullOrEmpty(room.state))
                    room.state = "空";
                foreach (var aaa in fncHouseTypeList)
                {
                    if (aaa.FncHouseNumber.Select(x => x.HouseNumber).Contains(room.title))
                    {
                        room.houseType = aaa.HouseType;
                        break;
                    }
                }
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
                        foreach (var aaa in fncHouseTypeList)
                        {
                            if (aaa.FncHouseNumber.Select(x => x.HouseNumber).Contains(room.title))
                            {
                                room.houseType = aaa.HouseType;
                                break;
                            }
                        }
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

            resources2.id = fncBranch.BranchName;
            resources2.title = fncBranch.BranchName;
            resources2.isType = fncBranch.IsType;
            resources2.children = roomNumberList;
            #endregion

            return Json(new { resources2, channel, numberCollet });
        }
    }
}
