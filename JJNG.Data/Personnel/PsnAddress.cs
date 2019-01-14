using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Personnel
{
    [Table("Psn_Address")]
    public partial class PsnAddress
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }
        public int AddressAccountId { get; set; }
        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Display(Name = "用途")]
        public string Purpose { get; set; }
        [Display(Name = "手机号")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "录单人")]
        public string EnteringStaff { get; set; }
        [Required]
        [Display(Name = "分店")]
        public string Branch { get; set; }
        [Display(Name = "备注")]
        public string Note { get; set; }
        public PsnAddressAccount PsnAddressAccount { get; set; }
    }
}
