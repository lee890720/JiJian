using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Finance
{
    [Table("Fnc_HouseNumber")]
    public partial class FncHouseNumber
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int HouseNumberId { get; set; }
        public int HouseTypeId { get; set; }

        [Display(Name = "房号")]
        public string HouseNumber { get; set; }
        public FncHouseType FncHouseType { get; set; }
    }
}
