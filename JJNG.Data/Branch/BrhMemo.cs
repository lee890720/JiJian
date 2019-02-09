using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Branch
{
    [Table("Brh_Memo")]
    public partial class BrhMemo
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MemoId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Display(Name = "录入时间")]
        public DateTime EnteringDate { get; set; }
        [Display(Name = "备忘录")]
        public string Memo { get; set; }
        [Display(Name = "是否完成")]
        public bool IsFinish { get; set; }
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
