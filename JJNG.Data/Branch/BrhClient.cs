using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Branch
{
    [Table("Brh_Client")]
    public partial class BrhClient
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Display(Name = "录入时间")]
        public DateTime EnteringDate { get; set; }
        [Display(Name = "客户姓名/编号")]
        public string Name { get; set; }
        [Display(Name = "跟进详情")]
        public string Follow { get; set; }
        [Display(Name = "好评")]
        public bool IsGood { get; set; }
        [Display(Name = "外销")]
        public bool IsSale { get; set; }
        [Required]
        [Display(Name = "录单人")]
        public string EnteringStaff { get; set; }
        [Required]
        [Display(Name = "分店")]
        public string Branch { get; set; }
        [Display(Name = "备注")]
        public string Note { get; set; }
    }
}
