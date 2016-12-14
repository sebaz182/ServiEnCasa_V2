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
    public class ProfesionesController : Controller
    {
        //Instancio mi base de datos
        private ModeloContainer db = new ModeloContainer();
        //
        // GET: Profesiones
        public ActionResult Index()
        {
            return View(db.Profesiones.ToList());
        }

        // GET: Profesiones/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Profesiones/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profesiones/Create
        [HttpPost]
        public ActionResult Create(CrearPresupuestoViewModels viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var profesion = new Profesiones { Desc_Profesion = viewModel.profesion };
                    if (ValidaProfesion(profesion.Desc_Profesion.ToString()) == false)
                    {
                        db.Profesiones.Add(profesion);
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

        // GET: Profesiones/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var profesion = db.Profesiones.Find(id);
            if (profesion == null)
            {
                return HttpNotFound();
            }
            var ViewModel = new EditarProfesionViewModels()
                {
                    profesion = profesion.Desc_Profesion
                };
            
            return View(ViewModel);
        }

        // POST: Profesiones/Edit/5
        [HttpPost]
        public ActionResult Edit(EditarProfesionViewModels vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Editar = db.Profesiones.Find(vm.id);
                    string desc = vm.profesion;
                    if (desc.ToUpper() != Editar.Desc_Profesion.ToString().ToUpper())
                    {
                        if (ValidaProfesion(desc) == false)
                        {
                            Editar.Desc_Profesion = vm.profesion;
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

        // GET: Profesiones/Delete/5
        public ActionResult Delete(int id)
        {
            var profesion = db.Profesiones.Find(id);
            if (profesion == null)
            {
                return HttpNotFound();
            }
            return View(profesion);
        }

        // POST: Profesiones/Delete/5
        [HttpPost,ActionName ("Delete")]
        public ActionResult ConfirmarEliminar(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var profesion = db.Profesiones.Find(id);
                if (profesion == null)
                {
                    return HttpNotFound();
                }
                if (profesion.Tareas.Count() >= 0)
                {
                    db.Entry(profesion).State = System.Data.Entity.EntityState.Deleted;
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

        public bool ValidaProfesion(string p)
        {
            foreach (Profesiones oProf in db.Profesiones.ToList())
            {
                if (oProf.Desc_Profesion.ToUpper() == p.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
