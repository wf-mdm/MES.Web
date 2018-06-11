namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WMS_PARTDATA
    {
        [Key]
        [Column(Order = 0)]
        [Required, Display(Name = "料号")]
        [StringLength(20)]
        public string PARTNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required, Display(Name = "版本")]
        [StringLength(20)]
        public string PARTVER { get; set; }

        [Display(Name = "描述")]
        public string DESCRIPTION { get; set; }


        [Display(Name = "物料类别")]
        public string PARTGRPNO2{ get; set; }

        [NotMapped]
        [Display(Name = "料号")]
        public String CodeName { get{ return String.Format("{0}-{1}", PARTNO, DESCRIPTION);} }
    }
}
