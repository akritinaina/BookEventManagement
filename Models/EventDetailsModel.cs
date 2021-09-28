using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Book_Reading_Event_Management.Models
{
    public class EventDetailsModel
    {
        
        public string Title { get; set; }

        
        public DateTime Date { get; set; }

      
        public string Location { get; set; }

        [DisplayName("Start Time")]
       
        public string startTime { get; set; }

        public string Type { get; set; }

        [DisplayName("Duration(Hrs)")]
        public float Duration { get; set; }

        
        public string Description { get; set; }

        [DisplayName("Other details")]
        public string otherDetails { get; set; }

        [DisplayName("Count of invitees")]
        public int TotalInvitees { get; set; }

        public string Comments { get; set; }

        [DisplayName("Old Comments")]
        public List<string> oldComments { get; set; }
    }
}