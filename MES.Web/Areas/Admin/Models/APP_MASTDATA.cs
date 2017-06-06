namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class APP_MASTDATA
    {
        [Key]
        [StringLength(80)]
        public string APP_ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? APP_SEQ { get; set; }

        [StringLength(255)]
        public string APP_DESCRIPTION { get; set; }

        [StringLength(1024)]
        public string APP_PATH { get; set; }

        [StringLength(512)]
        public string FILENAME { get; set; }

        [StringLength(512)]
        public string INDICTOR { get; set; }

        [StringLength(512)]
        public string TAG { get; set; }

        [StringLength(512)]
        public string ATTR { get; set; }

        [StringLength(160)]
        public string MODTYPE { get; set; }

        [StringLength(80)]
        public string PARENT_ID { get; set; }

        [StringLength(255)]
        public string COMMENTS { get; set; }

        [StringLength(1)]
        public string ACTIVE { get; set; }
    }
}
