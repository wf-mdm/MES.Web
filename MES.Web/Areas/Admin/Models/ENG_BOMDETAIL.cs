namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ENG_BOMDETAIL
    {
        public ENG_BOMDETAIL()
        {
            UPDATETIME = DateTime.Now;
        }
        [Key]
        [Column(TypeName = "numeric")]
        public decimal ITEMID { get; set; }

        [Required]
        [StringLength(30)]
        public string PARTNO { get; set; }

        [StringLength(4)]
        public string PARTVER { get; set; }

        [StringLength(10)]
        [Required, Display(Name = "工序号")]
        public string L_OPNO { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "子件料号")]
        public string COMP_PARTNO { get; set; }

        [StringLength(255)]
        [Display(Name = "名称")]
        public string DESCRIPTION { get; set; }
        [Column(TypeName = "numeric")]
        [Display(Name = "单耗量")]
        public decimal? UNITCONSUMEQTY { get; set; }
        [StringLength(1)]
        [Display(Name = "是否批次管理")]
        public string LotControl { get; set; }

        [StringLength(1)]
        [Display(Name = "序列号管理")]
        public string SERIALCONTROL { get; set; }

        [StringLength(1)]
        [Display(Name = "主要关键件")]
        public string IsKeyID { get; set; }
        [StringLength(1)]
        [Display(Name = "数量控制")]
        public string CNTCONTROL { get; set; }
        [StringLength(1)]
        [Display(Name = "是否半成品")]
        public string ISSEMI { get; set; }

        [StringLength(12)]
        [Display(Name = "半成品来源流程段")]
        public string SEMILINEGRP { get; set; }

        public DateTime UPDATETIME { get; set; }
        /*
            [StringLength(1)]
            public string ITEMTYPE { get; set; }

            [StringLength(30)]
            public string ORG_PARTNO { get; set; }

            [StringLength(4)]
            public string COMP_PARTVER { get; set; }

            [StringLength(30)]
            public string SN_PATTERN { get; set; }

            [Column(TypeName = "numeric")]
            public decimal? REPEATLIMIT { get; set; }

            [StringLength(255)]
            public string NLANG_DESC { get; set; }

            [StringLength(12)]
            public string COMP_UNIT { get; set; }


            [Column(TypeName = "numeric")]
            public decimal? DEFAULTLOTQTY { get; set; }

            [Column(TypeName = "numeric")]
            public decimal? WARNINGQTY { get; set; }

            [Column(TypeName = "numeric")]
            public decimal? BLOCKQTY { get; set; }

            [Column(TypeName = "numeric")]
            public decimal? WARNTIMES { get; set; }

            [Column(TypeName = "numeric")]
            public decimal? BUFFERQTY { get; set; }

            [StringLength(1)]
            public string EMPTYRUN { get; set; }

            */
    }
}
