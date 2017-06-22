using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Web.Areas.Rpt.Models
{
    public class TraceabilityRequest
    {
        [Display(Name = "工单号")]
        public String Wo { get; set; }
        [Display(Name = "序列号/批次")]
        public String Sn { get; set; }
        [Display(Name = "包装号")]
        public String Pack { get; set; }
        [Display(Name = "日期时间")]
        public DateTime Dt1 { get; set; }
        [Display(Name = "~")]
        public DateTime Dt2 { get; set; }
        [Display(Name = "反追溯?")]
        public Boolean IsRevert { get; set; }
    }
}