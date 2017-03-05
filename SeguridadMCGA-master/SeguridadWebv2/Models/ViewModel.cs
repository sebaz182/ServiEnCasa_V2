using SeguridadWebv2.Models.App;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models
{
    public class EditarProfesionViewModels
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Profesión")]
        public string profesion { get; set; }
    }

    public class ServicioContratadoVM
    {
        public Servicios servicio { get; set; }
    }

    public class ReportesVM
    {
        public IEnumerable<Servicios> servicios { get; set; }
        public IEnumerable<Presupuestos> presupuestos { get; set; }
        public IEnumerable<Solicitudes> solicitudes { get; set; }
        public IEnumerable<CuentaCorriente> cuentaCorriente { get; set; }
        public IEnumerable<Pagos> pagos { get; set; }
    }

    public class GeneralConfirmacionVM
    {
        public Solicitudes Solicitud { get; set; }
        public Presupuestos Presupuesto { get; set; }
        public CrearServicioVm CrearServicio { get; set; }
    }

    public class CrearServicioVm
    {
        public int idServicio { get; set; }
    }

    public class GeneralPresupuestoVM
    {
        public Solicitudes Solicitud { get; set; }
        public CrearPresupuestoViewModels CrearPrespuesto { get; set; }
    }

    public class CrearPresupuestoViewModels
    {
        //[Required]
        [Display(Name = "Profesion")]
        public string profesion { get; set; }

        [Required]
        [Display(Name = "Observaciones")]
        public string observaciones { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public int precio { get; set; }

        [Required]
        [Display(Name = "Hora de la Visita")]
        public DateTime hora { get; set; }

        public int idSolicitud { get; set; }
    }

    public class CrearTareaViewModel
    {
        [Required]
        [Display(Name = "Profesión")]
        public int ProfesionID { get; set; }

        [Required]
        [Display(Name = "Tarea")]
        public string tarea { get; set; }
    }

    public class EditarTareaViewModel
    {
        public int id { get; set; }

        [Display(Name = "Profesión")]
        public string profesion { get; set; }

        [Required]
        [Display(Name = "Tarea")]
        public string tarea { get; set; }
    }

    public class CrearUsuarioViewModel
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string apellido { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string mail { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Confirmar E-Mail")]
        [Compare("mail", ErrorMessage = "El E-Mail no coincide")]
        public string confMail { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Telefono")]
        public string telefono { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} deberia tener al menos {2} caracteres.", MinimumLength = 6)]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string pass { get; set; }

        [Required]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("pass", ErrorMessage = "La contraseña no coincide")]
        [DataType(DataType.Password)]
        public string confPass { get; set; }
    }

    public class CreateAdmiViewModel
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string apellido { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-Mail")]
        public string mail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} deberia tener al menos {2} caracteres.", MinimumLength = 6)]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string pass { get; set; }

        [Required]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("pass", ErrorMessage = "La contraseña no coincide")]
        [DataType(DataType.Password)]
        public string confPass { get; set; }
    }

    public class CreateServiViewModel
    {
        public int id { get; set; }

        public string Foto { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string apellido { get; set; }

        [Required]
        [Display(Name = "DNI")]
        public string dni { get; set; }

        [Required]
        [Display(Name = "Matricula")]
        public string matricula { get; set; }

        [Required]
        //[EmailAddress]
        [Display(Name = "E-Mail")]
        public string mail { get; set; }

        [Required]
        //[EmailAddress]
        [Display(Name = "Confirmar E-Mail")]
        [Compare("mail", ErrorMessage = "El E-Mail no coincide")]
        public string confMail { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Telefono")]
        public string telefono { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} deberia tener al menos {2} caracteres.", MinimumLength = 6)]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string pass { get; set; }

        [Required]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("pass", ErrorMessage = "La contraseña no coincide")]
        [DataType(DataType.Password)]
        public string confPass { get; set; }



        [Required]
        [Display(Name = "Profesión")]
        public int ProfesionID { get; set; }

        //[Required]
        [Display(Name = "Tarea")]
        public ICollection<Tareas> Tareas { get; set; }
    }

    public class ZonasViewModel
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Zona")]
        public string zona { get; set; }
    }

    public class ComisionViewModel
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Importe de la Comisión:  $")]
        public decimal importe { get; set; }
    }


    public class SolicitudViewModel
    {

        [Required]
        [Display(Name = "Profesión")]
        public int ProfesionId { get; set; }

        [Required]
        [Display(Name = "Tareas")]
        public int TareaId { get; set; }

        public string Foto { get; set; }

        [Required]
        [Display(Name = "Descripcion Servicio")]
        public string DescripcionServicio { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Display(Name = "Horario")]
        public int idHora { get; set; }

        [Required]
        [Display(Name = "Zona")]
        public string Zona { get; set; }
    }

    public class CrearProfesionViewModels
    {
        public int Id_Profesion { get; set; }
        public string profesion { get; set; }
    }

    public class ProfesionesYTareas
    {
        [Required]
        [Display(Name = "Profesión")]
        public int _idProfesion { get; set; }

        public ICollection<Tareas> TareasList { get; set; }
    }

    public class GeneralProfYTareasVM
    {
        public ICollection<Tareas> Tarea { get; set; }
        public ProfesionesYTareas ProfesionesYTareas { get; set; }
    }

    public class RatingViewModel
    {

        //public bool  { get; set; }
    }
}