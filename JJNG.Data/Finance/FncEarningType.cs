using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Finance
{
    [Table("Fnc_EarningType")]
    public partial class FncEarningType
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "收入分类")]
        public string EarningType { get; set; }

        [Display(Name = "顺序")]
        public int Sequence { get; set; }
    }
}
