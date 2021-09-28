using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Book_Reading_Event_Management.Models;
using System.Data.SqlClient;
using System.Configuration;



namespace Book_Reading_Event_Management.Controllers
{
    public class UserController : Controller
    {
        //BookEventManagementEntities1 entities1 = new BookEventManagementEntities1();
        
        
        public ActionResult LoginHomePage()
        {
            if(Session["user"]!=null)
            {
                DbmsFetching dbmsFetching = new DbmsFetching();
                var viewModel = new MyViewModel();
                viewModel.list1 = dbmsFetching.ShowPastEvents.ToList();
                viewModel.list2 = dbmsFetching.ShowUpcommingEvents.ToList();
                return View(viewModel);
            }
            else
            return View("InvalidSession");

        }
        [HttpGet]
        public ActionResult Login()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginUser loginuser)
        {
            DbmsFetching dbmsFetching = new DbmsFetching();
            int i = dbmsFetching.ValidateUser(loginuser);
            if(i==1)
            {
                Session["user"] = loginuser.Email;
                ViewBag.Message = "Login successfully";
                return Redirect("LoginHomePage");
            }
            else if(i==2)
            {
                return Redirect("AdminHomePage");
            }
            else
            {
                ViewBag.Message = "Login failed";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DbmsFetching dbmsFetching = new DbmsFetching();
                    if (dbmsFetching.AddUser(user))
                    {
                        ViewBag.Message = "Registered successfully";
                        return Redirect("Login");
                    }
                    else
                    {
                        ViewBag.Message = "Email Id already exist!!";
                        return View();
                    }
                }

                return View();
                
            }
            catch
            {
                return View();
            }

            
        }
        public ActionResult AdminHomePage()
        {
            DbmsFetching dbmsFetching = new DbmsFetching();
            var viewModel = new MyViewModel();
            viewModel.list1 = dbmsFetching.ShowAdminPastEvents.ToList();
            viewModel.list2 = dbmsFetching.ShowAdminUpcommingEvents.ToList();
            return View(viewModel);
           
        }
        public ActionResult Logout()
        {
            return RedirectToAction("Index","Home");
        }

    }
}