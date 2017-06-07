namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_USERANDROLES
    {
        [Key]
        [StringLength(20)]
        public string OPERID { get; set; }

        [StringLength(80)]
        public string OPERNAME { get; set; }

        [NotMapped]
        public String Name { get { return String.Format("{0}:{1}", OPERID, OPERNAME); } }
    }
}
