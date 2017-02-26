using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
            return View(db.Presupuestos.Where(x => x.Solicitudes.Id_Solicitud == id && x.Estado!="Aceptado").ToList().OrderByDescending(x=>x.Hora));
        }

        [HttpGet]
        public ActionResult SolicitudesAResponder()
        {
            List<Solicitudes> list = new List<Solicitudes>();

            var IdServi = User.Identity.GetUserId();

            var servi = db.Servis.FirstOrDefault(x => x.Id == IdServi);

            if (servi.ServisProfesiones != null)
            {
                foreach (var profesion in servi.ServisProfesiones.Select(x => x.Profesion))
                {
                    var filter = db.Solicitudes.Where(x => x.Profesiones.Id_Profesion == profesion.Id_Profesion && x.Estado!="Realizado");
                    list.AddRange(filter);
                }
            }

            if (servi.ServisTareas != null)
            {
                List<Solicitudes> tfilter = new List<Solicitudes>();
                foreach (var tarea in servi.ServisTareas.Select(x => x.Tarea))
                {
                    var filter = list.Where(x => x.Tareas.Id_Tarea == tarea.Id_Tarea);

                    tfilter.AddRange(filter);
                }

                list = tfilter;
            }

            return View(list);

        }

        // GET: Presupuestos
        public ActionResult Index()
        {
            var IdServi = User.Identity.GetUserId();
            var servi = db.Servis.FirstOrDefault(x => x.Id == IdServi);

            if (servi != null)
            {
                var list = db.Presupuestos.Where(x => x.Servis.Any(y=>y.Id==servi.Id) ).ToList().OrderByDescending(x=>x.Hora);

                return View(list);
            }
            else
            {
                return View();
            }
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
            if (ModelState.IsValid && User.Identity.GetUserId() != null)
            {
                var IdServi = User.Identity.GetUserId();
                var servi = db.Servis.Where(x => x.Id == IdServi).FirstOrDefault();
                var solicitud = db.Solicitudes.Find(viewModel.CrearPrespuesto.idSolicitud);
                var solMod = CambiaEstado(solicitud);

                try
                {
                    // TODO: Add insert logic here
                    var presupuesto = new Presupuestos
                    {
                        Estado = "Presupuestado",
                        Hora = viewModel.CrearPrespuesto.hora,
                        Fecha_Vencimiento = DateTime.Now.AddDays(3),
                        Observacion = viewModel.CrearPrespuesto.observaciones,
                        Precio = viewModel.CrearPrespuesto.precio,
                        Solicitudes = solicitud,
                    };

                    presupuesto.Servis.Add(servi);

                    var userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

                    userManager.SendEmail(solicitud.Usuarios.Id, "Respondieron a Tu Solicitud!", "Uno de nuestros capacitados Servis a realizado un presupuesto par tu Solicitud! /n Ingresa a ServiEnCasa para verlo. /Exitos!!!");

                    db.Entry(solMod).State = System.Data.Entity.EntityState.Modified;

                    db.Presupuestos.Add(presupuesto);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }

            return RedirectToAction("Index");
        } 

        public Solicitudes CambiaEstado(Solicitudes _solicitud)
        {
            _solicitud.Contador = _solicitud.Contador + 1;
            _solicitud.Estado = "Presupuestado";

            return (_solicitud);
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
