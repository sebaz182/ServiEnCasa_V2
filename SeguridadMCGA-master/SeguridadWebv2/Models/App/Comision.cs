using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.App
{
    [Table ("Comision")]
    public class Comision
    {
        public Comision()
        {

        }

        [Key]
        public int Id_Comision { get; set; }
        public decimal ImpComision { get; set; }

    }
}