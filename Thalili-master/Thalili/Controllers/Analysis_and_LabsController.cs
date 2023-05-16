using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Thalili.Models;

namespace Thalili.Controllers
{
    public class Analysis_and_LabsController : Controller
    {
        // GET: Analysis_and_Labs
        ThaliliEntities context = new ThaliliEntities();
        public ActionResult Index(int? page)
        {
            var labs = context.labs.ToList();
            var anlysis = context.medical_analysis.ToList();
            search_view anlysis_labs = new search_view(labs, anlysis);
            if (page == null)
                page = 1;
            ViewData["page"] = page;
            return View("lab_analysis", anlysis_labs);
        }

        public ActionResult lab(int id, int ?page)
        {
            if (page == null)
                page = 1;
            ViewData["page"] = page;
            var analysis = context.analysis_in_lab.Where(d => d.lab.lab_id == id).ToList();
            var rev = context.reviews.Where(d => d.Labs_id == id).ToList();
            ViewBag.rev = rev;
            return View(analysis);
        }

        public ActionResult analysis(int id, int? page)
        {
            if (page == null)
                page = 1;
            ViewData["page"] = page;
            var labs = context.analysis_in_lab.Where(d => d.medical_analysis_id == id).ToList();
            return View(labs);
        }

        public ActionResult AddToCart(int lab_id, int analysis_id)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Login");
            int user_id = (int)Session["UserID"];
            cart cart = new cart();
            cart.analysis_id = analysis_id;
            cart.Lab_id = lab_id;
            cart.user_id = user_id;
            cart.count = 1;
            var crt = context.carts.Where(x => x.Lab_id == lab_id && x.user_id == user_id && x.analysis_id == analysis_id).FirstOrDefault();
            if (crt != null)
            {
                context.carts.Where(x => x.Lab_id == cart.Lab_id && x.user_id == user_id && x.analysis_id == cart.analysis_id).FirstOrDefault().count++;
            }
            else
            {
                context.carts.Add(cart);
            }
            context.SaveChanges();
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult SearchLab(int AnalysisID, string Labname, int? page)
        {
            if (page == null)
                page = 1;
            ViewData["page"] = page;
            var lab = context.analysis_in_lab.Where(d => d.medical_analysis_id == AnalysisID).ToList();
            var analysis_in_lab = context.analysis_in_lab.Where(d => d.medical_analysis_id == AnalysisID && d.lab.name.Contains(Labname)).ToList();
            if (analysis_in_lab.Count == 0)
            {
                ViewBag.isEmpty = true;
                ViewData["page"] = 1;
            }
            else
              lab = analysis_in_lab;

            return View("analysis", lab);
        }
        public ActionResult SearchAnalysis(int LabID, string Analysis, int? page)
        {
            if (page == null)
                page = 1;
            ViewData["page"] = page;

            var analysis = context.analysis_in_lab.Where(d => d.Labs_id == LabID).ToList();
            var analysis_in_lab = context.analysis_in_lab.Where(d => d.Labs_id == LabID && d.medical_analysis.name.Contains(Analysis)).ToList();
            if (analysis_in_lab.Count == 0)
            {
                ViewBag.isEmpty = true;
                ViewData["page"] = 1;
            }
            else
                analysis = analysis_in_lab;

            var rev = context.reviews.Where(d => d.Labs_id == LabID).ToList();
            ViewBag.rev = rev;
            return View("lab", analysis);

        }
        public ActionResult Labsfilter(int filterPrice, int filterRating, int? page, int analysis_id)
        {
            var lab = context.analysis_in_lab.Where(d => d.medical_analysis_id == analysis_id).ToList();
            int from = 0, to = int.MaxValue;
            if (page == null)
                page = 1;

            ViewData["page"] = page;

            if (filterPrice == 1)
            {
                // lab = context.analysis_in_lab.Where(d => d.medical_analysis_id == analysis_id).ToList();
                from = 0;
            }
            else if (filterPrice == 2)
            {
                // var lab = context.analysis_in_lab.Where(d => d.medical_analysis_id == analysis_id && d.price >= 50 && d.price <= 100).ToList();
                from = 50; to = 100;
            }
            else if (filterPrice == 3)
            {
                //lab = context.analysis_in_lab.Where(d => d.medical_analysis_id == analysis_id && d.price >= 100 && d.price <= 200).ToList();
                from = 100; to = 200;
            }
            else if (filterPrice == 4)
            {
                //lab = context.analysis_in_lab.Where(d => d.medical_analysis_id == analysis_id && d.price >= 200 && d.price <= 300).ToList();
                from = 200; to = 300;
            }
            else if (filterPrice == 5)
            {
                //lab = context.analysis_in_lab.Where(d => d.medical_analysis_id == analysis_id && d.price >= 300).ToList();
                from = 300; to = int.MaxValue;
            }

            //Ratings
            ViewBag.isEmpty = false;
            var temp = new List<analysis_in_lab>();
            if (filterRating == 1)
            {
                temp = context.analysis_in_lab.Where(d => d.medical_analysis_id == analysis_id && d.lab.lab_rating <= 1 && d.price >= from && d.price <= to).ToList();
            }
            else if (filterRating == 2)
            {
                temp = context.analysis_in_lab.Where(d => d.medical_analysis_id == analysis_id && d.lab.lab_rating > 1 && d.lab.lab_rating <= 2 && d.price >= from && d.price <= to).ToList();
            }
            else if (filterRating == 3)
            {
                temp = context.analysis_in_lab.Where(d => d.medical_analysis_id == analysis_id && d.lab.lab_rating > 2 && d.lab.lab_rating <= 3 && d.price >= from && d.price <= to).ToList();
            }
            else if (filterRating == 4)
            {
                temp = context.analysis_in_lab.Where(d => d.medical_analysis_id == analysis_id && d.lab.lab_rating > 3 && d.lab.lab_rating <= 4 && d.price >= from && d.price <= to).ToList();
            }
            else if (filterRating == 5)
            {
                temp = context.analysis_in_lab.Where(d => d.medical_analysis_id == analysis_id && d.lab.lab_rating > 4 && d.lab.lab_rating <= 5 && d.price >= from && d.price <= to).ToList();
            }
            else if (filterRating == 6)
            {
                temp = context.analysis_in_lab.Where(d => d.medical_analysis_id == analysis_id && d.price >= from && d.price <= to).ToList();
            }

            if (temp.Count != 0)
                lab = temp;
            else ViewBag.isEmpty = true;
            return View("analysis", lab);
        }
        public ActionResult Make_Review(review rev,int lab_id)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Index", "Login");
            int user_id = (int)Session["UserID"];
            var is_exist = context.reviews.Where(d => d.Labs_id == lab_id && d.user_id == user_id).FirstOrDefault();
            if(is_exist!=null)
            {
                TempData["review_error"] = "انت بالفعل قمت بعمل تقييم";
                return RedirectToAction("lab/"+lab_id);
            }
            review rev1 = new review();
            rev1.comment = rev.comment;
            rev1.rating = rev.rating;
            rev1.user_id = user_id;
            rev1.Labs_id = lab_id;
            int all_rate =(int)context.labs.Where(d => d.lab_id == lab_id).FirstOrDefault().lab_rating;
            int number_of_ratings= context.labs.Where(d => d.lab_id == lab_id).FirstOrDefault().reviews.Count()+1;
            context.labs.Where(d => d.lab_id == lab_id).FirstOrDefault().lab_rating = (all_rate + rev.rating) / number_of_ratings;
            context.reviews.Add(rev1);
            context.SaveChanges();
            return RedirectToAction("lab/"+lab_id);
        }

    }
}