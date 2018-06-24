using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace MES.Web.Areas.Admin.Models
{
    public class ENG_LINEOPPARTCONF
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        [Display(Name = "产线")]
        public string LINENAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [Display(Name = "料号")]
        public string PARTNO { get; set; }

        [Display(Name = "配置方式")]
        public string SETTINGType { get; set; }

        [Display(Name = "工序代码")]
        public string L_OPNO { get; set; }

        [Display(Name = "工站代码")]
        public string L_STNO { get; set; }

        [Display(Name = "标准产能")]
        public decimal STDYIELDRATE { get; set; }

        [Display(Name = "生产准备时间")]
        public decimal PREPARETIME { get; set; }

        [Display(Name = "清线时间")]
        public decimal POSTTIME { get; set; }

        [Display(Name = "节拍时间")]
        public decimal CYCLETIME { get; set; }

        [Display(Name = "标准人员配置数量")]
        public decimal STDHEADS { get; set; }

        [Display(Name = "关键料及ID配置")]
        public String IDCFGLIST { get; set; }

        [Display(Name = "备注")]
        public decimal COMMENT { get; set; }

        [Display(Name = "不良申报自动解绑的关联料及ID清单")]
        public decimal NGIDUNBINDLST { get; set; }

        [Display(Name = "属性")]
        public decimal PROPLIST { get; set; }
    }
}