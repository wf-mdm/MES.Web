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
    }
}