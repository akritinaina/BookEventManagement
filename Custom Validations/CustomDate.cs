using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Book_Reading_Event_Management.Custom_Validations
{
    public class CustomDate: ValidationAttribute
    {
        public override bool IsValid(object date)
        {
            if(Convert.ToDateTime(date)<DateTime.Today)
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