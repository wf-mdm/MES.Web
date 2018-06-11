using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MES.Web.Areas.Admin.Models
{
    public partial class ENG_LINEPARTCONF
    {
        [Key]
        [Column(Order =0)]
        [Required, Display(Name = "产线代码")]
        [StringLength(20)]
        public String LINENAME { get; set; }

        [Display(Name = "产线")]
        [ForeignKey("LINENAME")]
        public virtual ENG_PRDLINE Line { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required, Display(Name = "料号")]
        [StringLength(20)]
        public String PARTNO { get; set; }

        [Column]
        [Required, Display(Name = "版本")]
        [StringLength(20)]
        public String PARTVER { get; set; }

        [Required, Display(Name = "配置方式")]
        [StringLength(1)]
        public String SETTINGType { get; set; }

        [Display(Name = "标准产能")]
        public Decimal STDYIELDRATE { get; set; }

        [Display(Name = "预设包装箱类型")]
        public String CONTNRTYPE { get; set; }

        [Display(Name = "预设工艺路线")]
        public String RT_NAME { get; set; }

        [Display(Name = "标准人员配备人数")]
        public Decimal STDHEADCOUNTS { get; set; }

        [Display(Name = "预设质量规范")]
        public String DEFAULT_CONFNAME { get; set; }
    }
}