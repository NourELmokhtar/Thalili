using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Thalili.Models;
namespace Thalili.Controllers
{
    public class SearchController : Controller
    {

        ThaliliEntities context = new ThaliliEntities();
        public ActionResult Index(string searchField, int? page)
        {

            var labs = context.labs.Where(d => d.name.Contains(searchField)).ToList();
            var analyss = context.medical_analysis.Where(d => d.name.Contains(searchField)).ToList();
            search_view anal_lab = new search_view(labs, analyss);
            ViewData["searchField"] = searchField;
            if (page == null)
                page = 1;
            ViewData["page"] = page;
            return View("lab_analysis", anal_lab);
        }
        public ActionResult filters(string searchField, string typeofsearch, int rating, int? page)
        {
            search_view result = new search_view();
            if (page == null)
                page = 1;
            ViewData["searchField"] = searchField;
            ViewData["page"] = page;
            #region filters
            if (typeofsearch == "All")
            {
                var labs = context.labs.ToList();
                var anlysis = context.medical_analysis.ToList();
                result.labList = labs;
                result.analysisList = anlysis;
            }
            else if (typeofsearch == "Labs")
            {
                var labs = context.labs.ToList();
                result.labList = context.labs.ToList();

            }
            else if (typeofsearch == "Analysis")
            {
                var anlysis = context.medical_analysis.ToList();
                result.analysisList = context.medical_analysis.ToList();
            }

            if (rating == 1 && result.labList != null)
            {
                result.labList = result.labList.Where(d => d.lab_rating >= 0 && d.lab_rating <= 1).ToList();
            }
            else if (rating == 2 && result.labList != null)
            {
                result.labList = result.labList.Where(d => d.lab_rating > 1 && d.lab_rating <= 2).ToList();
            }
            else if (rating == 3 && result.labList != null)
            {
                result.labList = result.labList.Where(d => d.lab_rating > 2 && d.lab_rating <= 3).ToList();
            }
            else if (rating == 4 && result.labList != null)
            {
                result.labList = result.labList.Where(d => d.lab_rating > 3 && d.lab_rating <= 4).ToList();
            }
            else if (rating == 5 && result.labList != null)
            {
                result.labList = result.labList.Where(d => d.lab_rating > 4 && d.lab_rating <= 5).ToList();
            }

            if (result.labList != null)
            {
                result.labList = result.labList.Where(d => d.name.Contains(searchField)).ToList();
            }
            if (result.analysisList != null)
            {
                result.analysisList = result.analysisList.Where(d => d.name.Contains(searchField)).ToList();
            }
            #endregion
            return View("lab_analysis", result);
        }
    }
}