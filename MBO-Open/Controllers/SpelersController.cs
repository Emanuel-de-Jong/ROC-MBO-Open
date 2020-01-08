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
    public class SpelersController : Controller
    {
        private MBOOpenEntities db = new MBOOpenEntities();

        // GET: Spelers
        public ActionResult Index(string message = "")
        {
            ViewBag.message = message;
            var spelers = db.Spelers.Include(s => s.Scholen);
            return View(spelers.ToList());
        }

        // GET: Spelers/Create
        public ActionResult Create()
        {
            ViewBag.SchoolID = new SelectList(db.Scholens, "ID", "Naam");
            return View();
        }

        // POST: Spelers/Create.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Voornaam,Tussenvoegsels,Achternaam,SchoolID")] Speler speler)
        {
            if (ModelState.IsValid)
            {
                db.Spelers.Add(speler);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SchoolID = new SelectList(db.Scholens, "ID", "Naam", speler.SchoolID);
            return View(speler);
        }

        // GET: Spelers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Speler speler = db.Spelers.Find(id);
            if (speler == null)
            {
                return HttpNotFound();
            }
            ViewBag.SchoolID = new SelectList(db.Scholens, "ID", "Naam", speler.SchoolID);
            return View(speler);
        }

        // POST: Spelers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Voornaam,Tussenvoegsels,Achternaam,SchoolID")] Speler speler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(speler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SchoolID = new SelectList(db.Scholens, "ID", "Naam", speler.SchoolID);
            return View(speler);
        }

        // GET: Spelers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Speler speler = db.Spelers.Find(id);
            if (speler == null)
            {
                return HttpNotFound();
            }
            return View(speler);
        }

        // POST: Spelers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Speler speler = db.Spelers.Find(id);
            db.Spelers.Remove(speler);
            db.SaveChanges();
            return RedirectToAction("Index");
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
