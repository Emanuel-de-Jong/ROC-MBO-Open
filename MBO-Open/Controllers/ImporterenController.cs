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
    public class ImporterenController : Controller
    {
        private MBOOpenEntities db = new MBOOpenEntities();

        public ActionResult ManageFile(HttpPostedFileBase file, string prevURL)
        {
            if (!file.FileName.ToLower().EndsWith(".xml"))
            {
                Session["message"] = "Het bestand is geen xml";
                return Redirect(prevURL);
            }

            //ImportXml(file);

            Session["message"] = "Het xml bestand is succesvol geimporteerd";
            return Redirect(prevURL);
        }

        private void ImportXml(HttpPostedFileBase xml)
        {
            XmlDocument spelers = new XmlDocument();

            spelers.Load(xml.InputStream);
            foreach (XmlNode node in spelers.DocumentElement)
            {
                Speler newSpeler = new Speler();

                List<Int32> schoolIDsWithName = db.FindScholenIDWithNaam(node.ChildNodes[2].InnerText).Select(i => i.GetValueOrDefault(0)).ToList();
                if (node.Name.ToLower() == "aanmelding")
                {
                    newSpeler.Voornaam = node.ChildNodes[0].InnerText;
                    newSpeler.Achternaam = node.ChildNodes[1].InnerText;
                    newSpeler.SchoolID = schoolIDsWithName[0];
                    newSpeler.Tussenvoegsels = node.ChildNodes[3].InnerText;
                }

                db.Spelers.Add(newSpeler);
                db.SaveChanges();
            }
        }
    }
}