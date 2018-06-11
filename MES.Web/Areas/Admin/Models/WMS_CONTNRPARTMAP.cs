using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MES.Web.Areas.Admin.Models
{
    public partial class WMS_CONTNRPARTMAP
    {
        [Key]
        [Required, Display(Name = "包装类型")]
        public String CONTNRTYPE{ get; set; }

        [Display(Name = "物料类别")]
        public String PARTGRP2 { get; set; }

        [Display(Name = "可装最大件数")]
        public decimal VOLUMEQTY { get; set; }

        [Display(Name = "计件单位")]
        public String VOL_UNIT { get; set; }

        [Display(Name = "小包数")]
        public decimal CONTAINSUBQTY { get; set; }

        [Display(Name = "允许超出数")]
        public decimal OVERLIMIT { get; set; }

        [Display(Name = "数量控制模式")]
        public String CONTRLBY { get; set; }

        [Display(Name = "小包类型")]
        public String SUBCONTNRTYPE { get; set; }
    }
}