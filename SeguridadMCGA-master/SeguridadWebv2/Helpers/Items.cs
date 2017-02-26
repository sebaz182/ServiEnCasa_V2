using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Helpers
{
    public class Items
    {
        public string title { get; set; }
        public int quantity { get; set; }
        public string currency_id { get; set; }
        public decimal unit_price { get; set; }
    }
}