using Microsoft.AspNet.Identity;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    public class PresupuestosController : Controller
    {
        private ModeloContainer db = new ModeloContainer();

        [HttpGet]
        public ActionResult PresupuestosSolicitud(int id)
        {
            return View(db.Presupuestos.Where(x=>x.Solicitudes.Id_Solicitud==id).ToList());
        }

        [HttpGet]
        public ActionResult SolicitudesAResponder()
        {
            //var IdUsuario = User.Identity.GetUserId();
            //var usuario = db.Users.Where(x => x.Id == IdUsuario).FirstOrDefault();

            //var servis = db.Servis.ToList();

            //var lista = db.Solicitudes.Where(x => x.Profesiones.Id_Profesion == Servis).ToList();


            //return View(lista);

            return View(db.Solicitudes.Where(x => x.Estado != "Realizado" && x.Contador == 0).ToList());
        }

        // GET: Presupuestos
        public ActionResult Index()
        {
            return View(db.Presupuestos.ToList());
        }

        // GET: Presupuestos/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Presupuestos/Contratar
        public ActionResult Contratar()
        {
            return View();
        }

        // POST: Presupuestos/Contratar
        [HttpPost]
        public ActionResult Contratar(int idPresupuesto)
        {
            try
            {
                var servicio = new Servicios();
                servicio.Estado = "Contratado";
                servicio.Presupuestos.Id_Presupuesto = idPresupuesto;
                db.Servicios.Add(servicio);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Presupuestos/Create
        public ActionResult Create(int id)
        {
            var solicitud = db.Solicitudes.Where(x => x.Id_Solicitud == id).FirstOrDefault();
            var presupuesto = new GeneralPresupuestoVM
            {
                Solicitud = solicitud,
                CrearPrespuesto = new CrearPresupuestoViewModels()
            };
            return View(presupuesto);
        }

        // POST: Presupuestos/Create
        [HttpPost]
        public ActionResult Create(GeneralPresupuestoVM viewModel)
        {
            var servi = db.Servis.Where(x => x.Id != null).SingleOrDefault();

            try
            {
                // TODO: Add insert logic here
                var presupuesto = new Presupuestos();
                presupuesto.Estado = "Presupuestado";
                presupuesto.Hora = viewModel.CrearPrespuesto.hora;
                presupuesto.Fecha_Vencimiento =DateTime.Now.AddDays(3);
                presupuesto.Observacion = viewModel.CrearPrespuesto.observaciones;
                presupuesto.Precio = viewModel.CrearPrespuesto.precio;
                var solicitud = db.Solicitudes.Find(viewModel.CrearPrespuesto.idSolicitud);
                presupuesto.Solicitudes = solicitud;
                presupuesto.Servis.Add(servi);

                solicitud.Contador = solicitud.Contador + 1;
                solicitud.Estado = "Presupuestado";
                
                db.Entry(solicitud).State = System.Data.Entity.EntityState.Modified;

                db.Presupuestos.Add(presupuesto);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        // GET: Presupuestos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Presupuestos/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Presupuestos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Presupuestos/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
