using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MES.Web.Models
{
    [Table("ENG_PRDLINE")]
    public class LineModel
    {
        [Key]
        [Column("LineName")]
        public String Name { get; set; }
        [Column("DISPLAYNAME")]
        public String DispName { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        [NotMapped]
        public String Userid { get; set; }
    }
    [Table("ENG_LINEOP")]
    public class OpModel
    {
        [Key, Column("LINENAME", Order =1)]
        public String Line { get; set; }
        [Key, Column("L_OPNO", Order = 2)]
        public String Op { get; set; }
        [Column("DISPLAYNAME")]
        public String Name { get; set; }
    }

    [Table("ENG_LINESTATION")]
    public class StnModel
    {
        [Key, Column("LINENAME", Order = 1)]
        public String Line { get; set; }
        [Key, Column("L_OPNO", Order = 2)]
        public String Op { get; set; }
        [Key, Column("L_STNO", Order = 3)]
        public String Stn { get; set; }
        [Column("DISPLAYNAME")]
        public String Name { get; set; }

    }
}