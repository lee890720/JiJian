﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Finance
{
    [Table("Fnc_ChannelType")]
    public partial class FncChannelType
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "渠道")]
        public string ChannelType { get; set; }

        [Display(Name = "顺序")]
        public int Sequence { get; set; }

        [Display(Name = "颜色")]
        public string Color { get; set; }
    }
}
