using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Book_Reading_Event_Management.Controllers;
using Book_Reading_Event_Management.Models;

namespace Book_Reading_Event_Management.Custom_Validations
{
    public class CustomEmail:ValidationAttribute
    {
        public override bool IsValid(object email)
        {
            UserModel userModel = new UserModel();
            userModel.Email = email.ToString();
            DbmsFetching db = new DbmsFetching();
            bool valid = db.DuplicateEmail(userModel);
            if(valid==true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
    }
}