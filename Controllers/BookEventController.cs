using Book_Reading_Event_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Book_Reading_Event_Management.Controllers
{
    public class BookEventController : Controller
    {
        // GET: BookEvent
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddEvent(string email)
        {

            if (email != null)
            {
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
                return View();
            }
            else
            {
                return View("~/Views/User/InvalidSession.cshtml");
            }
        }
        [HttpPost]
        public ActionResult AddEvent(BookEventModel bookEvent, string email)
        {

            try
            {
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
                if (ModelState.IsValid)
                {
                    DbmsFetching dbmsFetching = new DbmsFetching();
                    if (dbmsFetching.AddBookEvent(bookEvent, email))
                    {
                        ViewBag.Message = "Book Event Added Successfully!!";

                    }
                    else
                    {
                        ViewBag.Message = "Book Event Added Failed!!";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Fill the required fields correctly";
                }
                //ViewBag.Message = email;
                return View();

            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View();
            }
        }
        
        [HttpGet]
        public ActionResult ViewBookEvents(string title,string email)
        {
            if (email != null)
            {
                DbmsFetching dbmsFetching = new DbmsFetching();
                EventDetailsModel eventDetailsList = dbmsFetching.ShowEventsDetails(title);
                ViewBag.user = email;
                return View(eventDetailsList);
            }
            else
            {
                return View("~/Views/User/InvalidSession.cshtml");
            }
        }
        [HttpPost]
        public ActionResult ViewBookEvents(EventDetailsModel events, string title,string email)
        {
            DbmsFetching dbmsFetching = new DbmsFetching();
            EventDetailsModel eventDetailsList = dbmsFetching.ShowEventsDetails(title);
            ViewBag.user = email;
            if (dbmsFetching.AddComments(events, title))
            {
                ViewBag.Message = "Comment added";
            }
            else
                ViewBag.Message = "";
            return View(eventDetailsList);
        }

        public ActionResult ViewMyEvents(string user)
        {
            if (user != null)
            {
                ViewBag.useremail = user;
                DbmsFetching dbmsFetching = new DbmsFetching();
                IEnumerable<EventDetailsModel> myEventList = dbmsFetching.showMyEvents(user).ToList();
                if (myEventList.Count() > 0)
                    return View(myEventList);
                else
                    ViewBag.Message = "There are no events you have created.Thanks!!";
                return View(myEventList);
            }
            else
            {
                return View("~/Views/User/InvalidSession.cshtml");
            }
        }

        public ActionResult ViewEventsInvitedTo(string user)
        {
            if (user != null)
            {
                ViewBag.user = user;
                DbmsFetching dbmsFetching = new DbmsFetching();
                IEnumerable<EventDetailsModel> eventsInvitedList = dbmsFetching.ShowEventsInvitedTo(user).ToList();
                if (eventsInvitedList.Count() > 0)
                    return View(eventsInvitedList);
                else
                    ViewBag.Message = "There are no events you are invited to.Thanks!!";
                return View(eventsInvitedList);
            }
            
             else
            {
                    return View("~/Views/User/InvalidSession.cshtml");
                
            }

        }
        [HttpGet]
        public ActionResult UpdateEvent(string oldtitle,string user)
        {
            //-----------------Adding time in dropdown-----------------
            if (user != null)
            {
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
                int val = dbmsFetching.EditingValid(updateBookEvent, oldtitle, user);
                if (val == 0)
                {

                    return View("UpdateEvent", updateBookEvent);
                }
                else if (val == 2)
                {
                    return View("InvalidUpdate");
                }
                else
                {
                    return View("InvalidUserUpdate");
                }
            }
            else
            {
                return View("~/Views/User/InvalidSession.cshtml");
            }

        }
        [HttpPost]
        public ActionResult UpdateEvent(UpdateBookEventModel updateBookEventModel,string oldtitle,string user)
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
                int i = dbmsFetching.EditEvent(updateBookEventModel,oldtitle);
                if (i > 0)
                {
                    ViewBag.Message = "Updation successfull!!";

                }
                else
                {
                    ViewBag.Message ="No Updation";
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