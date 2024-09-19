using System;
using System.Web.UI;
using EventManager;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using System.Net.Mail;

public partial class Venue_Rent : Page
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (2 != Convert.ToInt32(Session["role"]))
        {
            Response.Redirect("~/");
        }

        if (!this.IsPostBack)
        {
            this.BindGrid();
        }
    }

    private void BindGrid()
    {
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT vr.*, [Creator].Username AS creator, [Pic].Username AS pic, [Venue].Name AS venuename " +
            "FROM [VenueRent] vr " +
            "LEFT JOIN [User] [Creator] ON [Creator].UserId = vr.[Created_by] " +
            "LEFT JOIN [User] [Pic] ON [Pic].UserId = vr.[Approved_by] " +
            "LEFT JOIN [Venue] ON [Venue].VenueId = vr.VenueId;", conn);

        using (SqlDataAdapter sda = new SqlDataAdapter())
        {
            cmd.Connection = conn;
            sda.SelectCommand = cmd;
            using (DataTable dt = new DataTable())
            {
                sda.Fill(dt);
                gridService.DataSource = dt;
                gridService.DataBind();
            }

        }

        conn.Close();
    }

    public enum Status
    {
        Review,
        Approved,
        Declined
    }

    protected void gridService_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);
        Status status = Status.Review;

        switch (e.CommandName)
        {
            case "Approve":
                status = Status.Approved;
                break;
            case "Decline":
                status = Status.Declined;
                break;
        }

        int user_id = Convert.ToInt32(Session["id"]);

        conn.Open();

        SqlCommand cmd = new SqlCommand("UPDATE [VenueRent] SET [Approved_by] = @approved_by, [Approved_at] = @approved_at, [Status] = @status WHERE [RentId] = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@status", status);
        cmd.Parameters.AddWithValue("@Approved_by", user_id);
        cmd.Parameters.AddWithValue("@approved_at", DateTime.Now);

        if (cmd.ExecuteNonQuery() > 0)
        {
            Session["success"] = "Rent ID #" + id + " has been updated!";
        }
        else
        {
            Session["error"] = "SQL Error!!";
        }

        

        cmd = new SqlCommand("SELECT vr.*, [Creator].email AS email, [Creator].name AS creatorname, [Venue].Name AS venuename " +
            "FROM [VenueRent] vr " +
            "LEFT JOIN [User] [Creator] ON [Creator].UserId = vr.[Created_by] " +
            "LEFT JOIN [Venue] ON [Venue].VenueId = vr.VenueId  WHERE [RentId] = @id;", conn);
        cmd.Parameters.AddWithValue("@id", id);
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                String mailSubject = "Your Venue Reservation Has Been " + (Status)status + " - [" + (String)reader["venuename"] + "], [" + (DateTime)reader["start_time"] + "]";
                String mailBody = "Dear " + reader["creatorname"] + ",\r\n\r\n" +
                        "We are excited to inform you that your reservation for " + (String)reader["venuename"] + " has been approved! 🎉\r\n\r\n" +
                        "Reservation Details:\r\n\r\n" +
                        "Reservation Number: #" + reader["RentId"] + "\r\n" +
                        "Venue: " + (String)reader["venuename"] + "\r\n" +
                        "Reservation Date: " + (DateTime)reader["start_time"] + "-" + (DateTime)reader["end_time"] + "\r\n\r\n" +
                        "We appreciate your choice of our venue for your event. We look forward to hosting your event and ensuring it's a memorable experience!\r\n\r\n" +
                        "Best regards,\r\n" +
                        "Best Venue Manager";

                if(status == Status.Declined)
                {
                    mailSubject = "Notification: Your Venue Reservation Request";
                    mailBody = "Dear  " + reader["creatorname"] + ",\r\n\r\n" +
                        "We regret to inform you that your reservation request for " + (String)reader["venuename"] + " has been declined.\r\n\r\n" +
                        "Reservation Details:\r\n\r\n" +
                        "Venue: " + (String)reader["venuename"] + "\r\n" +
                        "Requested Date: " + (DateTime)reader["created_at"] + "\r\n\r\n" +
                        "We understand that this may be disappointing, and we sincerely apologize for any inconvenience caused.\r\n"+
                        "Best regards,\r\n" +
                        "Best Venue Manager";
                }



                //Response.Write(mail.Subject);
                //Response.Write(mail.Body);
                //try
                //{
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
                //}
                //catch (Exception ex)
                //{
                //    Session["error"] = "Failed to send email!!";
                //}
            }
        }

        

        conn.Close();


        Response.Redirect("VenueRent");
    }

}