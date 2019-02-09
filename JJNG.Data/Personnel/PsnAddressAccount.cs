using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Personnel
{
    [Table("Psn_AddressAccount")]
    public partial class PsnAddressAccount
    {
        public PsnAddressAccount()
        {
            PsnAddress = new HashSet<PsnAddress>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AddressAccountId { get; set; }

        [Display(Name = "名称")]
        public string AccountName { get; set; }
        [Display(Name = "管理人")]
        public string Manager { get; set; }
        [Display(Name = "部门")]
        public string Department { get; set; }
        [Display(Name = "隶属")]
        public string Branch { get; set; }

        public ICollection<PsnAddress> PsnAddress { get; set; }
    }
}
