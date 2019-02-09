using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Branch
{
    [Table("Brh_StewardAccounts")]
    public partial class BrhStewardAccounts
    {
        public BrhStewardAccounts()
        {
            BrhStewardPaymentDetial = new HashSet<BrhStewardPaymentDetial>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public long StewardAccountsId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Display(Name = "录入日期")]
        public DateTime EnteringDate { get; set; }
        [Display(Name = "房号")]
        public string HouseNumber { get; set; }
        [Required]
        [Display(Name = "客户名称")]
        public string CustomerName { get; set; }
        [Required]
        [Display(Name = "外销分类")]
        public string ProductType { get; set; }
        [Required]
        [Display(Name = "外销名称")]
        public string Product { get; set; }
        [Required]
        [Display(Name = "成本")]
        public decimal Cost { get; set; }
        [Required]
        [Display(Name = "销售金额")]
        public decimal Amount { get; set; }
        [Required]
        [Display(Name = "利润")]
        public decimal Profit { get; set; }
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
        [Display(Name = "前台")]
        public string FrontDesk { get; set; }
        [Display(Name = "前台小组长")]
        public string FrontDeskLeader { get; set; }
        [Display(Name = "管家小组长")]
        public string StewardLeader { get; set; }
        [Display(Name = "对应关系")]
        public string RelationStaff { get; set; }
        [Required]
        [Display(Name = "管家审核")]
        public bool IsSteward { get; set; }
        [Required]
        [Display(Name = "财务审核")]
        public bool IsFinance { get; set; }
        [Required]
        [Display(Name = "分店")]
        public string Branch { get; set; }
        [Display(Name = "备注")]
        public string Note { get; set; }
        public ICollection<BrhStewardPaymentDetial> BrhStewardPaymentDetial { get; set; }
    }
}
