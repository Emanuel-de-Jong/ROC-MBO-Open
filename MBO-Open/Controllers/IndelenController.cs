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
    public class IndelenController : Controller
    {
        private MBOOpenEntities db = new MBOOpenEntities();

        // GET: Indelen
        public ActionResult Index()
        {
            var aanmeldingens = db.Toernooiens.ToList();
            return View(aanmeldingens.ToList());
        }

        // GET: Indelen/Details/5
        public ActionResult Details(int? id)
        {

            ViewBag.HeeftWedstrijden = db.Wedstrijdens.Where(w => w.ToernooiID == id).FirstOrDefault();

            return View();
        }

        // GET: Indelen/Create
        public ActionResult Create(int id)
        {

            List<int> spelers = (from s in db.Spelers
                                 join a in db.Aanmeldingens on s.ID equals a.SpelerID
                                 where a.ToernooiID == id
                                 select s.ID).ToList();

            List<int?> Spelers2 = new List<int?> { };

            for(int x = 0; x < 32; x++)
            {
                if (x > spelers.Count()-1) {
                    Spelers2.Add(null);
                } 
                else
                {
                    Spelers2.Add(spelers[x]);
                }
            }




                return View();
        }

        // POST: Indelen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ToernooiID,Ronde,Speler1ID,Speler2ID,Score1,Score2,WinnaarID")] Wedstrijden wedstrijden)
        {
            if (ModelState.IsValid)
            {
                db.Wedstrijdens.Add(wedstrijden);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Speler1ID = new SelectList(db.Spelers, "ID", "Voornaam", wedstrijden.Speler1ID);
            ViewBag.Speler2ID = new SelectList(db.Spelers, "ID", "Voornaam", wedstrijden.Speler2ID);
            ViewBag.WinnaarID = new SelectList(db.Spelers, "ID", "Voornaam", wedstrijden.WinnaarID);
            ViewBag.ToernooiID = new SelectList(db.Toernooiens, "ID", "Omschrijving", wedstrijden.ToernooiID);
            return View(wedstrijden);
        }

        // GET: Indelen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wedstrijden wedstrijden = db.Wedstrijdens.Find(id);
            if (wedstrijden == null)
            {
                return HttpNotFound();
            }
            ViewBag.Speler1ID = new SelectList(db.Spelers, "ID", "Voornaam", wedstrijden.Speler1ID);
            ViewBag.Speler2ID = new SelectList(db.Spelers, "ID", "Voornaam", wedstrijden.Speler2ID);
            ViewBag.WinnaarID = new SelectList(db.Spelers, "ID", "Voornaam", wedstrijden.WinnaarID);
            ViewBag.ToernooiID = new SelectList(db.Toernooiens, "ID", "Omschrijving", wedstrijden.ToernooiID);
            return View(wedstrijden);
        }

        // POST: Indelen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ToernooiID,Ronde,Speler1ID,Speler2ID,Score1,Score2,WinnaarID")] Wedstrijden wedstrijden)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wedstrijden).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Speler1ID = new SelectList(db.Spelers, "ID", "Voornaam", wedstrijden.Speler1ID);
            ViewBag.Speler2ID = new SelectList(db.Spelers, "ID", "Voornaam", wedstrijden.Speler2ID);
            ViewBag.WinnaarID = new SelectList(db.Spelers, "ID", "Voornaam", wedstrijden.WinnaarID);
            ViewBag.ToernooiID = new SelectList(db.Toernooiens, "ID", "Omschrijving", wedstrijden.ToernooiID);
            return View(wedstrijden);
        }

        // GET: Indelen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wedstrijden wedstrijden = db.Wedstrijdens.Find(id);
            if (wedstrijden == null)
            {
                return HttpNotFound();
            }
            return View(wedstrijden);
        }

        // POST: Indelen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wedstrijden wedstrijden = db.Wedstrijdens.Find(id);
            db.Wedstrijdens.Remove(wedstrijden);
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
