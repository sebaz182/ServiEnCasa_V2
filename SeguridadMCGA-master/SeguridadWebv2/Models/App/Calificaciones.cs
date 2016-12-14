using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.App
{
    [Table("Calificaciones")]
    public class Calificaciones
    {
        public Calificaciones()
        {

        }

        [Key]
        public int Id_Calificacion { get; set; }
        public string Obs_Servi { get; set; }
        public string Obs_Usuario { get; set; }
        public Nullable<bool> Cumplimiento { get; set; }
        public Nullable<int> Cal_Servi { get; set; }
        public Nullable<int> Cal_Usuario { get; set; }
        public Nullable<int> Cal_Trabajo { get; set; }

        public virtual Servicios Servicios { get; set; }
    }
}