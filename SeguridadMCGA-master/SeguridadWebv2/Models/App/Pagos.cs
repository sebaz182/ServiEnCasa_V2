using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace SeguridadWebv2.Models.App
{
    [Table("Pagos")]
    public class Pagos
    {
        [Key]
        public int idPago { get; set; }
        public string MPRefID { get; set; }
        public string Estado { get; set; }
        public decimal Importe { get; set; }
    }
}