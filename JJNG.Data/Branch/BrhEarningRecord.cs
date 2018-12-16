using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Branch
{
    [Table("Brh_EarningRecord")]
    public partial class BrhEarningRecord
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int EarningRecordId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日HH时mm分}")]
        [Display(Name = "录入时间")]
        public DateTime EnteringDate { get; set; }
        [Display(Name = "分类")]
        public string EarningType { get; set; }
        [Display(Name = "来源")]
        public string Source { get; set; }
        [Required]
        [Display(Name = "金额")]
        public double  Amount{ get; set; }
        [Required]
        [Display(Name = "收款方式")]
        public string PaymentType { get; set; }
        [Required]
        [Display(Name = "录单人")]
        public string EnteringStaff { get; set; }
        [Required]
        [Display(Name = "财务审核")]
        public bool IsFinance { get; set; }
        [Required]
        [Display(Name = "分店")]
        public string Branch { get; set; }
        [Display(Name = "备注")]
        public string Note { get; set; }
    }
}
