﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using SeguridadWebv2.Models.App;

namespace SeguridadWebv2.Models
{
    public class ApplicationUserLogin : IdentityUserLogin<string> { }
    public class ApplicationUserClaim : IdentityUserClaim<string> { }
    public class ApplicationUserRole : IdentityUserRole<string> { }

    // Must be expressed in terms of our custom Role and other types:
    public class ApplicationUser
        : IdentityUser<string, ApplicationUserLogin,
        ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            // Add any custom User properties/code here
        }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public bool Estado { get; set; }
        public int? Calificacion { get; set; }
        public int? CantServicios { get; set; }

        public async Task<ClaimsIdentity>
            GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            var userIdentity = await manager
                .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }


    // Must be expressed in terms of our custom UserRole:
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        public ApplicationRole()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
        }

        // Add any custom Role properties/code here
    }


    // Must be expressed in terms of our custom types:
    public class ModeloContainer
        : IdentityDbContext<ApplicationUser, ApplicationRole,
        string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ModeloContainer()
            : base("DefaultConnection")
        {
        }

        static ModeloContainer()
        {
            Database.SetInitializer<ModeloContainer>(new ApplicationDbInitializer());
        }

        public static ModeloContainer Create()
        {
            return new ModeloContainer();
        }

        // Add the ApplicationGroups property:
        public virtual IDbSet<ApplicationGroup> ApplicationGroups { get; set; }
        
        public virtual DbSet<Tareas> Tareas { get; set; }
        public virtual DbSet<Zonas> Zonas { get; set; }
        public virtual DbSet<Horarios> Horarios { get; set; }
        public virtual DbSet<Servicios> Servicios { get; set; }
        public virtual DbSet<Servis> Servis { get; set; }
        public virtual DbSet<Presupuestos> Presupuestos { get; set; }
        public virtual DbSet<Solicitudes> Solicitudes { get; set; }
        public virtual DbSet<Profesiones> Profesiones { get; set; }
        public virtual DbSet<ServisTareas> ServiTareas { get; set; }
        public virtual DbSet<ServisProfesiones> ServisProfesiones { get; set; }
        public virtual DbSet<Calificaciones> Calificaciones { get; set; }
        public virtual DbSet<Comision> Comision { get; set; }
        public virtual DbSet<CuentaCorriente> CuentaCorriente { get; set; }
        // Override OnModelsCreating:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationGroup>()
                .HasMany<ApplicationUserGroup>((ApplicationGroup g) => g.ApplicationUsers)
                .WithRequired().HasForeignKey<string>((ApplicationUserGroup ag) => ag.ApplicationGroupId);
            modelBuilder.Entity<ApplicationUserGroup>()
                .HasKey((ApplicationUserGroup r) =>
                    new
                    {
                        ApplicationUserId = r.ApplicationUserId,
                        ApplicationGroupId = r.ApplicationGroupId
                    }).ToTable("ApplicationUsuarioGrupos");

            modelBuilder.Entity<ApplicationGroup>()
                .HasMany<ApplicationGroupRole>((ApplicationGroup g) => g.ApplicationRoles)
                .WithRequired().HasForeignKey<string>((ApplicationGroupRole ap) => ap.ApplicationGroupId);
            modelBuilder.Entity<ApplicationGroupRole>().HasKey((ApplicationGroupRole gr) =>
                new
                {
                    ApplicationRoleId = gr.ApplicationRoleId,
                    ApplicationGroupId = gr.ApplicationGroupId
                }).ToTable("ApplicationGrupoAcciones");

        }
    }


    // Most likely won't need to customize these either, but they were needed because we implemented
    // custom versions of all the other types:
    public class ApplicationUserStore
        : UserStore<ApplicationUser, ApplicationRole, string,
            ApplicationUserLogin, ApplicationUserRole,
            ApplicationUserClaim>, IUserStore<ApplicationUser, string>,
        IDisposable
    {
        public ApplicationUserStore()
            : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context)
            : base(context)
        {
        }
    }


    public class ApplicationRoleStore
    : RoleStore<ApplicationRole, string, ApplicationUserRole>,
    IQueryableRoleStore<ApplicationRole, string>,
    IRoleStore<ApplicationRole, string>, IDisposable
    {
        public ApplicationRoleStore()
            : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationRoleStore(DbContext context)
            : base(context)
        {
        }
    }


    public class ApplicationGroup
    {
        public ApplicationGroup()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ApplicationRoles = new List<ApplicationGroupRole>();
            this.ApplicationUsers = new List<ApplicationUserGroup>();
        }

        public ApplicationGroup(string name)
            : this()
        {
            this.Name = name;
        }

        public ApplicationGroup(string name, string description)
            : this(name)
        {
            this.Description = description;
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ApplicationGroupRole> ApplicationRoles { get; set; }
        public virtual ICollection<ApplicationUserGroup> ApplicationUsers { get; set; }
    }


    public class ApplicationUserGroup
    {
        public string ApplicationUserId { get; set; }
        public string ApplicationGroupId { get; set; }
    }

    public class ApplicationGroupRole
    {
        public string ApplicationGroupId { get; set; }
        public string ApplicationRoleId { get; set; }
    }
}