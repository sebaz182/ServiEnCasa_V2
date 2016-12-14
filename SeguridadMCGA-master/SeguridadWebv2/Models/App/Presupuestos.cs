using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.App
{
    [Table("Presupuestos")]
    public class Presupuestos
    {
        public Presupuestos()
        {
            this.Servis = new HashSet<Servis>();
            this.Servicios = new HashSet<Servicios>();
        }

        [Key]
        public int Id_Presupuesto { get; set; }
        public string Estado { get; set; }
        public System.DateTime Fecha_Vencimiento { get; set; }
        public int Precio { get; set; }
        public string Observacion { get; set; }
        public System.DateTime Hora { get; set; }
        
        public virtual ICollection<Servis> Servis { get; set; }
        public virtual Solicitudes Solicitudes { get; set; }
        public virtual ICollection<Servicios> Servicios { get; set; }
    }
}