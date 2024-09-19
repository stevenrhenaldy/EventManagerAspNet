using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Microsoft.Ajax.Utilities;
using System.IO;
using System.Xml.Linq;
using System.IdentityModel.Protocols.WSTrust;
using System.Net.Mail;

public partial class Event_Show_Customer : Page
{

    public int? EventId { get; set; }
    public string Name { get; set; }
    public string Details { get; set; }
    public int? VenueRentId { get; set; }
    public string VenueName { get; set; }
    public DateTime? Start_Time { get; set; }
    public DateTime? End_Time { get; set; }
    public int? Capacity { get; set; }
    public string Image { get; set; }
    public int? Price { get; set; }
    public int? status { get; set; }
    public int? Created_by { get; set; }
    public DateTime? Created_at { get; set; }
    public int Count { get; set; } = 0;

    public bool Checked_in { get; set; } = false;


    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        
        string id = Request.QueryString["id"];
        if (id == null)
        {
            Response.Redirect("~/Index");
            return;
        }
        int? UserId = Session["id"] as Int32?;
        int VenueId = Convert.ToInt32(id);
        SqlCommand cmd = new SqlCommand("SELECT [Event].*, [Venue].name as VenueName FROM [Event] " +
            "LEFT JOIN [VenueRent] ON [VenueRent].RentId = [Event].VenueRentId " +
            "LEFT JOIN [Venue] ON [Venue].VenueId = [VenueRent].VenueId " +
            "WHERE [Event].[EventId] = @event_id AND [Event].[Status] > 0", conn);
        conn.Open();
        cmd.Parameters.Add("@event_id", SqlDbType.Int).Value = VenueId;
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                EventId = reader["EventId"] as Int32?;
                Name = reader["Name"] as String;
                Details = reader["Details"] as String;
                VenueRentId = reader["VenueRentId"] as Int32?;
                VenueName = reader["VenueName"] as String;
                Start_Time = reader["Start_Time"] as DateTime?;
                End_Time = reader["End_Time"] as DateTime?;
                Capacity = reader["Capacity"] as Int32?;
                Image = reader["Image"] as String;
                Price = reader["Price"] as Int32?;
                status = reader["Status"] as Int32?;
                Created_by = reader["Created_by"] as Int32?;
                Created_at = reader["Created_at"] as DateTime?;
            }
        }

        SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) AS count FROM [UserSignupEvent] WHERE [UserSignupEvent].[EventId] = @event_id", conn);
       
        cmdCount.Parameters.Add("@event_id", SqlDbType.Int).Value = VenueId;
        using (SqlDataReader reader = cmdCount.ExecuteReader())
        {
            if (reader.Read())
            {
                Count = (int)reader["count"];
            }
        }

        SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(*) AS count FROM [UserSignupEvent] WHERE [UserSignupEvent].[EventId] = @event_id", conn);

        cmdCheck.Parameters.Add("@event_id", SqlDbType.Int).Value = VenueId;
        using (SqlDataReader reader = cmdCount.ExecuteReader())
        {
            if (reader.Read())
            {
                if ((int)reader["count"] > 0) Checked_in = true;


            }
        }
        conn.Close();

    }



    protected void CloseModal_OnClick(object sender, EventArgs e)
    {
        string id = Request.QueryString["id"];
        Response.Redirect("Event.aspx?id=" + id);

    }

    protected void OpenModal_OnClick(object sender, EventArgs e)
    {
        string script = "$('#createModal').modal('show');";
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
    }

    protected void CreateBtn_OnClick(object sender, EventArgs e)
    {
        string script = "$('#editModal').modal('show');";

        int user_id = Convert.ToInt32(Session["id"]);
        

        SqlCommand cmd;
        cmd = new SqlCommand("INSERT INTO [UserSignupEvent]  (UserId, EventId, Created_at) VALUES (@userid, @eventid, @created_at);", conn);
        conn.Open();

        cmd.Parameters.Add("@eventid", SqlDbType.Int).Value = EventId;
        cmd.Parameters.Add("@userid", SqlDbType.Int).Value = user_id;
        cmd.Parameters.Add("@created_at", SqlDbType.DateTime).Value = DateTime.Now;

        

        if (cmd.ExecuteNonQuery() > 0)
        {
            Session["success"] = "Sign up success!";
        }
        else
        {
            Session["error"] = "Oops! Something wrong!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }

        SqlCommand cmdqry = new SqlCommand("SELECT [UserSignupEvent].SignupId AS singupid, [User].Name AS usersname, [User].Email AS email,  [Event].*, [Venue].name as VenueName FROM [UserSignupEvent] " +
            "LEFT JOIN [Event] ON [UserSignupEvent].EventId = [Event].EventId " +
            "LEFT JOIN [VenueRent] ON [VenueRent].RentId = [Event].VenueRentId " +
            "LEFT JOIN [Venue] ON [Venue].VenueId = [VenueRent].VenueId " +
            "LEFT JOIN [User] ON [User].UserId = [UserSignupEvent].UserId " +
            "WHERE [Event].[EventId] = @event_id AND [Event].[Status] > 0", conn);
        
        cmdqry.Parameters.Add("@event_id", SqlDbType.Int).Value = EventId;
        using (SqlDataReader reader = cmdqry.ExecuteReader())
        {
            if (reader.Read())
            {
                DateTime start_time = (DateTime)reader["Start_time"];
                DateTime end_time = (DateTime)reader["End_time"];
                String mailSubject = "Confirmation: Your Event Reservation";
                String mailBody = "Dear " + reader["usersname"] as String + ",\r\n\r\n" +
                    "We are thrilled to inform you that your registration for the upcoming event, " + reader["Name"] as String + ", has been successfully received. Your presence will undoubtedly add to the vibrancy of the occasion.\r\n\r\n" +
                    "Event Details:\r\n" +
                    "Event Name: "+ reader["Name"] as String + "\r\n" +
                    "Date: "+ (start_time.Date == end_time.Date ? start_time.ToString("yyyy/MM/dd") : start_time.ToString("yyyy/MM/dd") + " - " + end_time.ToString("yyyy/MM/dd")) + "\r\n" +
                    "Time: "+ start_time.ToString("hh:mm tt") + " - " + end_time.ToString("hh:mm tt")  + "\r\n" +
                    "Location: "+ reader["VenueName"] as String + "\r\n\r\n" +
                    "Your reservation details are as follows:\r\n" +
                    "Reservation ID: "+ reader["singupid"] as String + "\r\n\r\n" +
                    "We look forward to welcoming you to the event. " +
                    "Best regards,\r\n" +
                    "Best Venue Manager";
                MailMessage mail = new MailMessage();
                mail.To.Add((String)reader["email"]);
                mail.From = new MailAddress("libralieniqua@gmail.com", "Best Venue Manager");
                mail.Subject = mailSubject;
                mail.Body = mailBody;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("libralieniqua@gmail.com", "nidjkmfnkdgbesqa");
                smtp.EnableSsl = true;

                smtp.Send(mail);
            }
        }

        conn.Close();


        Response.Redirect("Event.aspx?id=" + EventId);

    }


}