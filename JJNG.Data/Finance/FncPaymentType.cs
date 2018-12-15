using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Finance
{
    [Table("Fnc_PaymentType")]
    public partial class FncPaymentType
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "付款方式")]
        public string PaymentType { get; set; }

        [Display(Name = "顺序")]
        public int  Sequence { get; set; }
    }
}
