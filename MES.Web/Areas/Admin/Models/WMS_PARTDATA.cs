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
        [Required, Display(Name = "�Ϻ�")]
        [StringLength(20)]
        public string PARTNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required, Display(Name = "�汾")]
        [StringLength(20)]
        public string PARTVER { get; set; }

        [Display(Name = "����")]
        public string DESCRIPTION { get; set; }


        [Display(Name = "�������")]
        public string PARTGRPNO2{ get; set; }

        [NotMapped]
        [Display(Name = "�Ϻ�")]
        public String CodeName { get{ return String.Format("{0}-{1}", PARTNO, DESCRIPTION);} }
    }
}
