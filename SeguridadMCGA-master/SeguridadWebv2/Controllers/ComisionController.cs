using SeguridadWebv2.Models;
using SeguridadWebv2.Models.App;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace SeguridadWebv2.Controllers
{
    [Authorize(Roles = "Admin, AllGrupos")]
    public class ComisionController : Controller
    {
        private ModeloContainer db = new ModeloContainer();

        public ActionResult Index()
        {
            return View(db.Comision.ToList());
        }

        // GET: Comision/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var comision = db.Comision.Find(id);
            if (comision == null)
            {
                return HttpNotFound();
            }
            var ViewModel = new ComisionViewModel()
            {
                importe = comision.ImpComision
            };

            return View(ViewModel);
        }

        // POST: Zonas/Edit/5
        [HttpPost]
        public ActionResult Edit(ComisionViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ComisionN = db.Comision.Find(viewModel.id);
                    if (viewModel.importe >= 0)
                    {
                        ComisionN.ImpComision = viewModel.importe;
                        db.Entry(ComisionN).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
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
    }
}