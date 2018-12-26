using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Branch
{
    [Table("Brh_StewardPaymentDetial")]
    public partial class BrhStewardPaymentDetial
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int StewardPaymentDetialId { get; set; }
        [Required]
        [Display(Name = "付款明细Id")]
        public long StewardAccountsId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}")]
        [Display(Name = "付款日期")]
        public DateTime PayDate { get; set; }
        [Required]
        [Display(Name = "付款方式")]
        public string PayWay { get; set; }
        [Required]
        [Display(Name = "付款金额")]
        public decimal PayAmount { get; set; }
        public BrhStewardAccounts BrhStewardAccounts { get; set; }
    }
}
