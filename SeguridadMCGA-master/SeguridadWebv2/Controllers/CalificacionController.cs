using SeguridadWebv2.Models;
using SeguridadWebv2.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    public class CalificacionController : Controller
    {
        private ModeloContainer db = new ModeloContainer();
        private CuentaCorrienteController _CuentaCorriente = new CuentaCorrienteController();

        [HttpGet]
        public ActionResult CalificarUser(int id, int idCal)
        {
            var calificaciones = new ClasificacionesViewModel();          

            calificaciones.cal_usuario = 0;

            calificaciones.pago = true;

            calificaciones.idCal = idCal;

            calificaciones.Servicio = db.Servicios.FirstOrDefault(x => x.Id_Servicio == id);

            return View(calificaciones);
        }

        [HttpPost]
        public ActionResult CalificarUser(ClasificacionesViewModel cModel)
        {
            try
            {
                
                var calificacion = db.Calificaciones.Where(x => x.Id_Calificacion == cModel.idCal).FirstOrDefault();

                calificacion.Cal_Usuario = cModel.cal_usuario;
                calificacion.Obs_DelServi = cModel.obs_DelServi;
                calificacion.Pago = cModel.pago;

                var user = db.Users.Where(x => x.Id == calificacion.Servicios.Presupuestos.Solicitudes.Usuarios.Id).FirstOrDefault();

                    user.CantServicios = user.CantServicios + 1;
                    user.Calificacion = user.Calificacion + cModel.cal_servi;

                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;

                db.Entry(calificacion).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            catch (Exception e)
            {
                throw (e);
            }

            return RedirectToAction("MisServicios", "Servicios");
        }



        [HttpGet]
        public ActionResult CalificarServi(int id)
        {
            var calificaciones = new ClasificacionesViewModel();
            calificaciones.cumplimiento = true;

            calificaciones.cal_servi = 0;
            calificaciones.cal_trabajo = 0;

            calificaciones.Servicio = db.Servicios.FirstOrDefault(x => x.Id_Servicio == id);

            return View(calificaciones);
        }

        [HttpPost]
        public ActionResult CalificarServi(ClasificacionesViewModel cModel)
        {
            try
            {
                var calificacion = new Calificaciones();
                var servicio = db.Servicios.FirstOrDefault(x => x.Id_Servicio == cModel.Servicio.Id_Servicio);
                var presupuesto = db.Presupuestos.FirstOrDefault(x => x.Id_Presupuesto == servicio.Presupuestos.Id_Presupuesto);
                var servi = db.Servis.Where(x => x.Presupuestos.Any(y => y.Id_Presupuesto == presupuesto.Id_Presupuesto)).FirstOrDefault();

                string idServi = servi.Id;

                servicio.Estado = "Calificado";

                calificacion.Servicios = servicio;
                calificacion.Obs_DelUsuario = cModel.obs_DelUser;
                calificacion.Cal_Servi = cModel.cal_servi;
                calificacion.Cal_Trabajo = cModel.cal_trabajo;
                calificacion.Cumplimiento = cModel.cumplimiento;

                if (cModel.cumplimiento == true)
                {
                    servi.CantServicios = servi.CantServicios + 1;
                    servi.CalTareas = servi.CalTareas + cModel.cal_trabajo;
                    servi.Calificacion = servi.Calificacion + cModel.cal_servi;

                    _CuentaCorriente._generarDebito(servi.Id, servicio.Id_Servicio.ToString());

                    db.Entry(servi).State = System.Data.Entity.EntityState.Modified;
                }

                db.Calificaciones.Add(calificacion);

                db.Entry(servicio).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
            }
            catch(Exception e)
            {
                throw (e);
            }

            return RedirectToAction("MisServicios","Servicios");
        }

        // GET: Calificacion
        public ActionResult Index()
        {
            return View();
        }

        // GET: Calificacion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Calificacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Calificacion/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Calificacion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Calificacion/Edit/5
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

        // GET: Calificacion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Calificacion/Delete/5
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
