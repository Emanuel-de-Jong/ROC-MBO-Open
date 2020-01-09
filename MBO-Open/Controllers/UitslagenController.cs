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
    [Authorize(Roles = "Admin")]
    public class UitslagenController : Controller
    {
        private MBOOpenEntities db = new MBOOpenEntities();

        public ActionResult Index()
        {
            // Gives all wedstrijden to the view so it can display it
            var wedstrijdens = db.Wedstrijdens.Include(w => w.Speler).Include(w => w.Speler1).Include(w => w.Speler2).Include(w => w.Toernooien);
            return View(wedstrijdens.ToList());
        }

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
            // Gives the two spelers playing the wedstrijd to the view so the user can choose which one won
            List<Speler> Winnaarchoices = new List<Speler> { wedstrijden.Speler, wedstrijden.Speler1 };
            ViewBag.WinnaarID = new SelectList(Winnaarchoices, "ID", "Voornaam", wedstrijden.WinnaarID);
            return View(wedstrijden);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Score1,Score2,WinnaarID")] Wedstrijden wedstrijden)
        {
            // Changes the existing wedstrijd with the information given by the user
            var newWedstrijd = db.Wedstrijdens.Find(wedstrijden.ID);
            newWedstrijd.Score1 = wedstrijden.Score1;
            newWedstrijd.Score2 = wedstrijden.Score2;
            newWedstrijd.WinnaarID = wedstrijden.WinnaarID;

            if (ModelState.IsValid)
            {
                // Pushes the edited data to the database
                db.Entry(newWedstrijd).State = EntityState.Modified;
                db.SaveChanges();

                Session["message"] = "De uitslag is succesvol bijgewerkt";

                return RedirectToAction("Index");
            }

            // The modelstate is not valid so we go back to the view
            Session["message"] = "Bewerken mislukt";
            ViewBag.WinnaarID = new SelectList(db.Spelers, "ID", "Voornaam", wedstrijden.WinnaarID);
            return View(wedstrijden);
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
