namespace MES.Web.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HR_PERMISSIONS
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string USERID { get; set; }

        [Required]
        [StringLength(1)]
        public string USERTYPE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(80)]
        public string MODULEID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(80)]
        public string ITEMID { get; set; }

        [StringLength(12)]
        public string PERMISSIONS { get; set; }
    }
}
