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
    public class ScholenController : Controller
    {
        private MBOOpenEntities db = new MBOOpenEntities();

        // GET: Scholen
        public ActionResult Index()
        {
            return View(db.Scholens.ToList());
        }

        // GET: Scholen/Edit/5
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

        // POST: Scholen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Naam")] Scholen scholen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scholen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scholen);
        }

        // GET: Scholen/Delete/5
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

            if(scholen.Spelers.Count == 0)
            {
                db.Scholens.Remove(scholen);
                db.SaveChanges();
                Session["message"] = "De school is succesvol verwijdert.";
            }
            else
            {
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
