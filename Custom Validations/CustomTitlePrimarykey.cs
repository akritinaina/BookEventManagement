using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Book_Reading_Event_Management.Models;
using Book_Reading_Event_Management.Controllers;

namespace Book_Reading_Event_Management.Custom_Validations
{
    public class CustomTitlePrimarykey: ValidationAttribute
    {
        public override bool IsValid(object title)
        {
           
            BookEventModel bookEventModel = new BookEventModel();
            bookEventModel.Title = title.ToString();
            DbmsFetching dbmsFetching = new DbmsFetching();
            bool valid = dbmsFetching.DuplicateTitle(bookEventModel);
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