using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Finance
{
    [Table("Fnc_HouseType")]
    public partial class FncHouseType
    {
        public FncHouseType()
        {
            FncHouseNumber = new HashSet<FncHouseNumber>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int HouseTypeId { get; set; }
        public int BranchId { get; set; }

        [Display(Name = "房型")]
        public string HouseType { get; set; }
        public string Order { get; set; }
        public string OTAOrder1 { get; set; }
        public string OTAOrder2 { get; set; }
        public bool IsReal { get; set; }
        [Display(Name = "预付价")]
        public int OTAPre { get; set; }
        [Display(Name = "底价基数")]
        public double OTABase { get; set; }
        [Display(Name = "现付基数")]
        public int OTASpot { get; set; }
        [Display(Name = "门店价基数")]
        public int StickerPrice { get; set; }
        [Display(Name = "合作价基数")]
        public double CooperationPrice { get; set; }
        [Display(Name = "旺季基数")]
        public double PeakPrice { get; set; }
        public FncBranch FncBranch { get; set; }
        public ICollection<FncHouseNumber> FncHouseNumber { get; set; }

    }
}
