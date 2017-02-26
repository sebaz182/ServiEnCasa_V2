using Microsoft.AspNet.Identity;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;


namespace SeguridadWebv2.Services
{
    [Authorize(Roles = "Admin, AllGrupos")]
    public class ReportServicesController : Controller
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


        #region Internal Classes
        private class PieData
        {
            public string label;
            public int data;
        }
        private class DonutData
        {
            public string label;
            public int value;
        }
        private class BarData
        {
            public string device;
            public int geekbench;
        }
        #endregion

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string BarChars()
        {
            List<BarData> datalist = new List<BarData>();

            string ps1 = "Electricista";
            string ps2 = "Gasista";
            string ps3 = "Cerrajero";
            string ps4 = "Service AA";
            string ps5 = "Serv Domestico";

            int p1 = dbContext.Solicitudes.Where(x => x.Profesiones.Id_Profesion == 1).Count();
            int p2 = dbContext.Solicitudes.Where(x => x.Profesiones.Id_Profesion == 2).Count();
            int p3 = dbContext.Solicitudes.Where(x => x.Profesiones.Id_Profesion == 3).Count();
            int p4 = dbContext.Solicitudes.Where(x => x.Profesiones.Id_Profesion == 4).Count();
            int p5 = dbContext.Solicitudes.Where(x => x.Profesiones.Id_Profesion == 5).Count();

            datalist.Add(new BarData { device = ps1, geekbench = p1 });
            datalist.Add(new BarData { device = ps2, geekbench = p2 });
            datalist.Add(new BarData { device = ps3, geekbench = p3 });
            datalist.Add(new BarData { device = ps4, geekbench = p4 });
            datalist.Add(new BarData { device = ps5, geekbench = p5 });


            return new JavaScriptSerializer().Serialize(datalist);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DonutCharts()
        {
            List<DonutData> datalist = new List<DonutData>();

            string ps1 = "Usuarios";
            string ps2 = "Servis";

            int p1 = dbContext.Users.Count();
            int p2 = dbContext.Servis.Count();


            datalist.Add(new DonutData { label = ps1, value = p1 });
            datalist.Add(new DonutData { label = ps2, value = p2 });


            return new JavaScriptSerializer().Serialize(datalist);
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string PieChart()
        {
            List<PieData> datalist = new List<PieData>();

            //Get Data           
            //foreach (var profesion in dbContext.Profesiones)
            //{
            //    var profesionales = dbContext.ServisProfesiones.Where(x => x.Profesion.Id_Profesion == profesion.Id_Profesion).ToList();

            //    datalist.Add(new PieData { label = profesion.Desc_Profesion.ToString(), data = profesionales.Count() });
            //}
            


            int p1 = dbContext.ServisProfesiones.Where(x => x.Profesion.Id_Profesion == 1).Count();
            int p2 = dbContext.ServisProfesiones.Where(x => x.Profesion.Id_Profesion == 2).Count();
            int p3 = dbContext.ServisProfesiones.Where(x => x.Profesion.Id_Profesion == 3).Count();
            int p4 = dbContext.ServisProfesiones.Where(x => x.Profesion.Id_Profesion == 4).Count();
            int p5 = dbContext.ServisProfesiones.Where(x => x.Profesion.Id_Profesion == 5).Count();

            string ps1 = "Electricista: " + p1.ToString();
            string ps2 = "Gasista: " + p2.ToString();
            string ps3 = "Cerrajero: " + p3.ToString();
            string ps4 = "Service Aire Acondicionado: " + p4.ToString();
            string ps5 = "Servicio Domestico: " + p5.ToString();
            
            datalist.Add(new PieData { label = ps1, data = p1 });
            datalist.Add(new PieData { label = ps2, data = p2 });
            datalist.Add(new PieData { label = ps3, data = p3 });
            datalist.Add(new PieData { label = ps4, data = p4 });
            datalist.Add(new PieData { label = ps5, data = p5 });

            return new JavaScriptSerializer().Serialize(datalist);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SolicitudesCount()
        {
            int solicitudes = 0;
            
            //Get Comments
            solicitudes = dbContext.Solicitudes.Count(); ;

            return new JavaScriptSerializer().Serialize(solicitudes);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string PresupuestosCount()
        {
            int presupuestos = 0;

            //Get Comments
            presupuestos = dbContext.Presupuestos.Count();

            return new JavaScriptSerializer().Serialize(presupuestos);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ServiciosCount()
        {
            int servicios = 0;


            //Get Comments
            servicios = dbContext.Servicios.Count();

            return new JavaScriptSerializer().Serialize(servicios);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string NoPresupuestadosCount()
        {
            int NoPresupuestados = 0;

            //Get Comments
            NoPresupuestados = dbContext.Solicitudes.Where(x => x.Contador == 0).Count();

            return new JavaScriptSerializer().Serialize(NoPresupuestados);
        }
    }
}