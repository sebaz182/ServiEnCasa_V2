using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.App
{
    [Table("ServisTareas")]
    public class ServisTareas
    {
        [Key]
        public int id { get; set; }
        public virtual Servis Servi{ get; set; }
        public virtual Tareas Tarea { get; set; }
    }
}