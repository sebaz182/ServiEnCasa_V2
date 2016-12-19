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
            return View(dbContext.Servis.ToList());
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
            ViewBag.Tareas = new SelectList(dbContext.Tareas.ToList(), "Id_Tarea", "Desc_Tarea");
            ViewBag.Profesiones = new SelectList(dbContext.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");

            if (ProfesionID == null)
            {
                ViewBag.ProfesionID = new SelectList(dbContext.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                ViewBag.TareaID = new SelectList(dbContext.Tareas.ToList(), "Id_Tarea", "Desc_Tarea");
                //ViewBag.TareaID = new SelectList(db.Tareas.Where(x => x.Profesiones.Id_Profesion == ProfesionID).ToList(), "Id_Tarea", "Desc_Tarea");
                return View();
            }
            else
            {
                ViewBag.ProfesionID = new SelectList(dbContext.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                ViewBag.TareaID = new SelectList(dbContext.Tareas.Where(x => x.Profesiones.Id_Profesion == ProfesionID).ToList(), "Id_Tarea", "Desc_Tarea");
                return View();
            }
        }

        public JsonResult GetTareas(int selectedcampo)
        {
            var resultado = dbContext.Tareas.Where(x => x.Profesiones.Id_Profesion == selectedcampo).ToList();
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Busqueda(int? ProfesionID)
        {
            if (ProfesionID == null)
            {
                ViewBag.ProfesionID = new SelectList(dbContext.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                ViewBag.TareaID = dbContext.Tareas.Where(x => x.Profesiones.Id_Profesion == ProfesionID).ToList();
                return View();
            }
            else
            {

                ViewBag.ProfesionID = new SelectList(dbContext.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                ViewBag.TareaID = dbContext.Tareas.Where(x => x.Profesiones.Id_Profesion == ProfesionID).ToList();
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

                ////Add Profesiones 
                //Profesiones p = db.Profesiones.Where(x => x.Id_Profesion == vmServi.ProfesionID).FirstOrDefault();

                //if (p != null)
                //{
                //    ServisProfesiones sp = new ServisProfesiones();

                //    sp.Profesion = p;
                //    servi.ServisProfesiones.Add(sp);
                //}

                ////Add Tareas
                //if (selectedTareas == null)
                //{
                //    Tareas t = db.Tareas.Where(x => x.Desc_Tarea == "Cablear").FirstOrDefault();

                //    if (t != null)
                //    {
                //        ServisTareas st = new ServisTareas();

                //        st.Tarea = t;
                //        servi.ServisTareas.Add(st);
                //    }
                //}


                //An entity object cannot be referenced by multiple instances of IEntityChangeTracker
                //System.InvalidOperationException: An entity object cannot be referenced by multiple instances of IEntityChangeTracker.
                var adminresult = await this.UserManager.CreateAsync(servi, vmServi.pass);

                //Add User to the selected Groups 
                if (adminresult.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(servi.Id, "Admin");
                    string selectedGroups = "Services";
                    if (selectedGroups != null)
                    {
                        //selectedGroups = selectedGroups ?? new string[] { };
                        //await this.GroupManager
                        //    .SetUserGroupsAsync(servi.Id, selectedGroups);
                    }


                    var code = await this.UserManager.GenerateEmailConfirmationTokenAsync(servi.Id);
                    //var callbackUrl = Url.Action("ConfirmarEmail", "Account", new { userId = servi.Id, code = code }, protocol: Request.Url.Scheme);
                    //await this.UserManager.SendEmailAsync(servi.Id, "Confirmar su cuenta", "Por favor para confirmar su cuenta haga click en el siguiente enlace: <a href=\"" + callbackUrl + "\">link</a>");


                    //return RedirectToAction("Index");
                    return RedirectToAction("Asignar", new { _idServi = servi.Id, _idProfesion = vmServi.ProfesionID });
                }
                else
                {
                    this.AddErrors(adminresult);
                }
            }
            ViewBag.Groups = new SelectList(
                await this.RoleManager.Roles.ToListAsync(), "Id", "Name");

            return View();
        }


        //Asignar Profesion y Tarea
        //GET: Asignar
        [HttpGet]
        public ActionResult Asignar(string _idServi, int? _idProfesion)
        {
            if (_idProfesion == null)
            {
                ViewBag.ProfesionID = new SelectList(dbContext.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                return View(dbContext.Tareas.ToList());
            }
            else
            {
                ViewBag.ProfesionID = new SelectList(dbContext.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                var resultado = dbContext.Tareas.Where(x => x.Profesiones.Id_Profesion == _idProfesion).ToList();
                return View(resultado);
            }

            //ViewBag.ProfesionID = new SelectList(db.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
            //return View(db.Tareas.ToList());
        }

        [HttpPost]
        public ActionResult Asignar(string _idServi, int? _idProfesion, params string[] selectedRows)
        {
            List<Tareas> list = new List<Tareas>();

            if (_idProfesion == null)
            {
                ViewBag.ProfesionID = new SelectList(dbContext.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                list = dbContext.Tareas.ToList();
            }
            else
            {
                ViewBag.ProfesionID = new SelectList(dbContext.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                list = dbContext.Tareas.Where(x => x.Profesiones.Id_Profesion == _idProfesion).ToList();
            }

            if (list.Any() && selectedRows != null)
            {
                var servi = dbContext.Servis.FirstOrDefault(x => x.Id == _idServi);
                foreach (var sIndex in selectedRows)
                {
                    int index;

                    Int32.TryParse(sIndex, out index);

                    if (index != 0)
                    {
                        var tarea = list.FirstOrDefault(x=>x.Id_Tarea == index);

                        if (tarea != null)
                        {
                            if (!dbContext.ServiTareas.Any(x => x.Servi.Id == servi.Id && x.Tarea.Id_Tarea == tarea.Id_Tarea))
                            {
                                //Tarea + Id Profesion
                                ServisTareas st = new ServisTareas();

                                st.Tarea = tarea;
                                st.Servi = servi;

                                dbContext.ServiTareas.Add(st);
                            }
                        }
                    }
                }
                if(!dbContext.ServisProfesiones.Any(x=>x.Servi.Id == servi.Id && x.Profesion.Id_Profesion == _idProfesion))
                { 
                    ServisProfesiones sp = new ServisProfesiones();
                    sp.Profesion = dbContext.Profesiones.FirstOrDefault(x => x.Id_Profesion == _idProfesion);
                    sp.Servi = dbContext.Servis.FirstOrDefault(x => x.Id == _idServi);
                    dbContext.ServisProfesiones.Add(sp);
                }
                dbContext.SaveChanges();
            }

            return View(list);
        }

        [HttpPost]
        public ActionResult BusquedaTarea(string _idServi, int? _idProfesion)
        {
            if (_idProfesion == null)
            {
                ViewBag.ProfesionID = new SelectList(dbContext.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                return View(dbContext.Tareas.ToList());
            }
            else
            {
                ViewBag.ProfesionID = new SelectList(dbContext.Profesiones.ToList(), "Id_Profesion", "Desc_Profesion");
                var resultado = dbContext.Tareas.Where(x => x.Profesiones.Id_Profesion == _idProfesion).ToList();
                return View(resultado);
            }
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
