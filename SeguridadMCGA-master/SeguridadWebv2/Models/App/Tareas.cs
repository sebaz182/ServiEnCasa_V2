using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeguridadWebv2.Models.App
{
    [Table("Tareas")]
    public class Tareas
    {
        public Tareas()
        {
            this.Solicitudes = new HashSet<Solicitudes>();
            this.ServisTareas = new HashSet<ServisTareas>();
        }

        [Key]
        public int Id_Tarea { get; set; }
        public string Desc_Tarea { get; set; }
        
        public virtual ICollection<Solicitudes> Solicitudes { get; set; }
        public virtual Profesiones Profesiones { get; set; }
        public virtual ICollection<ServisTareas> ServisTareas { get; set; }
    }
}