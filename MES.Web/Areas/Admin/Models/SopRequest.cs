using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Web.Areas.Admin.Models
{
    public class SopRequest
    {
        [Required, Display(Name = "产线")]
        public String LINE { get; set; }
        [Required, Display(Name = "工站")]
        public String STN { get; set; }
        [Display(Name = "料号")]
        public String PARTNO { get; set; }

        [DataType(DataType.Upload)]
        [Required,Display(Name = "SOP文件")]
        public HttpPostedFileBase FILE { get; set; }
    }
}