using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.App
{
    [Table("Profesiones")]
    public class Profesiones
    {
        public Profesiones()
        {
            this.Servis = new HashSet<Servis>();
            this.Solicitudes = new HashSet<Solicitudes>();
            this.Tareas = new HashSet<Tareas>();
        }
        [Key]
        public int Id_Profesion { get; set; }
        public string Desc_Profesion { get; set; }
        
        public virtual ICollection<Servis> Servis { get; set; }
        public virtual ICollection<Solicitudes> Solicitudes { get; set; }
        public virtual ICollection<Tareas> Tareas { get; set; }
    }
}