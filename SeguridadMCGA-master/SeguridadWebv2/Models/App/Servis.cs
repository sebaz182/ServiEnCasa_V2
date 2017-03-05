using SeguridadWebv2.Patterns;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.App
{
    [Table("Servis")]
    public class Servis : ApplicationUser
    {
        public Servis()
        {
            this.Presupuestos = new HashSet<Presupuestos>();
            this.CuentaCorriente = new HashSet<CuentaCorriente>();
            this.ServisTareas = new HashSet<ServisTareas>();
            this.ServisProfesiones = new HashSet<ServisProfesiones>();

        }
        
        public string DNI { get; set; }
        public string Matricula { get; set; }
        public string Foto { get; set; }
        public int? CalTareas { get; set; }




        public virtual ICollection<ServisProfesiones> ServisProfesiones { get; set; }
        public virtual ICollection<ServisTareas> ServisTareas { get; set; }
        public virtual ICollection<Presupuestos> Presupuestos { get; set; }
        public virtual ICollection<CuentaCorriente> CuentaCorriente { get; set; }
    }
}