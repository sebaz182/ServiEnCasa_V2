using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;


namespace SeguridadWebv2.Controllers
{
    public class ReportesController : Controller
    {
        private ModeloContainer db = new ModeloContainer();

        public ActionResult Index()
        {
            return View();
        }
    }
}