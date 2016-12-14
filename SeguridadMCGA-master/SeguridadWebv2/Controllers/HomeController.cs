using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace SeguridadWebv2.Controllers
{
    public class HomeController : Controller
    {

        Models.ModeloContainer db = new Models.ModeloContainer();

        public class jsonReturn 
        {
            public int Id;
            public string option;
        }

        [WebMethod]
        public string getTareas(int idProf) 
        {
            var tareas = db.Tareas.Where(x => x.Profesiones.Id_Profesion == idProf).Select(y => new jsonReturn { Id = y.Id_Tarea, option = y.Desc_Tarea });

           return JsonConvert.SerializeObject(tareas.ToList());
        }
            
        public ActionResult Index()
        {
            ViewBag.Profesiones = db.Profesiones.ToList();
            ViewBag.Tareas = db.Tareas.ToList();
            ViewBag.Horarios = db.Horarios.ToList();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
        [HttpPost]
        public ActionResult EnviarSolicitud(SolicitudViewModel model, HttpPostedFileBase fotoupload)
        {
            model.Foto = "";
            if (ModelState.IsValid && User.Identity.GetUserId() != null)
            {
                try
                {
                    if (fotoupload != null)
                    {
                        string path = "~/Content/img/" + fotoupload.FileName;
                        model.Foto = path;
                        string fullpath = Server.MapPath("~/Content/img/") + fotoupload.FileName;
                        fotoupload.SaveAs(fullpath);
                    }
                    else
                    {
                        model.Foto = "~/Content/img/sinFoto.png";
                    }


                    var profesiones = db.Profesiones.Find(model.ProfesionId);
                    var tareas = db.Tareas.Find(model.TareaId);
                    var zona = db.Zonas.Where(x => x.Zona == model.Zona).FirstOrDefault();
                    var hora = db.Horarios.Find(model.idHora);

                    var IdUsuario = User.Identity.GetUserId();
                    var usuario = db.Users.Where(x => x.Id == IdUsuario).FirstOrDefault();

                    var solicitud = new Solicitudes
                    {
                        Profesiones = profesiones,
                        Tareas = tareas,
                        Zonas = zona,
                        Desc_Solicitud = model.DescripcionServicio,
                        Estado = "Alta",
                        Fecha = model.FechaInicio,
                        Contador = 0,
                        Foto = model.Foto,
                        Usuarios = usuario,
                        Horarios = hora,
                    };
                    db.Solicitudes.Add(solicitud);
                    db.SaveChanges();

                    ViewBag.Message = "El archivo se ha subido correctamente";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                return View();
            }

            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult BuscarZonas(string busqueda)
        {
            busqueda = busqueda.ToUpper();
            List<string> zonas = db.Zonas.Where(x => x.Zona.ToUpper().Contains(busqueda)).Select(x=>x.Zona).ToList();

            return Json(zonas, JsonRequestBehavior.AllowGet);
        }

    }
}