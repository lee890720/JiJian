using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Branch
{
    [Table("Brh_FrontPaymentDetial2")]
    public partial class BrhFrontPaymentDetial2
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FrontPaymentDetialId { get; set; }
        [Required]
        [Display(Name = "付款明细Id")]
        public long FrontDeskAccountsId { get; set; }
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
}
