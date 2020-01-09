using System.Data;
using System.Data.Entity;
using System.Net;
using MBO_Open.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MBO_Open.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ImporterenController : Controller
    {
        private MBOOpenEntities db = new MBOOpenEntities();


        public ActionResult Index()
        {
            // Gives all Toernooien to the view so it put them in a select
            ViewBag.Toernooien = new SelectList(db.Toernooiens, "ID", "Omschrijving");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageFile(HttpPostedFileBase file, Toernooien Toernooien)
        {
            // Checks if the file is a xml
            if (!file.FileName.ToLower().EndsWith(".xml"))
            {
                // If it is not, we go back to the view
                Session["message"] = "Het bestand is geen xml";
                return RedirectToAction("Index");
            }

            // Imports the content of the xml that isn't in the database yet, into the database
            ImportXml(file);

            Session["message"] = "Het xml bestand is succesvol geimporteerd";
            return RedirectToAction("Index");
        }

        private void ImportXml(HttpPostedFileBase xml)
        {
            XmlDocument spelers = new XmlDocument();

            spelers.Load(xml.InputStream);

            // Gets the ID of the school in the file
            Nullable<int> schoolId = db.FindScholenIDWithNaam(spelers.DocumentElement.ChildNodes[0]["schoolnaam"].InnerText).ToList()[0];
            // If there is none (e.g. It does not exist yet)
            if(schoolId == null)
            {
                // Creates a new school model
                var newSchool = new Scholen();
                // Fills it with the data in the file
                newSchool.Naam = spelers.DocumentElement.ChildNodes[0]["schoolnaam"].InnerText;
                // And pushes it to the database
                db.Scholens.Add(newSchool);
                db.SaveChanges();
                schoolId = db.FindScholenIDWithNaam(spelers.DocumentElement.ChildNodes[0]["schoolnaam"].InnerText).ToList()[0];
            }

            // Goes through all the spelers
            foreach (XmlNode node in spelers.DocumentElement)
            {
                // Gets the ID of the speler
                Nullable<int> spelerID = db.FindSpelerIDWithNaam(node["spelervoornaam"].InnerText, node["spelerachternaam"].InnerText, node["spelertussenvoegsels"].InnerText).ToList()[0];
                // If there is one, we don't need to add it
                if (spelerID != null)
                {
                    // So we continue
                    continue;
                }

                // Else, it makes a new speler model
                Speler newSpeler = new Speler();

                // Puts the content of the file in it
                if (node.Name.ToLower() == "aanmelding")
                {
                    newSpeler.Voornaam = node["spelervoornaam"].InnerText;
                    newSpeler.Achternaam = node["spelerachternaam"].InnerText;
                    newSpeler.SchoolID = schoolId.GetValueOrDefault();
                    newSpeler.Tussenvoegsels = node["spelertussenvoegsels"].InnerText;
                }

                // And saves it to the database
                db.Spelers.Add(newSpeler);
            }
            db.SaveChanges();
        }
    }
}