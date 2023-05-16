using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thalili.Models;

namespace Thalili.Controllers
{
    
    public class JoinAsPartnerController : Controller
    {
        
        // GET: JoinAsPartner
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SaveData(request rq)
        {
            using (ThaliliEntities context = new ThaliliEntities()) 
            {
                context.requests.Add(rq);
                context.SaveChanges();
            }
                return View();
        }
    }
}