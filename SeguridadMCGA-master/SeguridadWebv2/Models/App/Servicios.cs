using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.App
{
    [Table("Servicios")]
    public class Servicios
    {
        public Servicios()
        {
            this.Calificaciones = new HashSet<Calificaciones>();
        }

        [Key]
        public int Id_Servicio { get; set; }
        public string Estado { get; set; }

        public virtual Presupuestos Presupuestos { get; set; }
        public virtual ICollection<Calificaciones> Calificaciones { get; set; }
    }
}