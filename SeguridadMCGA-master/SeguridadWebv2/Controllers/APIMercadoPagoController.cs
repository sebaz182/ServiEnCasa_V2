using System.Web.Mvc;
using mercadopago;
using SeguridadWebv2.Helpers;
using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SeguridadWebv2.Controllers
{
    public class APIMercadoPagoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public void MercadoPago()
        {
            MP mp = new MP(ConfigurationManager.AppSettings["MPClientID"], ConfigurationManager.AppSettings["MPSecret"]);

            string item = "{\"items\":[{\"title\":\"Testing\",\"quantity\":1,\"currency_id\":\"ARS\",\"unit_price\":10.5}]}";
            Hashtable preference = mp.createPreference(item);

            var resultado = (Hashtable)((ArrayList)((Hashtable)preference["response"])["items"])[0];
        }


        [HttpPost]
        public ActionResult DoCheckout()
        {
            var pf = new PreferencesMP
            {
                items = new List<Items>()
                {
                    new Items() {
                        currency_id = "ARS",
                        unit_price = 123,
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
                    success = "http://" + Request.Url + Url.RouteUrl("CheckoutStatus"),
                    failure = "http://" + Request.Url + Url.RouteUrl("CheckoutStatus"),
                    pending = "http://" + Request.Url + Url.RouteUrl("CheckoutStatus")
                }
            };
            Hashtable preference = mp.createPreference(JsonConvert.SerializeObject(data));

            string MPRefID = (string)((Hashtable)preference["response"])["id"];

            return Json(new { url = (string)((Hashtable)preference["response"])[ConfigurationManager.AppSettings["MPUrl"]] });
        }

        public class StatusMP
        {
            public StatusMP()
            {
                Items = new List<Items>();
            }

            public string MPRefID { get; set; }
            public string MPCollectionID { get; set; }
            public IList<Items> Items { get; set; }
            public string Status { get; set; }
        }

        [HttpGet]
        public ActionResult CheckoutStatus(string collection_id, string collection_status, string preference_id, string external_reference, string payment_type, string merchant_order_id)
        {
            var pepe = RouteData.Values["preference_id"] + Request.Url.Query;
            var pepe2 = RouteData.Values["collection_status"] + Request.Url.Query;
            var pepe3 = RouteData.Values["collection_id"] + Request.Url.Query;
            float ExchangeRate = 5.5F;

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

                //NotifyUserOrderStatus();
                StatusMP statuscode = new StatusMP()
                {
                    Status = stado,
                    MPCollectionID = collection,
                    MPRefID = order
                };
                return View("Status", statuscode);
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