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

    public class SolicitudesController : Controller
    {
        private ModeloContainer db = new ModeloContainer();
        

        // GET: Solicitudes
        [HttpGet]
        public ActionResult MisSolicitudes()
        {
            var IdUsuario = User.Identity.GetUserId();

            var lista = db.Solicitudes.Where(y => y.Usuarios.Id == IdUsuario).ToList();

            lista.OrderByDescending(x => x.Fecha);

            return View(lista.Where(x=>x.Estado != "Realizado").ToList());
        }

        public List<Servis> _maching(Solicitudes _solicitud)
        {
            var _ListaServis = new List<Servis>();

            foreach (var servi in db.Servis.Include("ServisProfesiones").ToList())
            {
                var prof = servi.ServisProfesiones.Where(x => x.Profesion.Id_Profesion == _solicitud.Profesiones.Id_Profesion).Any();
                if (prof == true)
                {
                    _ListaServis.Add(servi);
                }
            }

            return _ListaServis;
        }


        // GET: Solicitudes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Solicitudes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Solicitudes/Create
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

        // GET: Solicitudes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Solicitudes/Edit/5
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

        // GET: Solicitudes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Solicitudes/Delete/5
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
