﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JJNG.Data.Branch
{
    [Table("Brh_ImprestAccounts")]
    public partial class BrhImprestAccounts
    {
        public BrhImprestAccounts()
        {
            BrhImprestRecord = new HashSet<BrhImprestRecord>();
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ImprestAccountsId { get; set; }

        [Display(Name = "备用金名称")]
        public string ImprestAccountsName { get; set; }
        [Display(Name = "初始额度")]
        public decimal Balance { get; set; }
        [Display(Name = "剩余额度")]
        public decimal Equity { get; set; }
        [Display(Name = "未转额度")]
        public decimal MoveAmount { get; set; }
        [Display(Name = "管理人")]
        public string Manager { get; set; }
        [Display(Name = "部门")]
        public string Department { get; set; }
        [Display(Name = "隶属")]
        public string Branch { get; set; }
        [Display(Name = "用途")]
        public PurposeType Purpose { get; set; }

        public ICollection<BrhImprestRecord> BrhImprestRecord { get; set; }
    }
    public enum PurposeType
    {
        默认 = 0,
        线上推广 = 1
    }
}
