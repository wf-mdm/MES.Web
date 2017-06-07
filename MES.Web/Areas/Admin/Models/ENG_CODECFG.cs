namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ENG_CODECFG
    {
        public String Name { get { return String.Format("{0}:{1}", CODEID, CODEDESC); } }
        [Key]
        [Column(Order = 0)]
        [StringLength(80)]
        public string CODENAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(80)]
        public string CODEID { get; set; }
        [StringLength(255)]
        public string CODEDESC { get; set; }
/*
        [StringLength(255)]
        public string CODEVAL { get; set; }


        [StringLength(255)]
        public string COMMENT { get; set; }
*/
    }
}
