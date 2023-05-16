using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thalili.Models;

namespace Thalili.Controllers
{
    public class MyOrdersController : Controller
    {
        ThaliliEntities context = new ThaliliEntities();
        // GET: MyOrders
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Login");
            int user_id = (int)Session["UserID"];
            var orders = context.sub_order.Where(d => d.user_id == user_id).ToList();
            return View(orders);
        }
    }
}