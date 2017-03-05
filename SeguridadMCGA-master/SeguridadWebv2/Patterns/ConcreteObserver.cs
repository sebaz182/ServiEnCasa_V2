using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeguridadWebv2.Models.App;

namespace SeguridadWebv2.Patterns
{
    class ConcreteObserver : Observer
    {
        //private Servis _servi;

        public override void Actualizacion(Servis servi)
        {
            CuentaCorriente ultimoCredit = null;
            CuentaCorriente ultimoDebito = null;

            var list = servi.CuentaCorriente.ToList();
            var listaCreditos = list.OrderByDescending(x => x.Fecha).Where(x => x.Credito > 0).ToList();
            var listaDebitos = list.OrderByDescending(x => x.Fecha).Where(x => x.Debito > 0).ToList();
            decimal? deuda = 0;

            foreach (var cred in listaCreditos)
            {
                deuda = deuda + cred.Credito;
            }
            foreach (var deb in listaDebitos)
            {
                deuda = deuda - deb.Debito;
            }

            if (listaCreditos != null)
            {
                ultimoCredit = listaCreditos.FirstOrDefault();
            }

            if (listaDebitos != null)
            {
                ultimoDebito = listaDebitos.FirstOrDefault();
            }

            if (ultimoDebito != null && ultimoCredit != null)
            {
                TimeSpan ts = ultimoDebito.Fecha - ultimoCredit.Fecha;

                if (ts.Days >= 30 && deuda < 0) 
                {
                    servi.Estado = false;
                } 
            }
        }

    } 
}