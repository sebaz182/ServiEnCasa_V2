using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.App
{
    [Table("ComisionAuditoria")]
    public class ComisionAuditoria
    {
        public ComisionAuditoria()
        {

        }

        [Key]
        public int Id_ComisionAuditoria { get; set; }
        public decimal ImpComision { get; set; }
        public DateTime FechaAlta { get; set; }
        public string UsuarioAlta { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; }
    }
}