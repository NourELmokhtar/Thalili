using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thalili.Models;

namespace Thalili.Controllers
{
    public class CartController : Controller
    {
        ThaliliEntities context = new ThaliliEntities();
        // GET: Cart
        public ActionResult Index()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Login");
            int user_id = (int)Session["UserID"];
            var cart = context.carts.Where(d => d.user_id == user_id).ToList();
            return View(cart);
        }
        public ActionResult edit(int analysis_id, int Lab_id, int count)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Login");
            int user_id = (int)Session["UserID"];
            context.carts.Where(d => d.analysis_id == analysis_id && d.Lab_id == Lab_id && d.user_id == user_id).FirstOrDefault().count = count;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult deleteItem(int analysis_id, int Lab_id)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Login");
            int user_id = (int)Session["UserID"];
            cart cart = context.carts.Where(d => d.analysis_id == analysis_id && d.Lab_id == Lab_id && d.user_id == user_id).FirstOrDefault();
            context.carts.Remove(cart);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult completeRequst(string Adress)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Login");
            int user_id = (int)Session["UserID"];
            List<sub_order> suborders = new List<sub_order>();
            order order = new order();
            order.location = Adress;
            order.date = DateTime.Now;
            order.is_sent = false;
            context.orders.Add(order);
            var carts = context.carts.Where(d => d.user_id == user_id).ToList();
            foreach (cart crt in carts)
            {
                sub_order sub_order = new sub_order();
                sub_order.user_id = user_id;
                sub_order.medical_analysis_id = crt.analysis_id;
                sub_order.lab_id = crt.Lab_id;
                sub_order.count = crt.count;
                var subord = context.sub_order.Where(x => x.lab_id == crt.Lab_id && x.user_id == user_id && x.medical_analysis_id == crt.analysis_id).FirstOrDefault();
                if (subord != null)
                {
                    if (subord.is_finshed == true)
                    {
                        context.sub_order.Remove(subord);
                        order.sub_order.Add(sub_order);
                    }
                    else
                        context.sub_order.Where(x => x.lab_id == crt.Lab_id && x.user_id == user_id && x.medical_analysis_id == crt.analysis_id).FirstOrDefault().count += crt.count;
                }
                else
                {
                    order.sub_order.Add(sub_order);
                }
                context.carts.Remove(crt);
            }
            if (order.sub_order.Count == 0)
                context.orders.Remove(order);
            context.SaveChanges();
            return View();
        }
    }
}