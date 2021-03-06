﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.App
{
    [Table("CuentaCorriente")]
    public class CuentaCorriente
    {
        public CuentaCorriente()
        {
            this.Credito = 0m;
            this.Debito = 0m;
        }

        [Key]
        public int Id_CtaCorriente { get; set; }
        public Nullable<decimal>  Credito { get; set; }
        public Nullable<decimal> Debito { get; set; }
        public string Detalle { get; set; }
        public DateTime Fecha { get; set; }

        public virtual Servis Servis { get; set; }
    }
}