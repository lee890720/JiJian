﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Branch
{
    [Table("Brh_ConnectRecord")]
    public partial class BrhConnectRecord
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ConnectRecordId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        [Display(Name = "录入时间")]
        public DateTime EnteringDate { get; set; }
        [Display(Name = "早班人员")]
        public string MorningStaff { get; set; }
        [Display(Name = "晚班人员")]
        public string NigthStaff { get; set; }
        [Required]
        [Display(Name = "单据数量")]
        public int BillCount { get; set; }
        [Required]
        [Display(Name = "房费现金")]
        public decimal HouseCash { get; set; }
        [Required]
        [Display(Name = "其他现金")]
        public decimal OtherCash { get; set; }
        [Required]
        [Display(Name = "房卡数量")]
        public int CardCount { get; set; }
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
