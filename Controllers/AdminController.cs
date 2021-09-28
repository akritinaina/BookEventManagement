using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Book_Reading_Event_Management.Models;


namespace Book_Reading_Event_Management.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
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
        [HttpGet]
        public ActionResult UpdateEvent(string oldtitle)
        {
            //-----------------Adding time in dropdown-----------------
            DateTime StartTime = DateTime.ParseExact("00:00", "HH:mm", null);
            // Set the end time (23:55 means 11:55 PM)
            DateTime EndTime = DateTime.ParseExact("23:55", "HH:mm", null);

            //To set 1 hour interval
            TimeSpan Interval = new TimeSpan(1, 0, 0);
            List<SelectListItem> ddlTime = new List<SelectListItem>();


            while (StartTime <= EndTime)
            {
                ddlTime.Add(new SelectListItem { Text = StartTime.ToShortTimeString(), Value = StartTime.ToShortTimeString() });

                StartTime = StartTime.Add(Interval);
            }
            ddlTime.Insert(0, new SelectListItem { Text = "--Select time--", Value = "0" });
            ViewBag.DDLList = ddlTime;
            //----------------------Updating Events-------------------
            DbmsFetching dbmsFetching = new DbmsFetching();
            UpdateBookEventModel updateBookEvent = dbmsFetching.GetOldDetails(oldtitle);
            int val = dbmsFetching.EditingValidForAdmin(updateBookEvent, oldtitle);
            if (val == 0)
            {

                return View("UpdateEvent", updateBookEvent);
            }
            else
            {
                return View("~/Views/Admin/InvalidPastUpdate.cshtml");
            }
            

        }
        [HttpPost]
        public ActionResult UpdateEvent(UpdateBookEventModel updateBookEventModel, string oldtitle)
        {
            //-----------------Adding time in dropdown-----------------
            DateTime StartTime = DateTime.ParseExact("00:00", "HH:mm", null);
            // Set the end time (23:55 means 11:55 PM)
            DateTime EndTime = DateTime.ParseExact("23:55", "HH:mm", null);

            //To set 1 hour interval
            TimeSpan Interval = new TimeSpan(1, 0, 0);
            List<SelectListItem> ddlTime = new List<SelectListItem>();


            while (StartTime <= EndTime)
            {
                ddlTime.Add(new SelectListItem { Text = StartTime.ToShortTimeString(), Value = StartTime.ToShortTimeString() });

                StartTime = StartTime.Add(Interval);
            }
            ddlTime.Insert(0, new SelectListItem { Text = "--Select time--", Value = "0" });
            ViewBag.DDLList = ddlTime;
            //----------------Updating Events-------------------------------
            try
            {
                DbmsFetching dbmsFetching = new DbmsFetching();
                int i = dbmsFetching.EditEvent(updateBookEventModel, oldtitle);
                if (i > 0)
                {
                    ViewBag.Message = "Updation successfull!!";

                }
                else
                {
                    ViewBag.Message = "No Updation";
                }
                return View(updateBookEventModel);
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View(updateBookEventModel);
            }


        }

    }
}