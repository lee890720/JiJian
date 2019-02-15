using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Branch
{
    [Table("Brh_Yun")]
    public partial class BrhYun
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public long 系统订单号 { get; set; }
        public DateTime 预订日期 { get; set; }
        public string 客人 { get; set; }
        public string 订单来源 { get; set; }
        public string 订单状态 { get; set; }
        public string 房间号 { get; set; }
        public DateTime 到店时间 { get; set; }
        public DateTime 退房时间 { get; set; }
        public int 间夜 { get; set; }
        public decimal 房费 { get; set; }
        public string 订单备注 { get; set; }
    }
}
