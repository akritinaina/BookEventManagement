using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Book_Reading_Event_Management.Custom_Validations;


namespace Book_Reading_Event_Management.Models
{
    public class UpdateBookEventModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date is required")]
        [CustomDate(ErrorMessage="Date should be equal or greater than today")]
        public DateTime Date { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [DisplayName("Start Time")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Start time is required")]
        [CustomStartTime(ErrorMessage = "Start Time is required")]
        public string startTime { get; set; }

        public string Type { get; set; }

        [DisplayName("Duration(Hrs)")]
        [Range(0, 4, ErrorMessage = "Value cannot be negative and cannot be greater than 4hrs")]
        public float Duration { get; set; }

        [StringLength(50, ErrorMessage = "Maximum 50 characters long")]
        public string Description { get; set; }

        [DisplayName("Other details")]
        [StringLength(500, ErrorMessage = "Maximum 500 characters long")]
        public string otherDetails { get; set; }

        [DisplayName("Add Email of invitee(comma seperated emails)")]
        public string inviteeEmail { get; set; }
    }
}