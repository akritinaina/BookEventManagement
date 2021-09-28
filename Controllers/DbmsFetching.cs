using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Book_Reading_Event_Management.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace Book_Reading_Event_Management.Controllers
{
    public class DbmsFetching
    {
        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DBCS"].ToString();
            con = new SqlConnection(constr);

        }
        public bool DuplicateEmail(UserModel user)
        {
            connection();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from Users where Email=@email", con);
            cmd.Parameters.AddWithValue("@email", user.Email);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
            }

        }
        public bool AddUser(UserModel user)
        {
            bool valid = DuplicateEmail(user);
            if (valid)
            {

                return false;
            }
            else
            {
                connection();
                con.Open();
                SqlCommand cmd = new SqlCommand("Insert into Users values(@Name,@Email,@Password)", con);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Name", user.FullName);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {

                    return true;

                }
                else
                {

                    return false;
                }
            }
        }
        public IEnumerable<UserModel> ShowUsers
        {
            get
            {
                connection();
                List<UserModel> users = new List<UserModel>();

                SqlCommand cmd = new SqlCommand("Select * from Users", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    UserModel user = new UserModel();
                    user.FullName = rdr["Full Name"].ToString();
                    user.Email = rdr["Email"].ToString();
                    user.Password = rdr["Password"].ToString();
                    users.Add(user);
                }
                return users;
            }

        }
        public IEnumerable<EventDetailsModel> ShowPastEvents
        {
            get
            {
                connection();
                List<EventDetailsModel> eventList = new List<EventDetailsModel>();

                SqlCommand cmd = new SqlCommand("Select * from BookEvent where Date<SYSDATETIME() AND Type='Public'", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EventDetailsModel bookEventModel = new EventDetailsModel();
                    bookEventModel.Title = rdr["Title"].ToString();
                    eventList.Add(bookEventModel);
                }
                return eventList;
            }

        }
        public IEnumerable<EventDetailsModel> ShowUpcommingEvents
        {
            get
            {
                connection();
                List<EventDetailsModel> eventList = new List<EventDetailsModel>();

                SqlCommand cmd = new SqlCommand("Select * from BookEvent where Date>=SYSDATETIME() AND Type='Public'", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EventDetailsModel bookEventModel = new EventDetailsModel();
                    bookEventModel.Title = rdr["Title"].ToString();
                    eventList.Add(bookEventModel);
                }
                return eventList;
            }

        }
        public IEnumerable<EventDetailsModel> ShowAdminPastEvents
        {
            get
            {
                connection();
                List<EventDetailsModel> eventList = new List<EventDetailsModel>();

                SqlCommand cmd = new SqlCommand("Select * from BookEvent where Date<SYSDATETIME()", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EventDetailsModel bookEventModel = new EventDetailsModel();
                    bookEventModel.Title = rdr["Title"].ToString();
                    eventList.Add(bookEventModel);
                }
                return eventList;
            }

        }
        public IEnumerable<EventDetailsModel> ShowAdminUpcommingEvents
        {
            get
            {
                connection();
                List<EventDetailsModel> eventList = new List<EventDetailsModel>();

                SqlCommand cmd = new SqlCommand("Select * from BookEvent where Date>=SYSDATETIME()", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EventDetailsModel bookEventModel = new EventDetailsModel();
                    bookEventModel.Title = rdr["Title"].ToString();
                    eventList.Add(bookEventModel);
                }
                return eventList;
            }

        }

        public IEnumerable<EventDetailsModel> showMyEvents(string user)
        {
            

                connection();
                List<EventDetailsModel> eventList = new List<EventDetailsModel>();

                SqlCommand cmd = new SqlCommand("Select * from BookEvent where Email=@email ORDER BY Date DESC", con);
                cmd.Parameters.AddWithValue("@email", user);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EventDetailsModel bookEventModel = new EventDetailsModel();
                    bookEventModel.Title = rdr["Title"].ToString();
                    eventList.Add(bookEventModel);
                }
                return eventList;
            
            

        }
        public int CountInvitees(string title)
        {
            connection();
            con.Open();
            int count=0;
            SqlCommand cmd = new SqlCommand("Select * from EventInvitation where EventTitle=@title", con);
            cmd.Parameters.AddWithValue("@title", title);
            SqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                count++;
            }
            con.Close();
            return count;



        }

        public EventDetailsModel ShowEventsDetails(string title)
        {
            int count = CountInvitees(title);
            connection();
            con.Open();
            //List<EventDetailsModel> eventDetailsList = new List<EventDetailsModel>();
            SqlCommand cmd = new SqlCommand("Select * from BookEvent where Title=@title", con);
            cmd.Parameters.AddWithValue("@title", title);
            SqlDataReader rdr = cmd.ExecuteReader();
            EventDetailsModel eventDetails = new EventDetailsModel();
            while (rdr.Read())
            {
                
                eventDetails.Title = rdr["Title"].ToString();
                eventDetails.Date = Convert.ToDateTime(rdr["Date"]);
                eventDetails.Location = rdr["Location"].ToString();
                eventDetails.startTime = rdr["StartTime"].ToString();
                eventDetails.Duration = Convert.ToInt64(rdr["Duration"]);
                eventDetails.Description = rdr["Description"].ToString();
                eventDetails.otherDetails = rdr["OtherDetails"].ToString();

           }
            con.Close();
            eventDetails.TotalInvitees = count;
            eventDetails.oldComments = ShowOldComments(title).ToList();
            //eventDetailsList.Add(eventDetails);
            return eventDetails;

        }
        public IEnumerable<string>  ShowOldComments(string title)
        {
            connection();
            con.Open();
            List<string> oldcommentslist = new List<string>();
            SqlCommand cmd = new SqlCommand("Select Comments from EventComments where EventTitle=@eventTitle ORDER BY Timestamp DESC", con);
            cmd.Parameters.AddWithValue("@eventTitle", title);
            SqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                oldcommentslist.Add(rdr["Comments"].ToString());
            }
            con.Close();
            return oldcommentslist;

        }
        public bool AddComments(EventDetailsModel eventmodel,string title)
        {
            connection();
            con.Open();
            SqlCommand cmd1 = new SqlCommand("sp_InsertComments", con);
            cmd1.CommandType = System.Data.CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@EventTitle", title);
            cmd1.Parameters.AddWithValue("@Comments", eventmodel.Comments);
            int i = cmd1.ExecuteNonQuery();
            if(i>0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public int ValidateUser(LoginUser user)
        {
            connection();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from Users where Email=@email AND Password=@password", con);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            SqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.HasRows)
            {
                if(user.Email.Equals("myadmin@bookevents.com"))
                {
                    return 2;
                }
                return 1;
            }
            else
            {
                
                return 0;
            }
        }
        public bool DuplicateTitle(BookEventModel book)
        {
            connection();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from BookEvent where Title=@title", con);
            cmd.Parameters.AddWithValue("@title", book.Title);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
            }

        }
        public bool AddBookEvent(BookEventModel bookEvent,string email)
        {
                
                connection();
                con.Open();
                SqlCommand cmd1 = new SqlCommand("sp_AddEvent", con);
                cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Title", bookEvent.Title);
                cmd1.Parameters.AddWithValue("@Date",bookEvent.Date);
                cmd1.Parameters.AddWithValue("@Location",bookEvent.Location);
                cmd1.Parameters.AddWithValue("@StartTime", bookEvent.startTime);
                cmd1.Parameters.AddWithValue("@Duration", bookEvent.Duration);
                cmd1.Parameters.AddWithValue("@Description", bookEvent.Description);
                cmd1.Parameters.AddWithValue("@OtherDetails",bookEvent.otherDetails);
                cmd1.Parameters.AddWithValue("@Type", bookEvent.Type);
                cmd1.Parameters.AddWithValue("@Email", email);
                
                int i =cmd1.ExecuteNonQuery();

                con.Close();
                con.Open();
                SqlCommand cmd2 = new SqlCommand("InsertInviteeEmail", con);
                cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@title", bookEvent.Title);
                cmd2.Parameters.AddWithValue("@inviteeEmail", bookEvent.inviteeEmail);
                cmd2.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {

                    return true;

                }
                else
                {

                    return false;
                }
            
        }
        public IEnumerable<EventDetailsModel> ShowEventsInvitedTo(string user)
        {
            connection();
            con.Open();
            List<EventDetailsModel> inviteesList = new List<EventDetailsModel>();
            SqlCommand cmd = new SqlCommand("Select * from EventInvitation where InviteesEmail=@email", con);
            cmd.Parameters.AddWithValue("@email", user);
            SqlDataReader rdr = cmd.ExecuteReader();
            while(rdr.Read())
            {
                EventDetailsModel eventDetailsModel = new EventDetailsModel();
                eventDetailsModel.Title = rdr["EventTitle"].ToString();
                inviteesList.Add(eventDetailsModel);
            }
            return inviteesList;
            
        }
        public int EditingValidForAdmin(UpdateBookEventModel updateBookEvent, string title)
        {
            connection();
            con.Open();
            int valid;
           
            DateTime date = updateBookEvent.Date;
            
            if (date < DateTime.Today)
            {
                valid = 1;
                return valid;
            }
            else
            {
                valid = 0;
                return valid;
            }
        }
        public int EditingValid(UpdateBookEventModel updateBookEvent,string title,string email)
        {
            connection();
            con.Open();
            int valid;
            SqlCommand cmd = new SqlCommand("Select * from BookEvent where Title=@title AND Email=@email", con);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@email", email);
            SqlDataReader rdr = cmd.ExecuteReader();
            DateTime date = updateBookEvent.Date;
            if(!rdr.HasRows)
            {
                con.Close();
                valid = 1;
                return valid;
            }
            else if(date<DateTime.Today)
            {
                valid = 2;
                return valid; 
            }
            else
            {
                con.Close();
                valid = 0;
                return valid;
            }
        }
        public UpdateBookEventModel GetOldDetails(string title)
        {
            connection();
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from BookEvent where Title=@title", con);
            cmd.Parameters.AddWithValue("@title", title);
            SqlDataReader rdr = cmd.ExecuteReader();
            UpdateBookEventModel updateBookEvent = new UpdateBookEventModel();
            while (rdr.Read())
            {

                updateBookEvent.Title = rdr["Title"].ToString();
                updateBookEvent.Date = Convert.ToDateTime(rdr["Date"]);
                updateBookEvent.Location = rdr["Location"].ToString();
                updateBookEvent.startTime = rdr["StartTime"].ToString();
                updateBookEvent.Duration = Convert.ToInt64(rdr["Duration"]);
                updateBookEvent.Description = rdr["Description"].ToString();
                updateBookEvent.otherDetails = rdr["OtherDetails"].ToString();
                updateBookEvent.Type = rdr["Type"].ToString();
            }
            con.Close();
            return updateBookEvent;

        }
        public int EditEvent(UpdateBookEventModel updateBookEvent,string title)
        {
            connection();
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_UpdateEvent", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Title", updateBookEvent.Title);
            cmd.Parameters.AddWithValue("@oldTitle", title);
            cmd.Parameters.AddWithValue("@Date", updateBookEvent.Date);
            cmd.Parameters.AddWithValue("@Location", updateBookEvent.Location);
            cmd.Parameters.AddWithValue("@StartTime", updateBookEvent.startTime);
            cmd.Parameters.AddWithValue("@Duration", updateBookEvent.Duration);
            cmd.Parameters.AddWithValue("@Description", updateBookEvent.Description);
            cmd.Parameters.AddWithValue("@Otherdetails", updateBookEvent.otherDetails);
            cmd.Parameters.AddWithValue("@Type", updateBookEvent.Type);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            con.Open();
            SqlCommand cmd2 = new SqlCommand("InsertInviteeEmail", con);
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@title", updateBookEvent.Title);
            cmd2.Parameters.AddWithValue("@inviteeEmail", updateBookEvent.inviteeEmail);
            cmd2.ExecuteNonQuery();
            con.Close();
            return i;
        }
        public int EditInviteeEmail(UpdateBookEventModel updateBookEvent)
        {
            con.Open();
            SqlCommand cmd2 = new SqlCommand("InsertInviteeEmail", con);
            cmd2.CommandType = System.Data.CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@title", updateBookEvent.Title);
            cmd2.Parameters.AddWithValue("@inviteeEmail", updateBookEvent.inviteeEmail);
            int i=cmd2.ExecuteNonQuery();
            con.Close();
            return i;
        }
    }
}