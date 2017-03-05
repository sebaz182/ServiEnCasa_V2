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
    
    public class ServiciosController : Controller
    {

        public ModeloContainer dbContext
        {
            get
            {
                if (_db == null)
                {
                    _db = new ModeloContainer();
                }
                return _db;
            }
        }
        private ModeloContainer _db;

        //[Authorize(Roles = "Users, AllGrupos")]
        public ActionResult MisServicios()
        {
            var IdUsuario = User.Identity.GetUserId();

            var lista = dbContext.Servicios.Where(x => x.Presupuestos.Solicitudes.Usuarios.Id == IdUsuario).ToList();

            return View(lista.ToList().OrderByDescending(x => x.Presupuestos.Hora));
        }

        //[Authorize(Roles = "Servis, AllGrupos")]
        public ActionResult ServiciosRealizados()
        {
            var IdUsuario = User.Identity.GetUserId();

            var lista = dbContext.Servicios.Where(x => x.Presupuestos.Servis.Any(y => y.Id == IdUsuario)).ToList();

            return View(lista.ToList().OrderByDescending(x=>x.Presupuestos.Hora));
        }

        [HttpGet]
        public ActionResult CancelarServicio (int id)
        {
            var servicio = dbContext.Servicios.Where(x => x.Id_Servicio == id).FirstOrDefault();

            return View(servicio);
        }

        [HttpPost, ActionName("CancelarServicio")]
        public ActionResult ConfirmarCancelacion(int id)
        {
            try
            {
                var servicio = dbContext.Servicios.Where(x => x.Id_Servicio == id).FirstOrDefault();

                servicio.Estado = "Cancelado";

                dbContext.Entry(servicio).State = System.Data.Entity.EntityState.Modified;

                dbContext.SaveChanges();

                return RedirectToAction("MisServicios", "Servicios");
            }
            catch
            {
                return View();
            }
            
        }
        // GET: Servicios
        public ActionResult Index()
        {
            return View();
        }

        // GET: Servicios/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        // GET: Servicios/ConfirmarContratacion
        [HttpGet]
        public ActionResult ConfirmarContratacion(int id)
        {
            var presupuesto = dbContext.Presupuestos.Where(x => x.Id_Presupuesto == id).FirstOrDefault();
            var solicitud = dbContext.Solicitudes.Where(x => x.Presupuestos.Any(y => y.Id_Presupuesto == presupuesto.Id_Presupuesto)).FirstOrDefault();

            var ConfServ = new GeneralConfirmacionVM
            {
                Presupuesto = presupuesto,
                Solicitud = solicitud,
                CrearServicio = new CrearServicioVm()
            };
            return View(ConfServ);
        }

        // POST: Servicios/Create
        [HttpPost]
        public ActionResult ConfirmarContratacion(int idpresupuesto, int idsolicitud )
        {
            try
            {
                var presupuesto = dbContext.Presupuestos.FirstOrDefault(x => x.Id_Presupuesto == idpresupuesto);
                var solicitud = dbContext.Solicitudes.FirstOrDefault(x => x.Id_Solicitud == idsolicitud);
                var presMod = CambiaEstadoPres(presupuesto);
                var solMod = CambiaEstadoSol(solicitud);
                var servi = presupuesto.Servis.FirstOrDefault();

                if (presupuesto != null)
                {
                    var servicio = new Models.App.Servicios();
                    servicio.Estado = "Confirmado";
                    servicio.Presupuestos = presupuesto;

                    dbContext.Entry(solMod).State = System.Data.Entity.EntityState.Modified;
                    dbContext.Entry(presMod).State = System.Data.Entity.EntityState.Modified;

                    dbContext.Servicios.Add(servicio);
                    
                    dbContext.SaveChanges();

                    var userManager = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();

                    userManager.SendEmail(solicitud.Usuarios.Id, "Felicitaciones por tu contratación!", "Felicitaciones... "+solicitud.Usuarios.Nombre.ToString()+ " "+solicitud.Usuarios.Apellido.ToString() + " \n Contrataste los servicios de " + servi.Nombre.ToString() +" "+servi.Apellido.ToString()+" \n Matricula: "+servi.Matricula.ToString() + " \n Comunícate con el Servi para pasarle tus datos al teléfono: " + servi.PhoneNumber.ToString() +" \n Exitos!!!");

                    userManager.SendEmail(servi.Id, "Felicitaciones Nuevo Tabajo", "Felicitaciones usted ha sido contratado por: " + solicitud.Usuarios.Nombre.ToString() + " " + solicitud.Usuarios.Apellido.ToString() + " \nPara la tarea de  " +solicitud.Desc_Solicitud.ToString() );

                    return RedirectToAction("ServicioContratado","Servicios", new { servicioId = servicio.Id_Servicio });

               
                }

                return View();
                // TODO: Add insert logic here


                
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult ServicioContratado(int servicioId)
        {
            try
            {
                var _servicio = dbContext.Servicios.Where(x => x.Id_Servicio == servicioId).FirstOrDefault();

                var _ServicioContratadoVM = new ServicioContratadoVM()
                {
                    servicio = _servicio
                };

                return View(_ServicioContratadoVM);
            }
            catch
            {
                return RedirectToAction("Inicio","Home");   
            }
        }


        // GET: Servicios/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Servicios/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Inicio","Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Servicios/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Servicios/Delete/5
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

        public Solicitudes CambiaEstadoSol(Solicitudes _solicitud)
        {
            _solicitud.Estado = "Realizado";

            return (_solicitud);
        }

        public Presupuestos CambiaEstadoPres(Presupuestos _presupuesto)
        {
            _presupuesto.Estado = "Aceptado";

            return (_presupuesto);
        }
    }
}
