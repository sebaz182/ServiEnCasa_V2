using SeguridadWebv2.Models;
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

        [HttpGet]
        public ActionResult CalificarServi(int id)
        {
            return View(db.Servicios.Where(x => x.Id_Servicio == id).ToList());
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
