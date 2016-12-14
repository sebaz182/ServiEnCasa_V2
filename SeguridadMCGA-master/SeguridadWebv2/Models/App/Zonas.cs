using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.App
{
    [Table("Zonas")]
    public class Zonas
    {
        public Zonas()
        {
            this.Solicitudes = new HashSet<Solicitudes>();
        }
        [Key]
        public int Id_Zona { get; set; }
        public string Zona { get; set; }
        
        public virtual ICollection<Solicitudes> Solicitudes { get; set; }
    }
}