﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Branch
{
    [Table("Brh_ImprestRecord")]
    public partial class BrhImprestRecord
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ImprestRecordId { get; set; }
        public int ImprestAccountsId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "录入时间")]
        public DateTime EnteringDate { get; set; }
        [Display(Name = "分类")]
        public string ExpendType { get; set; }
        [Display(Name = "用途")]
        public string Purpose { get; set; }
        [Required]
        [Display(Name = "金额")]
        public decimal Amount { get; set; }
        [Required]
        [Display(Name = "付款方式")]
        public string PaymentType { get; set; }
        [Display(Name = "对应审批单号")]
        public string ConnectNumber { get; set; }
        [Required]
        [Display(Name = "录单人")]
        public string EnteringStaff { get; set; }
        [Required]
        [Display(Name = "财务审核")]
        public bool IsFinance { get; set; }
        [Display(Name = "是否转移")]
        public bool IsMove { get; set; }
        [Required]
        [Display(Name = "分店")]
        public string Branch { get; set; }
        [Display(Name = "备注")]
        public string Note { get; set; }
        public BrhImprestAccounts BrhImprestAccounts { get; set; }
    }
}
