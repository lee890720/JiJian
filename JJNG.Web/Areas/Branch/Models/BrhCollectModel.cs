using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Web.Areas.Branch.Models
{
    public partial class BrhCollectModel
    {
        [Display(Name = "类别")]
        public string Type { get; set; }
        [Display(Name = "金额")]
        public decimal Amount { get; set; }
        [Display(Name = "数量")]
        public int Count { get; set; }
    }
}
