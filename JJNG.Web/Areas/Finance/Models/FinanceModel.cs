using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JJNG.Data.Finance;
using System.ComponentModel.DataAnnotations;

namespace JJNG.Web.Areas.Finance.Models
{
    public class FinanceModel
    {
    }

    public class MonthData:FncMonthData
    {
        public decimal 同比增长额 { get; set; }
        public decimal 环比增长额 { get; set; }
        public double 同比增长率 { get; set; }
        public double 环比增长率 { get; set; }
        public decimal 出同比增长额 { get; set; }
        public decimal 出环比增长额 { get; set; }
        public double 出同比增长率 { get; set; }
        public double 出环比增长率 { get; set; }
        public decimal 均同比增长额 { get; set; }
        public decimal 均环比增长额 { get; set; }
        public double 均同比增长率 { get; set; }
        public double 均环比增长率 { get; set; }
        public decimal 有同比增长额 { get; set; }
        public decimal 有环比增长额 { get; set; }
        public double 有同比增长率 { get; set; }
        public double 有环比增长率 { get; set; }
        public decimal 支同比增长额 { get; set; }
        public decimal 支环比增长额 { get; set; }
        public double 支同比增长率 { get; set; }
        public double 支环比增长率 { get; set; }
        public decimal 外同比增长额 { get; set; }
        public decimal 外环比增长额 { get; set; }
        public double 外同比增长率 { get; set; }
        public double 外环比增长率 { get; set; }
        public decimal 外利同比增长额 { get; set; }
        public decimal 外利环比增长额 { get; set; }
        public double 外利同比增长率 { get; set; }
        public double 外利环比增长率 { get; set; }
    }
}
