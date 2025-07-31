using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MBO_Open.Models
{
    public class HandmatigAanmelden
    {
        public Aanmeldingen Aanmeldingen { get; set; }
        public Toernooien Toernooien { get; set; }
    }
}