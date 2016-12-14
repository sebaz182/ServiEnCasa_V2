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
            this.Profesiones = new HashSet<Profesiones>();
            this.Presupuestos = new HashSet<Presupuestos>();
            this.Tareas = new HashSet<Tareas>();
            this.CuentaCorriente = new HashSet<CuentaCorriente>();
        }
        
        public string DNI { get; set; }
        public string Matricula { get; set; }
        public string Foto { get; set; }
        
        public virtual ICollection<Profesiones> Profesiones { get; set; }
        public virtual ICollection<Presupuestos> Presupuestos { get; set; }
        public virtual ICollection<Tareas> Tareas { get; set; }
        public virtual ICollection<CuentaCorriente> CuentaCorriente { get; set; }
    }
}