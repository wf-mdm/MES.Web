namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HR_OPERCAPBMATRIX
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string OPERID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string LINEName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string L_OPNO { get; set; }

        [StringLength(10)]
        public string L_STNO { get; set; }

        [StringLength(3)]
        public string CAPBLEVEL { get; set; }

        [StringLength(255)]
        public string COMMENTS { get; set; }
    }
}
