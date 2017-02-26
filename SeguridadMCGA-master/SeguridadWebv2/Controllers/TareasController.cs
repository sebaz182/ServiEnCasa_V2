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
    public class TareasController : Controller
    {
        // crear una instancia desde la base de datos.
        private ModeloContainer db = new ModeloContainer();
        //
        // GET: Tareas
        public ActionResult Index(int? ProfesionID)
        {
            if (ProfesionID == null)
            {
                ViewBag.ProfesionID = new SelectList(db.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                return View(db.Tareas.ToList());
            }
            else
            {

                ViewBag.ProfesionID = new SelectList(db.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                var resultado = db.Tareas.Where(x => x.Profesiones.Id_Profesion == ProfesionID).ToList();
                return View(resultado);
            }
        }
        
        [HttpPost]
        public ActionResult Busqueda(int ProfesionID)
        {
            var resultado = db.Tareas.Where(x => x.Profesiones.Id_Profesion == ProfesionID).ToList();
            return View(resultado);
        }

        // GET: Tareas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tareas/Create
        public ActionResult Create()
        {
            ViewBag.ProfesionID = new SelectList(db.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
            return View();
        }

        // POST: Tareas/Create
        [HttpPost]
        public ActionResult Create(CrearTareaViewModel vmCrearTarea)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var prof = db.Profesiones.Find(vmCrearTarea.ProfesionID);
                    var tarea = new Tareas { Desc_Tarea = vmCrearTarea.tarea, Profesiones = prof };
                    if (ValidaTarea(tarea.Desc_Tarea.ToString(), tarea.Profesiones.Id_Profesion) == false)
                    {
                        db.Tareas.Add(tarea);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.ProfesionID = new SelectList(db.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                return View();
            }
            catch
            {
                return RedirectToAction("Index");
            }
            }

        // GET: Tareas/Edit/5
        public ActionResult Edit(int id)
        {

            var tarea = db.Tareas.Find(id);

            if (tarea == null)
            {
                return HttpNotFound();
            }
            var ViewModel = new EditarTareaViewModel()
            {
                profesion = tarea.Profesiones.Desc_Profesion,
                tarea = tarea.Desc_Tarea
            };

            return View(ViewModel);
        }

        // POST: Tareas/Edit/5
        [HttpPost]
        public ActionResult Edit(EditarTareaViewModel vmEditar)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Editar = db.Tareas.Find(vmEditar.id);
                    string desc = vmEditar.tarea;
                    if (desc.ToUpper() != Editar.Desc_Tarea.ToString().ToUpper())
                    {
                        if (ValidaTarea(desc,vmEditar.id) == false)
                        {
                            Editar.Desc_Tarea = vmEditar.tarea;
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

        // GET: Tareas/Delete/5
        public ActionResult Delete(int id)
        {
            var tareas = db.Tareas.Find(id);
            if (tareas == null)
            {
                return HttpNotFound();
            }
            return View(tareas);
        }

        // POST: Tareas/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmarEliminar(int id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var tarea = db.Tareas.Find(id);
                if (tarea == null)
                {
                    return HttpNotFound();
                }

                db.Entry(tarea).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public bool ValidaTarea(string t, int id)
        {
            foreach (Tareas oTarea in db.Tareas.ToList())
            {
                if (oTarea.Desc_Tarea.ToUpper() == t.ToUpper() && oTarea.Profesiones.Id_Profesion == id )
                {
                    return true;
                }
            }
            return false;
        }
    }
}
