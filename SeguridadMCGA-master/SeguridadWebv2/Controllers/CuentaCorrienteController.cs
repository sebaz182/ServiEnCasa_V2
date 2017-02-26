using Microsoft.AspNet.Identity;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mercadopago;
using System.Collections;

namespace SeguridadWebv2.Controllers
{
    public class CuentaCorrienteController : Controller
    {
        private ModeloContainer db = new ModeloContainer();

        public ActionResult Index() 
        { 
            var idServi = User.Identity.GetUserId();
            var movimientos = db.CuentaCorriente.Where(x => x.Servis.Id == idServi).ToList().OrderByDescending(z=>z.Fecha);
            return View(movimientos);
        }

        public ActionResult RealizarPago(string idServi) {

            var movimientos = db.CuentaCorriente.Where(x => x.Servis.Id == idServi).ToList();

            return View();
        }

        public void _generarDebito(string id, string detalle)
        {
            var _cuentaCorriente = new CuentaCorriente();

            _cuentaCorriente.Servis = db.Servis.Where(x => x.Id == id).FirstOrDefault();
            _cuentaCorriente.Detalle = "Comisión por Servicio N°: " + detalle;
            _cuentaCorriente.Credito = 0;
            _cuentaCorriente.Debito = db.Comision.FirstOrDefault().ImpComision;
            _cuentaCorriente.Fecha = DateTime.Today;

            db.CuentaCorriente.Add(_cuentaCorriente);

            db.SaveChanges();
        }
    }
}