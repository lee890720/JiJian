using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Branch
{
    [Table("Brh_Scalp")]
    public partial class BrhScalp
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public long ScalpId { get; set; }

        public int ImprestAccountsId { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Display(Name = "录入日期")]
        public DateTime EnteringDate { get; set; }

        [Required]
        [Display(Name = "房号")]
        public string HouseNumber { get; set; }

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

        [Display(Name = "颜色")]
        public string Color { get; set; }

        [Required]
        [Display(Name = "单价")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Display(Name = "总价")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Display(Name = "结算价")]
        public decimal Settlement { get; set; }

        [Required]
        [Display(Name = "佣金")]
        public decimal Commission { get; set; }

        [Required]
        [Display(Name = "录单人")]
        public string EnteringStaff { get; set; }

        [Required]
        [Display(Name = "前台审核")]
        public bool IsFront { get; set; }

        [Required]
        [Display(Name = "财务审核")]
        public bool IsFinance { get; set; }

        [Display(Name = "是否转移")]
        public bool IsMove { get; set; }

        [Required]
        [Display(Name = "分店")]
        public string Branch { get; set; }

        [Display(Name = "备注")]
        public string Note { get; set; }
    }
}
