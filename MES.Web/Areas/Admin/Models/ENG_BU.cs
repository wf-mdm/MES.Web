namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ENG_BU
    {
        [Key]
        [StringLength(12)]
        [Column(Order = 0)]
        [Display(Name = "公司代码")]
        public string BUNO { get; set; }

        [StringLength(255)]
        [Column(Order = 1)]
        [Display(Name = "公司")]
        public string BUNAME { get; set; }
/*
        [StringLength(20)]
        public string COGRPNO { get; set; }
        [StringLength(255)]
        public string CO_NAME { get; set; }

        [StringLength(255)]
        public string WEBSITE { get; set; }

        [StringLength(80)]
        public string TELNO { get; set; }

        [StringLength(80)]
        public string FAXNO { get; set; }

        [StringLength(3999)]
        public string BUINFO { get; set; }

        [StringLength(255)]
        public string BUOWNER { get; set; }

        [StringLength(255)]
        public string COMMENTS { get; set; }
*/
    }
}
