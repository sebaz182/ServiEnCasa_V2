using SeguridadWebv2.Models;
using SeguridadWebv2.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SeguridadWebv2.Controllers
{
    [Authorize(Roles = "Admin, AllPermisos")]
    public class ZonasController : Controller
    {
        //Instanciar la Base de Datos
        private ModeloContainer db = new ModeloContainer();
        //
        // GET: Zonas
        public ActionResult Index()
        {
            return View(db.Zonas.ToList());
        }

        // GET: Zonas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Zonas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Zonas/Create
        [HttpPost]
        public ActionResult Create(ZonasViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var zona = new Zonas { Zona = viewModel.zona };
                    if (ValidaZona(zona.Zona.ToString()) == false)
                    {
                        db.Zonas.Add(zona);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                return View();
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Zonas/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var zona = db.Zonas.Find(id);
            if (zona == null)
            {
                return HttpNotFound();
            }
            var ViewModel = new ZonasViewModel()
            {
                zona = zona.Zona
            };

            return View(ViewModel);
        }

        // POST: Zonas/Edit/5
        [HttpPost]
        public ActionResult Edit(ZonasViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Editar = db.Zonas.Find(viewModel.id);
                    string zona = viewModel.zona;
                    if (zona.ToUpper() != Editar.Zona.ToString().ToUpper())
                    {
                        if (ValidaZona(zona) == false)
                        {
                            Editar.Zona = viewModel.zona;
                            db.Entry(Editar).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                    return View();
                }
                // TODO: Add update logic here
            }
            catch
            {
                return View();
            }
            return View();
        }

        // GET: Zonas/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var zona = db.Zonas.Find(id);
            if (zona == null)
            {
                return HttpNotFound();
            }
            return View(zona);
        }

        // POST: Zonas/Delete/5
        [HttpPost, ActionName ("Delete")]
        public ActionResult ConfirmarEliminacion(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var zona = db.Zonas.Find(id);
                if (zona == null)
                {
                    return HttpNotFound();
                }
                if (zona.Solicitudes.Count() >= 0)
                {
                    db.Entry(zona).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }

                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public bool ValidaZona(string z)
        {
            foreach (Zonas oZona in db.Zonas.ToList())
            {
                if (oZona.Zona.ToUpper() == z.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
