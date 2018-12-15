using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Finance
{
    [Table("Fnc_ExpendType")]
    public partial class FncExpendType
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "支出分类")]
        public string ExpendType { get; set; }

        [Display(Name = "顺序")]
        public int Sequence { get; set; }
    }
}
