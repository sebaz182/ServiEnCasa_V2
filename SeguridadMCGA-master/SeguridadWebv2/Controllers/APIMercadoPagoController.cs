using System.Web.Mvc;
using mercadopago;
using SeguridadWebv2.Helpers;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System;
using Microsoft.AspNet.Identity;
using SeguridadWebv2.Models;
using SeguridadWebv2.Models.App;

namespace SeguridadWebv2.Controllers
{
    public class APIMercadoPagoController : Controller
    {
        private CuentaCorrienteController _CuentaCorriente = new CuentaCorrienteController();
        private ModeloContainer db = new ModeloContainer();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoCheckout()
        {
            decimal _saldo = 0;
            var saldo = Request["saldo"];
            Decimal.TryParse(saldo, out _saldo);

            
            var pf = new PreferencesMP
            {
                items = new List<Items>()
                {
                    new Items() {
                        currency_id = "ARS",
                        unit_price = _saldo,
                        quantity = 1,
                        title = "Pago de comisión por servicios de ServiEnCasa"
                    }
                }
            };
            MP mp = new MP(ConfigurationManager.AppSettings["MPClientID"], ConfigurationManager.AppSettings["MPSecret"]);
            mp.sandboxMode(bool.Parse(ConfigurationManager.AppSettings["MPSandbox"]));
            var data = new
            {
                items = pf.items.Select(i => new { title = i.title, quantity = i.quantity, currency_id = i.currency_id, unit_price = i.unit_price }).ToArray(),
                back_urls = new
                {
                    success = Request.Url.DnsSafeHost + ":1230" + Url.RouteUrl("CheckoutStatus"),
                    failure = Request.Url.DnsSafeHost + ":1230" + Url.RouteUrl("CheckoutStatus"),
                    pending = Request.Url.DnsSafeHost + ":1230" + Url.RouteUrl("CheckoutStatus")
                }
            };
            Hashtable preference = mp.createPreference(JsonConvert.SerializeObject(data));

            string MPRefID = (string)((Hashtable)preference["response"])["id"];

            var _pago = new Pagos();

            _pago.Estado = "Pemdiente";
            _pago.Importe = _saldo;
            _pago.MPRefID = MPRefID;

            db.Pagos.Add(_pago);

            db.SaveChanges();

            return Json(new { url = (string)((Hashtable)preference["response"])[ConfigurationManager.AppSettings["MPUrl"]] });
        }

        [HttpGet]
        public ActionResult CheckoutStatus(string collection_id, string collection_status, string preference_id, string external_reference, string payment_type, string merchant_order_id)
        {
            string mpRefID = Request["preference_id"];
            string status = Request["collection_status"];
            string collectionID = Request["collection_id"];

            if (string.IsNullOrWhiteSpace(mpRefID) || string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(collectionID))
            {
                return Redirect("/");
            }
            else
            {
                string order = mpRefID;
                string collection = collectionID;
                string stado = status;

                string _IdServi = User.Identity.GetUserId();

                //NotifyUserOrderStatus();
                StatusMP statuscode = new StatusMP()
                {
                    Status = stado,
                    MPCollectionID = collection,
                    MPRefID = order
                };

                if (stado == "approved")
                {
                    var _pago = db.Pagos.Where(x => x.MPRefID == order).FirstOrDefault();
                    _pago.Estado = "Aprobado";
                    _CuentaCorriente._generarCredito(_IdServi,mpRefID, _pago.Importe );

                    db.Entry(_pago).State = System.Data.Entity.EntityState.Modified;
                }

                return View("../CuentaCorriente/Status", statuscode);
            }
        }

        [HttpGet]
        public ActionResult Notification()
        {
            string topic = Request["topic"];
            string id = Request["id"];

            MP mp = new MP(ConfigurationManager.AppSettings["MPClientID"], ConfigurationManager.AppSettings["MPSecret"]);
            mp.sandboxMode(bool.Parse(ConfigurationManager.AppSettings["MPSandbox"]));

            Hashtable paymentInfo = mp.getPaymentInfo(id);

            //NotifyUserOrderStatus();
            //NotifyBuyerOrderStatus();

            //NotifyUserOrderStatus();

            string Status = ((Hashtable)((Hashtable)paymentInfo["response"])["collection"])["status"].ToString();


            return Json(new { status = "OK" }, JsonRequestBehavior.AllowGet);
        }
    }
}