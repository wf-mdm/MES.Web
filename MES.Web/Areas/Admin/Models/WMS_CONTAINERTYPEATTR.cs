using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MES.Web.Areas.Admin.Models
{
    public partial class WMS_CONTAINERTYPEATTR
    {
        [Key, Column(Order=0)]
        [Required, Display(Name = "包装类型")]
        [StringLength(20)]
        public String CONTNRTYPE{ get; set; }

        [Key, Column(Order = 1)]
        [Display(Name = "属性名")]
        public String PROPERTYNAME { get; set; }
        
        [Display(Name = "属性值")]
        public String PROPERTYVALUE { get; set; }
    }
}