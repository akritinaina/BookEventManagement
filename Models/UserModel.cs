using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Book_Reading_Event_Management.Custom_Validations;

namespace Book_Reading_Event_Management.Models
{
    public class UserModel
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        [CustomEmail(ErrorMessage ="Sorry!!Email Id already exist!!")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Full name is required")]
        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Must be atleast 5 characters long.", MinimumLength = 5)]
        [System.Web.Security.MembershipPassword(
    MinRequiredNonAlphanumericCharacters = 1,
    MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).",
    ErrorMessage = "Your password must be 5 characters long and contain at least one symbol (!, @, #, etc)."
        )]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}