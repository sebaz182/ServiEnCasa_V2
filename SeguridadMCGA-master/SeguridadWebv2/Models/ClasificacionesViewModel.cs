using SeguridadWebv2.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Models
{
    public class ClasificacionesViewModel
    {

        public Servicios Servicio { get; set; }
        public bool cumplimiento { get; set; }
        public string obs_DelServi { get; set; }
        public int? cal_trabajo { get; set; }
        public int? cal_servi { get; set; }
        public int? cal_usuario { get; set; }
        public string obs_DelUser { get; set; }
        public bool pago { get; set; }

        public int? idCal { get; set; }

    }
}