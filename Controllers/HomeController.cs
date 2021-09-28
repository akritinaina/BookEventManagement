using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Book_Reading_Event_Management.Models;

namespace Book_Reading_Event_Management.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DbmsFetching dbmsFetching = new DbmsFetching();
            
            var viewModel = new MyViewModel();
            viewModel.list1 = dbmsFetching.ShowPastEvents.ToList();
            viewModel.list2 = dbmsFetching.ShowUpcommingEvents.ToList();
            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            
            return Redirect("https://cas.nagarro.com/");
        }
        public ActionResult ViewPastEvents()
        {
            DbmsFetching dbmsFetching = new DbmsFetching();
            List<EventDetailsModel> list = dbmsFetching.ShowPastEvents.ToList();
            //ViewBag.useremail = email;
            if (list.Count() > 0)
                return View(list);
            else
                ViewBag.Message = "There are no Events.Thanks!!";
            return View(list);
        }
        public ActionResult ViewUpcommingEvents()
        {
            DbmsFetching dbmsFetching = new DbmsFetching();
            List<EventDetailsModel> list = dbmsFetching.ShowUpcommingEvents.ToList();
            //ViewBag.useremail = email;
            if (list.Count() > 0)
                return View(list);
            else
                ViewBag.Message = "There are no Events.Thanks!!";
            return View(list);
        }
        [HttpGet]
        public ActionResult ViewBookEvents(string title)
        {
            DbmsFetching dbmsFetching = new DbmsFetching();
            EventDetailsModel eventDetailsList = dbmsFetching.ShowEventsDetails(title);
            //ViewBag.user = email;
            return View(eventDetailsList);
        }
        [HttpPost]
        public ActionResult ViewBookEvents(EventDetailsModel events, string title)
        {
            DbmsFetching dbmsFetching = new DbmsFetching();
            EventDetailsModel eventDetailsList = dbmsFetching.ShowEventsDetails(title);
            //ViewBag.user = email;
            if (dbmsFetching.AddComments(events, title))
            {
                ViewBag.Message = "Comment added";
            }
            else
                ViewBag.Message = "";
            return View(eventDetailsList);
        }
    }
}