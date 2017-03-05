using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeguridadWebv2.Models.App;

namespace SeguridadWebv2.Patterns
{
    class ConcreteServi : Servi
    {
        private DateTime _serviState;

        // Gets or sets subject state
        public DateTime ServiState
        {
            get { return _serviState; }
            set
            {
                if (_serviState < value)
                {
                    _serviState = value;
                    Notify();
                }
            }
        }

        public ConcreteServi(Servis servi)
        {
            _servi = servi;
        }
    }
}