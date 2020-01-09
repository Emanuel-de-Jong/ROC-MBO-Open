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
    public class ScholenController : Controller
    {
        private MBOOpenEntities db = new MBOOpenEntities();

        public ActionResult Index()
        {
            // Gives all scholen to the view so it can display it
            return View(db.Scholens.ToList());
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scholen scholen = db.Scholens.Find(id);
            if (scholen == null)
            {
                return HttpNotFound();
            }
            return View(scholen);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        // The parameter is the selected school with some variables edited by the user
        public ActionResult Edit([Bind(Include = "ID,Naam")] Scholen scholen)
        {
            if (ModelState.IsValid)
            {
                // Pushes the edited data to the database
                db.Entry(scholen).State = EntityState.Modified;
                db.SaveChanges();
                Session["message"] = "School succesvol bijgewerkt";
                return RedirectToAction("Index");
            }

            // The modelstate is not valid so we go back to the view
            Session["message"] = "Bewerken mislukt";
            return View(scholen);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scholen scholen = db.Scholens.Find(id);
            if (scholen == null)
            {
                return HttpNotFound();
            }

            // If the school doesn't have spelers
            if(scholen.Spelers.Count == 0)
            {
                // It is allowed to be deleted
                db.Scholens.Remove(scholen);
                db.SaveChanges();
                Session["message"] = "De school is succesvol verwijdert.";
            }
            else
            {
                // Else, we go back to the view
                Session["message"] = "De school heeft spelers dus kan niet verwijdert worden.";
            }

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
