using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MBO_Open.Models;


namespace MBO_Open.Controllers
{
    public class SluitenController : Controller
    {
        private MBOOpenEntities db = new MBOOpenEntities();
        // GET: Sluiten
        public ActionResult Index()
        {
            return View();
        }
    }
}