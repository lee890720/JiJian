using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Finance
{
    [Table("Fnc_MonthData")]
    public partial class FncMonthData
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MonthDataId { get; set; }
        public int BranchId { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月}")]
        [Display(Name = "月份")]
        public DateTime Month { get; set; }
        [Display(Name = "房费流水")]
        public decimal HouseAmount { get; set; }
        [Display(Name = "总间夜")]
        public int HouseTotal { get; set; }
        [Display(Name = "已售间夜")]
        public int HouseCount { get; set; }
        [Display(Name ="出租率")]
        public double Rate { get; set; }
        [Display(Name = "均价")]
        public decimal Average { get; set; }
        [Display(Name = "有效均价")]
        public decimal ValidAverage { get; set; }
        [Display(Name = "收入")]
        public decimal Earning { get; set; }
        [Display(Name = "支出")]
        public decimal Expend { get; set; }
        [Display(Name = "外销流水")]
        public decimal SaleAmount { get; set; }
        [Display(Name = "外销利润")]
        public decimal SaleProfit { get; set; }
        public FncBranch FncBranch { get; set; }
    }
}
