using JJNG.Data.Branch;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JJNG.Web.Areas.Branch.Models
{
    public class BranchModel : Event
    {
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Display(Name = "付款日期")]
        public DateTime PayDate { get; set; }
        [Required]
        [Display(Name = "付款方式")]
        public string PayWay { get; set; }
        [Required]
        [Display(Name = "付款金额")]
        public decimal PayAmount { get; set; }
    }
    public class Branch1
    {
        public string id { get; set; }
        public string title { get; set; }
        public bool isType { get; set; }
        public string state { get; set; }
        public List<RoomType> children { get; set; }
    }
    public class Branch2
    {
        public string id { get; set; }
        public string title { get; set; }
        public bool isType { get; set; }
        public string state { get; set; }
        public List<Room> children { get; set; }
    }
    public class RoomType
    {
        public string id { get; set; }
        public string title { get; set; }
        public string order { get; set; }
        public string state { get; set; }
        public List<Room> children { get; set; }
    }
    public class Room
    {
        public string id { get; set; }
        public string title { get; set; }
        public string state { get; set; }
        public bool isClean { get; set; }
        public int typeId { get; set; }
    }
    public class Event : BrhFrontDeskAccounts
    {
        public string id { get; set; }
        public string resourceId { get; set; }
        public string title { get; set; }
        public bool allDay { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string className { get; set; }
        public bool editable { get; set; }
        public bool isTitle { get; set; }
    }
}
