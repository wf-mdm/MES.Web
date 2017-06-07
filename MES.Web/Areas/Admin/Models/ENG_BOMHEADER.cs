namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ENG_BOMHEADER
    {
        public static String DEFAULT_PARTVER = "0";
        public ENG_BOMHEADER()
        {
            PARTVER = DEFAULT_PARTVER;
        }

        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        [Required, Display(Name = "料号")]
        public string PARTNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string PARTVER { get; set; }

        [StringLength(12)]
        [Required, Display(Name = "工艺路线")]
        public string RT_NAME { get; set; }

        [StringLength(20)]
        [Required, Display(Name = "工艺参数集")]
        public string DEFAULT_CONFNAME { get; set; }

        [StringLength(255)]
        [Required, Display(Name = "名称")]
        public string DESCRIPTION { get; set; }

/*
        [StringLength(1)]
        public string BOMActive { get; set; }
        [StringLength(255)]
        public string NLANG_DESC { get; set; }

        [StringLength(30)]
        public string SN_PATTERN { get; set; }

        [StringLength(12)]
        public string PRDUNIT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LOTQTY { get; set; }

        [StringLength(1)]
        public string LotControl { get; set; }

        [StringLength(1)]
        public string SERIALCONTROL { get; set; }

        [StringLength(30)]
        public string AUTOWOFMT { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DEFAULTLOTQTY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WARNINGQTY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? BLOCKQTY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? REPEATLIMIT { get; set; }

        public DateTime? CREATETIME { get; set; }

        public DateTime UPDATETIME { get; set; }

        [StringLength(30)]
        public string SN_AUTOFMT { get; set; }

        [StringLength(1)]
        public string ISVSN { get; set; }

        [StringLength(30)]
        public string CONTNRTYPE { get; set; }

        [StringLength(30)]
        public string PARTGRP2 { get; set; }

        [StringLength(50)]
        public string CUSTPARTNO { get; set; }

        [StringLength(50)]
        public string CUSTNAME { get; set; }

        [StringLength(100)]
        public string CUSTDESC { get; set; }

        [StringLength(100)]
        public string CUSTCOMMENT { get; set; }

        [StringLength(100)]
        public string LOTNOFMT { get; set; }

        [StringLength(1)]
        public string EmptyRun { get; set; }
*/
    }
}
