using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Areas.Branch.Models
{
    public class BrhFrontModel
    {
        public long FrontDeskAccountsId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        [Display(Name = "录入日期")]
        public DateTime EnteringDate { get; set; }

        [Required]
        [Display(Name = "房号")]
        public string HouseNumber { get; set; }

        [Required]
        [Display(Name = "入住人")]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "入住人数")]
        public int CustomerCount { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "入住日期")]
        public DateTime StartDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "离店日期")]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "订房渠道")]
        public string Channel { get; set; }

        [Required]
        [Display(Name = "单价")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Display(Name = "总价")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Display(Name = "应收款")]
        public decimal Receivable { get; set; }

        [Required]
        [Display(Name = "已收款")]
        public decimal Received { get; set; }

        [Required]
        [Display(Name = "是否结账")]
        public bool IsFinish { get; set; }

        [Required]
        [Display(Name = "录单人")]
        public string EnteringStaff { get; set; }

        [Display(Name = "管家")]
        public string Steward { get; set; }

        [Display(Name = "前台组长")]
        public string FrontDeskLeader { get; set; }

        [Display(Name = "管家组长")]
        public string StewardLeader { get; set; }

        [Display(Name = "对应关系")]
        public string RelationStaff { get; set; }

        [Required]
        [Display(Name = "前台审核")]
        public bool IsFront { get; set; }

        [Required]
        [Display(Name = "财务审核")]
        public bool IsFinance { get; set; }

        [Required]
        [Display(Name = "分店")]
        public string Branch { get; set; }

        [Display(Name = "备注")]
        public string Note { get; set; }

        public DateTime PayDate1 { get; set; }
        public string PayWay1 { get; set; }
        [Required]
        public decimal PayAmount1 { get; set; }
        public DateTime PayDate2 { get; set; }
        public string PayWay2 { get; set; }
        [Required]
        public decimal PayAmount2 { get; set; }
        public DateTime PayDate3 { get; set; }
        public string PayWay3 { get; set; }
        [Required]
        public decimal PayAmount3 { get; set; }
    }

    public class BrhGroupModel
    {
        public string PayWay { get; set; }
        public decimal FrontAmount { get; set; }
        public decimal StewardAmount { get; set; }
        public decimal EarningAmount { get; set; }
        public decimal Total { get; set; }
    }
}
