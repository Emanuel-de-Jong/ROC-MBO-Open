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
    public class ToernooioverzichtController : Controller
    {
        private MBOOpenEntities db = new MBOOpenEntities();

        // GET: Toernooioverzicht
        public ActionResult Index()
        {
            ViewBag.Toernooien = new SelectList(db.Toernooiens, "ID", "Omschrijving");
            var wedstrijdens = db.Wedstrijdens.ToList();
            return View(wedstrijdens.ToList());
            
        }

        // GET: Toernooioverzicht/Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
