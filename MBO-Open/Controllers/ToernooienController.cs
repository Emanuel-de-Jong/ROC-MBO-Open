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
    public class ToernooienController : Controller
    {
        private MBOOpenEntities db = new MBOOpenEntities();

        // GET: Toernooien
        public ActionResult Index()
        {
            return View(db.Toernooiens.ToList());
        }

        // GET: Toernooien/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Toernooien toernooien = db.Toernooiens.Find(id);
            if (toernooien == null)
            {
                return HttpNotFound();
            }
            return View(toernooien);
        }

        // GET: Toernooien/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Toernooien/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Omschrijving,Datum")] Toernooien toernooien)
        {
            if (ModelState.IsValid)
            {
                db.Toernooiens.Add(toernooien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toernooien);
        }

        // GET: Toernooien/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Toernooien toernooien = db.Toernooiens.Find(id);
            if (toernooien == null)
            {
                return HttpNotFound();
            }
            return View(toernooien);
        }

        // POST: Toernooien/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Omschrijving,Datum")] Toernooien toernooien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toernooien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toernooien);
        }

        // GET: Toernooien/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Toernooien toernooien = db.Toernooiens.Find(id);
            if (toernooien == null)
            {
                return HttpNotFound();
            }
            return View(toernooien);
        }

        // POST: Toernooien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Toernooien toernooien = db.Toernooiens.Find(id);

            try
            {
                db.Toernooiens.Remove(toernooien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.isException = true;
                return View(toernooien);
            }
        }

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
