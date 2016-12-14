using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.App
{
    [Table("Solicitudes")]
    public class Solicitudes
    {
        public Solicitudes()
        {
            this.Contador = 0;
            this.Presupuestos = new HashSet<Presupuestos>();
        }

        [Key]
        public int Id_Solicitud { get; set; }
        public string Estado { get; set; }
        public int Contador { get; set; }
        public string Foto { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Desc_Solicitud { get; set; }

        public virtual Tareas Tareas { get; set; }
        public virtual Profesiones Profesiones { get; set; }
        public virtual ICollection<Presupuestos> Presupuestos { get; set; }
        public virtual Zonas Zonas { get; set; }
        public virtual ApplicationUser Usuarios { get; set; }
        public virtual Horarios Horarios { get; set; }
    }
}