using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MBO_Open.Controllers
{
    public class MessageController : Controller
    {
        public ActionResult ClearMessage(string prevURL)
        {
            Session["message"] = "";
            return Redirect(prevURL);
        }
    }
}