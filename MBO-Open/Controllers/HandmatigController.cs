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
    public class HandmatigController : Controller
    {
        private MBOOpenEntities db = new MBOOpenEntities();

        // GET: Handmatig
        public ActionResult Index()
        {
            var aanmeldingens = db.Toernooiens.ToList();
            return View(aanmeldingens.ToList());
        }

        // GET: Handmatig/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aanmeldingen = db.Aanmeldingens.Where(a => a.ToernooiID == id).OrderBy(b => b.Speler.Scholen.Naam);
            if (aanmeldingen == null)
            {
                return HttpNotFound();
            }

            ViewBag.toernooiID = id;
            return View(aanmeldingen);
        }

        // GET: Handmatig/Create
        public ActionResult Create(int id)
        {
            
            ViewBag.Scholen = new SelectList(db.Scholens, "ID", "Naam");
            ViewBag.SpelersSelect = false;
            HandmatigAanmelden handmatigAanmelden = new HandmatigAanmelden
            {
                Toernooien = db.Toernooiens.Where(t => t.ID == id).FirstOrDefault(),
            };
            return View(handmatigAanmelden);
        }

        // POST: Handmatig/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, int Scholen, int? Spelers)
        {

            Aanmeldingen aanmeldingen = new Aanmeldingen
            {
                ToernooiID = id
            };
            if (Spelers != null)
            {
                aanmeldingen.SpelerID = (int)Spelers;
            }

            HandmatigAanmelden handmatigAanmelden = new HandmatigAanmelden
            {
                Aanmeldingen = aanmeldingen,
                Toernooien = db.Toernooiens.Where(t => t.ID == id).FirstOrDefault(),
            };

            var spelers = from s in db.Spelers
                          where s.SchoolID == Scholen
                          orderby s.Achternaam
                          select new { s.ID, volledigeNaam = s.Voornaam + " " + s.Tussenvoegsels + " " + s.Achternaam };

              ViewBag.Scholen = new SelectList(db.Scholens.Where(s => s.ID == Scholen), "ID", "Naam");
            ViewBag.Spelers = new SelectList(spelers, "ID", "volledigeNaam");
            ViewBag.SpelersSelect = true;

            if (Spelers != null)
            {
                if (db.Toernooiens.Find(aanmeldingen.ToernooiID).Aanmeldingens.Count() <= 32 && db.Toernooiens.Find(aanmeldingen.ToernooiID).Datum > DateTime.Now)
                {
                    db.Aanmeldingens.Add(aanmeldingen);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = id });
                }
                else
                {
                    Session["message"] = "kan niet meer aanmelden voor dit toernooi";
                    return View(handmatigAanmelden);
                }
            }

                return View(handmatigAanmelden);
            

            
        }

        // GET: Handmatig/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aanmeldingen aanmeldingen = db.Aanmeldingens.Find(id);
            if (aanmeldingen == null)
            {
                return HttpNotFound();
            }
            ViewBag.SpelerID = new SelectList(db.Spelers, "ID", "Voornaam", aanmeldingen.SpelerID);
            ViewBag.ToernooiID = new SelectList(db.Toernooiens, "ID", "Omschrijving", aanmeldingen.ToernooiID);
            return View(aanmeldingen);
        }

        // POST: Handmatig/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SpelerID,ToernooiID")] Aanmeldingen aanmeldingen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aanmeldingen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SpelerID = new SelectList(db.Spelers, "ID", "Voornaam", aanmeldingen.SpelerID);
            ViewBag.ToernooiID = new SelectList(db.Toernooiens, "ID", "Omschrijving", aanmeldingen.ToernooiID);
            return View(aanmeldingen);
        }

        // GET: Handmatig/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aanmeldingen aanmeldingen = db.Aanmeldingens.Find(id);
            if (aanmeldingen == null)
            {
                return HttpNotFound();
            }
            return View(aanmeldingen);
        }

        // POST: Handmatig/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Aanmeldingen aanmeldingen = db.Aanmeldingens.Find(id);
            db.Aanmeldingens.Remove(aanmeldingen);
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
