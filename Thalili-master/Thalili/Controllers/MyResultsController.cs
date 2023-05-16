using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thalili.Models;

namespace Thalili.Controllers
{
    public class MyResultsController : Controller
    {
        ThaliliEntities context = new ThaliliEntities();
        // GET: MyResults
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Login");
            int user_id = (int)Session["UserID"];
            var results = context.sub_order.Where(d => d.user_id == user_id&&d.order.is_sent==true).ToList();
            return View(results);
        }
    }
}