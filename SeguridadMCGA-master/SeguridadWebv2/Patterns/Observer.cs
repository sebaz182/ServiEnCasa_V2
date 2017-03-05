using SeguridadWebv2.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeguridadWebv2.Patterns
{
    abstract class Observer
    {
        public abstract void Actualizacion(Servis servi);
    }
}