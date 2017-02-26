using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models.App
{
    public class InicializacionDatos
    {
        public static void InitializeIdentityForEF(ModeloContainer db)
        {
            /*Usuario*/
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string nombre = "Sebastian";
            const string apellido = "Zeballos";
            const bool estado = true;
            const string name = "seba@gmail.com";
            const string password = "Mcga@123456";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name, Nombre = nombre, Apellido = apellido, Estado = estado, EmailConfirmed = true };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);

            }

            var groupManager = new GrupoManager();
            var newGroup = new ApplicationGroup("Administradores", "Acceso General al Sistema");

            groupManager.CreateGroup(newGroup);
            groupManager.SetUserGroups(user.Id, new string[] { newGroup.Id });
            groupManager.SetGroupRoles(newGroup.Id, new string[] { role.Name });


            /*Servi*/

            const string nombreserv = "Sebastian";
            const string apellidoserv = "Zeballos";
            const bool estadoserv = true;
            const string nameserv = "seba@gmail.com";
            const string passwordserv = "Mcga@123456";
            const string roleNameserv = "Servis";

            var rol = roleManager.FindByName(roleNameserv);
            if (rol == null)
            {
                rol = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(rol);
            }

            var servi = userManager.FindByName(name);
            if (servi == null)
            {
                servi = new Servis { UserName = nameserv, Email = nameserv, Nombre = nombreserv, Apellido = apellidoserv, Estado = estadoserv, EmailConfirmed = true };
                var resultado = userManager.Create(servi, passwordserv);
                resultado = userManager.SetLockoutEnabled(servi.Id, false);

            }

            var groupManagerServi = new GrupoManager();
            var newGroupServi = new ApplicationGroup("Servis", "Acceso de Servis al Sistema");

            groupManager.CreateGroup(newGroupServi);
            groupManager.SetUserGroups(servi.Id, new string[] { newGroupServi.Id });
            groupManager.SetGroupRoles(newGroupServi.Id, new string[] { rol.Name });


            var groupManagerUser = new GrupoManager();
            var newGroupUser = new ApplicationGroup("Users", "Acceso de Usuarios al Sistema");

            groupManager.CreateGroup(newGroupUser);
            groupManager.SetUserGroups(user.Id, new string[] { newGroupUser.Id });
            groupManager.SetGroupRoles(newGroupUser.Id, new string[] { rol.Name });

            var PermisosServis = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Servis"
                },
            };
            PermisosServis.ForEach(c => db.Roles.Add(c));

            var PermisosUsers = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Users"
                },
            };
            PermisosUsers.ForEach(c => db.Roles.Add(c));


            var PermisosUsuario = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Usuario"
                },
                new ApplicationRole {
                    Name = "Editar_Usuario"
                },
                new ApplicationRole {
                    Name = "Detalle_Usuario"
                },
                new ApplicationRole {
                    Name = "Eliminar_Usuario"
                },
                new ApplicationRole {
                    Name = "AllUsuarios"
                }
            };
            PermisosUsuario.ForEach(c => db.Roles.Add(c));


            var PermisosGrupo = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Grupo"
                },
                new ApplicationRole {
                    Name = "Editar_Grupo"
                },
                new ApplicationRole {
                    Name = "Detalle_Grupo"
                },
                new ApplicationRole {
                    Name = "Eliminar_Grupo"
                },
                new ApplicationRole {
                    Name = "AllGrupos"
                }
            };
            PermisosGrupo.ForEach(c => db.Roles.Add(c));


            var PermisosAcciones = new List<ApplicationRole> {
                new ApplicationRole {
                    Name = "Agregar_Permiso"
                },
                new ApplicationRole {
                    Name = "Editar_Permiso"
                },
                new ApplicationRole {
                    Name = "Detalle_Permiso"
                },
                new ApplicationRole {
                    Name = "Eliminar_Permiso"
                },
                new ApplicationRole {
                    Name = "AllPermisos"
                }
            };
            PermisosUsuario.ForEach(c => db.Roles.Add(c));

            var grupos = new List<ApplicationGroup> {
                new ApplicationGroup {
                    Name = "Gestionar Usuarios",
                    Description = "Gestionar Usuarios"
                },
                new ApplicationGroup {
                    Name = "Gestionar Grupos",
                    Description = "Gestionar Grupos"
                },
                new ApplicationGroup {
                    Name = "Gestionar Acciones",
                    Description = "Gestionar Acciones"
                },
             };
            grupos.ForEach(c => db.ApplicationGroups.Add(c));

            var zonas = new List<Zonas>
            {
                 new Zonas
                 {
                    Zona = "Rosario Centro"
                },
                new Zonas
                {
                    Zona = "Rosario Sur"
                },
                new Zonas
                {
                    Zona = "Rosario Norte"
                },
                new Zonas
                {
                    Zona = "Rosario Oeste"
                }
            };
            zonas.ForEach(c => db.Zonas.Add(c));

            var horarios = new List<Horarios>
            {
                 new Horarios
                 {
                    Horario = "8:00Hs - 12:00Hs"
                 },
                new Horarios
                {
                    Horario = "12:00Hs - 16:00Hs"
                },
                new Horarios
                {
                    Horario = "16:00Hs - 19:00Hs"
                }
            };
            horarios.ForEach(c => db.Horarios.Add(c));

            var comision = new List<Comision>
            {
                 new Comision
                 {
                    ImpComision = 45
                 }
            };
            comision.ForEach(c => db.Comision.Add(c));

            var profesiones = new List<Profesiones>
            {
                new Profesiones
                {
                    Desc_Profesion = "Electricista"
                },
                new Profesiones
                {
                    Desc_Profesion = "Gasista"
                },
                new Profesiones
                {
                   Desc_Profesion = "Cerrajero"
                },
                new Profesiones
                {
                    Desc_Profesion = "Service Aire Acondicionado"
                },
                new Profesiones
                {
                    Desc_Profesion = "Servicio Domestico"
                }
            };
            profesiones.ForEach(c => db.Profesiones.Add(c));


            var tareas = new List<Tareas>
            {
                new Tareas
                {
                    Desc_Tarea = "Instalar",
                    Profesiones = profesiones[0]
                },
                new Tareas
                {
                    Desc_Tarea = "Reparar",
                    Profesiones = profesiones[0]
                },
                new Tareas
                {
                    Desc_Tarea = "Sustituir",
                    Profesiones = profesiones[0]
                },
                new Tareas
                {
                    Desc_Tarea = "Cablear",
                    Profesiones = profesiones[0]
                },
                new Tareas
                {
                    Desc_Tarea = "Otro",
                    Profesiones = profesiones[0]
                },

                /*2*/
                new Tareas
                {
                    Desc_Tarea = "Revisar perdida de Gas",
                    Profesiones = profesiones[1]
                },
                new Tareas
                {
                    Desc_Tarea = "Habilitar Instalación",
                    Profesiones = profesiones[1]
                },
                new Tareas
                {
                    Desc_Tarea = "Realizar Instalación",
                    Profesiones = profesiones[1]
                },
                new Tareas
                {
                    Desc_Tarea = "Realizar Service",
                    Profesiones = profesiones[1]
                },
                new Tareas
                {
                    Desc_Tarea = "Otro",
                    Profesiones = profesiones[1]
                },
                /*3*/
                new Tareas
                {
                    Desc_Tarea = "Abrir Cerradura",
                    Profesiones = profesiones[2]
                },
                new Tareas
                {
                    Desc_Tarea = "Reparar",
                    Profesiones = profesiones[2]
                },
                new Tareas
                {
                    Desc_Tarea = "Sustituir",
                    Profesiones = profesiones[2]
                },
                new Tareas
                {
                    Desc_Tarea = "Otro",
                    Profesiones = profesiones[2]
                },
                /*4*/
               new Tareas
                {
                    Desc_Tarea = "Instalar",
                    Profesiones = profesiones[3]
                },
                new Tareas
                {
                    Desc_Tarea = "Reparar",
                    Profesiones = profesiones[3]
                },
                new Tareas
                {
                    Desc_Tarea = "Sustituir",
                    Profesiones = profesiones[3]
                },
                new Tareas
                {
                    Desc_Tarea = "Recargar",
                    Profesiones = profesiones[3]
                },
                new Tareas
                {
                    Desc_Tarea = "Otro",
                    Profesiones = profesiones[3]
                },
                /*5*/
                
                new Tareas
                {
                    Desc_Tarea = "Servicio Completo",
                    Profesiones = profesiones[4]
                },
                new Tareas
                {
                    Desc_Tarea = "Cocina",
                    Profesiones = profesiones[4]
                },
                new Tareas
                {
                    Desc_Tarea = "Limpieza",
                    Profesiones = profesiones[4]
                },
                new Tareas
                {
                    Desc_Tarea = "Lavado y Planchado",
                    Profesiones = profesiones[4]
                },
                new Tareas
                {
                    Desc_Tarea = "Niñera",
                    Profesiones = profesiones[4]
                },
                new Tareas
                {
                    Desc_Tarea = "Otro",
                    Profesiones = profesiones[4]
                }
            };
            tareas.ForEach(c => db.Tareas.Add(c));

            db.SaveChanges();
        }
    }
}