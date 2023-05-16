using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thalili.Models;

namespace Thalili.Controllers
{

    public class HomeController : Controller
    {
        ThaliliEntities Context = new ThaliliEntities();

        public ActionResult Index()
        {
            var labs=Context.labs.OrderByDescending(d => d.lab_rating).Take(4).ToList();
            return View(labs);
        }
    }
}