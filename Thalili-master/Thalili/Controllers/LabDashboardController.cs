using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thalili.Models;
using Thalili.Helpers;

namespace Thalili.Controllers
{
    public class LabDashboardController : Controller
    {
        ThaliliEntities context = new ThaliliEntities();


        public ActionResult Orders(int? page)
        {
            if (Session["labID"] == null)
                return RedirectToAction("Index", "Login");
            int lab_id = (int)Session["labID"];
            TempData["avaliable"] = context.labs.Where(d => d.lab_id == lab_id).FirstOrDefault().is_available;
            List<List<sub_order>> allorders = new List<List<sub_order>>();
            if (page == null)
                page = 1;
            ViewData["page"] = page;
            var sub1 = context.sub_order.Where(d => d.lab_id == lab_id&&d.order.is_sent==false).OrderByDescending(d=>d.order_id).ToList();
            allorders.Add(sub1);
            var sub2 = context.sub_order.Where(d => d.lab_id == lab_id && d.order.is_sent == true).OrderByDescending(d=>d.order_id).ToList();
            allorders.Add(sub2);
            return View(allorders);
        }
        public ActionResult SendClient(int? id)
        {
            if (Session["labID"] == null)
                return RedirectToAction("Index", "Login");
            context.orders.Where(d => d.order_id == id).FirstOrDefault().is_sent = true;
            context.SaveChanges();
            return RedirectToAction("Orders");
        }
        public ActionResult Reject_order(int id)
        {
            if (Session["labID"] == null)
                return RedirectToAction("Index", "Login");
            context.orders.Where(d => d.order_id == id).FirstOrDefault().is_accept = false;
            context.SaveChanges();
            return RedirectToAction("Orders");
        }
        public ActionResult UploadPdf(HttpPostedFileBase file,int analysis_id, int user_id)
        {
            if (Session["labID"] == null)
                return RedirectToAction("Index", "Login");
            int lab_id = (int)Session["labID"];
            var fileExtenstion = Path.GetExtension(file.FileName);
            var fileguid = Guid.NewGuid().ToString();
            string pdf_name = fileguid + fileExtenstion;
            context.sub_order.Where(d => d.user_id ==user_id && d.medical_analysis_id == analysis_id && d.lab_id == lab_id).FirstOrDefault().pdf = pdf_name;
            context.sub_order.Where(d => d.user_id == user_id && d.medical_analysis_id == analysis_id && d.lab_id == lab_id).FirstOrDefault().is_finshed = true;
            string filePath = Server.MapPath($"~/Content/Results/{pdf_name}");

            context.SaveChanges();
            file.SaveAs(filePath);

            return RedirectToAction("Orders");
        }
        public ActionResult Analysis(int? page)
        {
            if (Session["labID"] == null)
                return RedirectToAction("Index", "Login");
            int lab_id = (int)Session["labID"];
            if (page == null)
                page = 1;
            ViewData["page"] = page;
            var analysis_in_lab = context.analysis_in_lab.Where(d => d.Labs_id == lab_id).ToList();
            var analysis = context.medical_analysis.ToList();
            ViewBag.analysis = analysis;
            return View(analysis_in_lab);
        }
        public ActionResult EditThalil(string ThalilName, decimal price)
        {
            if (Session["labID"] == null)
                return RedirectToAction("Index", "Login");
            int lab_id = (int)Session["labID"];
            var edit = context.analysis_in_lab.Where(d => d.Labs_id == lab_id && d.medical_analysis.name == ThalilName).FirstOrDefault();
            edit.price = price;
            context.SaveChanges();
            return RedirectToAction("Analysis");
        }

        public ActionResult AddThalil(string ThalilName, decimal price)
        {
            if (Session["labID"] == null)
                return RedirectToAction("Index", "Login");
            int lab_id = (int)Session["labID"];
            try
            {            
                var analysis = context.medical_analysis.Where(d => d.name == ThalilName).FirstOrDefault();
                analysis_in_lab New_Analysis = new analysis_in_lab();
                New_Analysis.Labs_id = lab_id;
                New_Analysis.medical_analysis_id = analysis.medical_analysis_id;
                New_Analysis.price = price;
                context.analysis_in_lab.Add(New_Analysis);
                context.SaveChanges();

                return RedirectToAction("Analysis");
            }
            catch(Exception e)
            {
                TempData["msg"] = "<script>alert('this Medical analysis is already exsists');</script>";
                return RedirectToAction("Analysis");
            }
        }
        public ActionResult DeleteThalil(int id)
        {
            if (Session["labID"] == null)
                return RedirectToAction("Index", "Login");
            int lab_id = (int)Session["labID"];
            var deleteitem = context.analysis_in_lab.Where(d => d.Labs_id == lab_id && d.medical_analysis_id == id).FirstOrDefault();
            context.analysis_in_lab.Remove(deleteitem);
            context.SaveChanges();
            return RedirectToAction("Analysis");
        }

        [HttpGet]
        public ActionResult Settings()
        {
            if (Session["labID"] == null)
                return RedirectToAction("Index", "Login");
            int lab_id = (int)Session["labID"];
            var lab = context.labs.Where(d => d.lab_id == lab_id).FirstOrDefault();
            lab.lab_owner.pass = "***********";
            return View(lab);
        }

        [HttpPost]
        public ActionResult Settings(lab edits)
        {
            if (Session["labID"] == null)
                return RedirectToAction("Index", "Login");
            int lab_id = (int)Session["labID"];
            var lab = context.labs.Where(d => d.lab_id == lab_id).FirstOrDefault();
            lab.name = edits.name;
            lab.phone_number = edits.phone_number;
            lab.description = edits.description;
            lab.location = edits.location;
            if(edits.lab_owner.pass!= "***********")
            lab.lab_owner.pass = Crypto.Hash(edits.lab_owner.pass);
            lab.img = edits.img;
            context.SaveChanges();
            TempData["SuccessMesssage"] = "تم تعديل البيانات بنجاح";

            return RedirectToAction("Settings");
        }
        public ActionResult UploadImage(HttpPostedFileBase file)   //next edit will be **delete temp pic 
        {
            if (Session["labID"] == null)
                return RedirectToAction("Index", "Login");
            if(file ==null)
            {
                return RedirectToAction("Settings");
            }
            var fileExtenstion = Path.GetExtension(file.FileName);
            var fileguid = Guid.NewGuid().ToString();
            var filee = fileguid + fileExtenstion;
            string filePath = Server.MapPath($"~/Content/Images/Labs/{filee}");
            TempData["Image"] = filee;        
            file.SaveAs(filePath);
            return RedirectToAction("Settings");
        }
        public ActionResult Lab_Avaliable(bool is_avaliable)
        {
            int lab_id = (int)Session["labID"];
           
            context.labs.Where(d => d.lab_id == lab_id).FirstOrDefault().is_available = is_avaliable;
            context.SaveChanges();
            return RedirectToAction("Orders");
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}