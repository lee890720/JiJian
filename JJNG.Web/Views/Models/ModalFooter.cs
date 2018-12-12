using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JJNG.Web.Models
{
    public class ModalFooter
    {
        public string SubmitButtonText { get; set; } = "提交";
        public string CancelButtonText { get; set; } = "取消";
        public string SubmitButtonID { get; set; } = "btn-submit";
        public string CancelButtonID { get; set; } = "btn-cancel";
        public bool OnlyCancelButton { get; set; }
    }
}
