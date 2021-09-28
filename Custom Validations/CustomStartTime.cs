using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Book_Reading_Event_Management.Custom_Validations
{
    public class CustomStartTime: ValidationAttribute
    {
        public override bool IsValid(object startTime)
        {
            startTime = startTime.ToString();
            if (startTime.Equals("0"))
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