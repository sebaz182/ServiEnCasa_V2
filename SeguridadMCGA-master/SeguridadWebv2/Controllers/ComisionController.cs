using Microsoft.AspNet.Identity;
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

        public ActionResult ComisionAuditoria()
        {
            return View(db.ComisionAuditoria.ToList().OrderByDescending(x=>x.Id_ComisionAuditoria));
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

        [HttpPost]
        public ActionResult Edit(ComisionViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ComisionN = db.Comision.Find(viewModel.id);

                    var _comAuditoria = new ComisionAuditoria();
                    var idUser = User.Identity.GetUserId();
                    var usuario = db.Users.Where(x => x.Id == idUser).FirstOrDefault();

                    _comAuditoria.FechaAlta = ComisionN.FechaAlta;
                    _comAuditoria.FechaModificacion = DateTime.Now;
                    _comAuditoria.UsuarioAlta = ComisionN.Usuario;
                    _comAuditoria.ImpComision = ComisionN.ImpComision;
                    _comAuditoria.UsuarioModificacion =  usuario.Nombre +" "+ usuario.Apellido;

                    if (viewModel.importe >= 0 && ComisionN.ImpComision!= viewModel.importe)
                    {
                        ComisionN.ImpComision = viewModel.importe;
                        ComisionN.Usuario = usuario.Nombre + " " + usuario.Apellido;


                        db.Entry(ComisionN).State = EntityState.Modified;

                        db.ComisionAuditoria.Add(_comAuditoria);

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