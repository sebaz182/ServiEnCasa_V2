using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.App
{
    [Table("Horarios")]
    public class Horarios
    {
        public Horarios()
        {
            this.Solicitudes = new HashSet<Solicitudes>();
        }
        [Key]
        public int Id_Horario { get; set; }
        public string Horario { get; set; }
        
        public virtual ICollection<Solicitudes> Solicitudes { get; set; }
    }
}