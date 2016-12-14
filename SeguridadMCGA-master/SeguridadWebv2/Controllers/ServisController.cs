using SeguridadWebv2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SeguridadWebv2.Models.App;

namespace SeguridadWebv2.Controllers
{
    public class ServisController : Controller
    {
        private ModeloContainer db = new ModeloContainer();

        public ServisController()
        {
        }
        public ServisController(ApplicationUserManager userManager,
            ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private GrupoManager _groupManager;
        public GrupoManager GroupManager
        {
            get
            {
                return _groupManager ?? new GrupoManager();
            }
            private set
            {
                _groupManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext()
                    .Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        // GET: Servis
        public ActionResult Index()
        {
            return View(db.Servis.ToList());
        }

        // GET: Servis/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Servis/Create
        [HttpGet]
        public ActionResult Create(int? ProfesionID)
        {
                // Show a list of available groups:
            ViewBag.Tareas = new SelectList(db.Tareas.ToList(), "Id_Tarea", "Desc_Tarea");
            ViewBag.Profesiones = new SelectList(db.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");

            if (ProfesionID == null)
            {
                ViewBag.ProfesionID = new SelectList(db.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                ViewBag.TareaID = new SelectList(db.Tareas.ToList(), "Id_Tarea", "Desc_Tarea");
                //ViewBag.TareaID = new SelectList(db.Tareas.Where(x => x.Profesiones.Id_Profesion == ProfesionID).ToList(), "Id_Tarea", "Desc_Tarea");
                return View();
            }
            else
            {
                ViewBag.ProfesionID = new SelectList(db.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                ViewBag.TareaID = new SelectList(db.Tareas.Where(x => x.Profesiones.Id_Profesion == ProfesionID).ToList(), "Id_Tarea", "Desc_Tarea");
                return View();
            }


        }

        public JsonResult GetTareas(int selectedcampo)
        {
            var resultado = db.Tareas.Where(x => x.Profesiones.Id_Profesion == selectedcampo).ToList();
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Busqueda(int? ProfesionID)
        {
            if (ProfesionID == null)
            {
                ViewBag.ProfesionID = new SelectList(db.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                ViewBag.TareaID = db.Tareas.Where(x => x.Profesiones.Id_Profesion == ProfesionID).ToList();
                return View();
            }
            else
            {

                ViewBag.ProfesionID = new SelectList(db.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                ViewBag.TareaID = db.Tareas.Where(x => x.Profesiones.Id_Profesion == ProfesionID).ToList();
                return View();
            }
            
        }

        // POST: Servis/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateServiViewModel vmServi, HttpPostedFileBase fotoupload, params string[] selectedTareas)
        {
            vmServi.Foto = "";
            if (ModelState.IsValid)
            {
                if (fotoupload != null)
                {
                    string path = "~/Content/img/" + fotoupload.FileName;
                    vmServi.Foto = path;
                    string fullpath = Server.MapPath("~/Content/img/") + fotoupload.FileName;
                    fotoupload.SaveAs(fullpath);
                }
                else
                {
                    vmServi.Foto = "~/Content/img/sinFoto.png";
                }

                var prof = db.Profesiones.Find(vmServi.ProfesionID);

                Servis servi = new Servis
                {
                    UserName = vmServi.mail,
                    Email = vmServi.mail,
                    Nombre = vmServi.nombre,
                    Apellido = vmServi.apellido,
                    Estado = true,
                    PhoneNumber = vmServi.telefono,
                    Matricula = vmServi.matricula,
                    DNI = vmServi.dni,
                    Foto = vmServi.Foto,
                };
                servi.Profesiones.Add(prof);

                if (selectedTareas == null)
                    {
                        Tareas t = db.Tareas.Where(x=>x.Desc_Tarea == "Cableado").FirstOrDefault();
                        servi.Tareas.Add(t);
                        //selectedTareas = selectedTareas ?? new string[] { };
                        //await this.GroupManager
                        //    .SetUserGroupsAsync(user.Id, selectedGroups);
                    }
                
                var adminresult = await this.UserManager
                    .CreateAsync(servi, vmServi.pass);

                //Add User to the selected Groups 
                if (adminresult.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(servi.Id, "Admin");
                    string selectedGroups = "Services"; 
                    if (selectedGroups != null)
                    {
                        //selectedGroups = selectedGroups ?? new string[] { };
                        await this.GroupManager
                            .SetUserGroupsAsync(servi.Id, selectedGroups);
                    }


                    var code = await this.UserManager.GenerateEmailConfirmationTokenAsync(servi.Id);
                    var callbackUrl = Url.Action("ConfirmarEmail", "Account", new { userId = servi.Id, code = code }, protocol: Request.Url.Scheme);
                    await this.UserManager.SendEmailAsync(servi.Id, "Confirmar su cuenta", "Por favor para confirmar su cuenta haga click en el siguiente enlace: <a href=\"" + callbackUrl + "\">link</a>");
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    this.AddErrors(adminresult);
                }
            }
            ViewBag.Groups = new SelectList(
                await this.RoleManager.Roles.ToListAsync(), "Id", "Name");
            return View();







            //try
            //{
            //if (ModelState.IsValid)
            //{
            //var prof = db.Profesiones.Find(vmServi.ProfesionID);

            //    var prof = db.Profesiones.Find(vmServi.ProfesionID);
            //    //var servi = new Servis { Nombre = vmServi.nombre, Apellido = vmServi.apellido, Email = vmServi.mail, Pass = cc.Encriptar(vmServi.pass), Telefono = vmServi.telefono,  };
            //    //if (cc.ValidarEmail(servi.Email.ToString()) == false)
            //    //{
            //        if (selectedTareas != null)
            //        {
            //            Tareas t = db.Tareas.Where(x=>x.Desc_Tarea == "Cableado").FirstOrDefault();
            //            servi.Tareas.Add(t);
            //            //selectedTareas = selectedTareas ?? new string[] { };
            //            //await this.GroupManager
            //            //    .SetUserGroupsAsync(user.Id, selectedGroups);
            //        }
            //        servi.Profesiones.Add(prof);
            //        servi.Activo = true;
            //        //agregar datos extras
            //        servi.DNI = "33222111";
            //        servi.Matricula = "3444";
            //        servi.Foto = "foto.jpg";
            //        db.Servis.Add(servi);
            //        db.SaveChanges();
            //        return RedirectToAction("Index");
            //    //}
            //}
            //return View();
            //}
            //catch
            //{
            //return RedirectToAction("Index");
            //}
        }

        // GET: Servis/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Servis/Edit/5
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

        // GET: Servis/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Servis/Delete/5
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

        public void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}
