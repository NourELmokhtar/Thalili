using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thalili.Models;
using Thalili.Helpers;
namespace Thalili.Controllers
{
    public class ProfileController : Controller
    {
        ThaliliEntities context = new ThaliliEntities();
        // GET: Profile
        public ActionResult Index()
        {
            if(Session["UserID"]==null)
                return RedirectToAction("Index", "Login");
            int user_id = (int)Session["UserID"];
            user userr = context.users.Where(d => d.user_id == user_id).FirstOrDefault();
            userr.pass = "**********";
            return View("Profile", userr);
        }
        public ActionResult SaveChanges(user user1)
        {
            int user_id = (int)Session["UserID"];
            user userr = context.users.Where(d => d.user_id == user_id).FirstOrDefault();
            userr.name = user1.name;
            if(user1.pass!= "**********")
            userr.pass = Crypto.Hash(user1.pass);
            userr.phone_number = user1.phone_number;
            context.SaveChanges();
            Session["userName"] = userr.name;
            TempData["SuccessMesssage"] = "تم تعديل البيانات بنجاح";
            return RedirectToAction("Index");

        }
    }
}