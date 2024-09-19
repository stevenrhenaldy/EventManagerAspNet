using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Microsoft.Ajax.Utilities;
using System.IO;
using System.Xml.Linq;

public partial class Venue_Show_Client : Page
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


    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Session["role"]) < 1)
        {
            Response.Redirect("~/");
        }
        string id = Request.QueryString["id"];
        if (id == null)
        {
            Response.Redirect("./Index");
            return;
        }
        int? UserId = Session["id"] as Int32?;
        int VenueId = Convert.ToInt32(id);
        SqlCommand cmd = new SqlCommand("SELECT [Event].*, [Venue].name as VenueName FROM [Event] " +
            "LEFT JOIN [VenueRent] ON [VenueRent].RentId = [Event].VenueRentId " +
            "LEFT JOIN [Venue] ON [Venue].VenueId = [VenueRent].VenueId " +
            "WHERE [Event].[Created_by] = @user_id AND [Event].[EventId] = @event_id", conn);
        conn.Open();
        cmd.Parameters.Add("@event_id", SqlDbType.Int).Value = VenueId;
        cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = UserId;
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
        if (!this.IsPostBack)
        {
            this.BindGrid();
        

            int st = (int?)status ?? 0;
            String pr = Price == null ? "0" : Price.ToString();
            eventid.Value = EventId.ToString();
            EventName.Text = Name;
            EventStatus.SelectedIndex = (int)st;
            EventDetails.Text = Details;
            EventCapacity.Text = Capacity.ToString();
            EventPrice.Text = pr;
            EventStartTime.Text = Start_Time?.ToString("s");
            EventEndTime.Text = End_Time?.ToString("s");
        }
        conn.Close();

    }

    private void BindGrid()
    {
        
        int user_id = Convert.ToInt32(Session["id"]);
        SqlCommand cmd = new SqlCommand("SELECT [UserSignupEvent].SignupId AS Signupid, [User].* FROM [UserSignupEvent] LEFT JOIN [User] ON [User].[UserId] = [UserSignupEvent].[UserId] WHERE [UserSignupEvent].[EventId] = @event_id", conn);
        cmd.Parameters.AddWithValue("@event_id", EventId);
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
    }


    public enum Status
    {
        Draft,
        Published,
    }

    protected void CloseModal_OnClick(object sender, EventArgs e)
    {
        string id = Request.QueryString["id"];
        Response.Redirect("Show.aspx?id=" + id);

    }

    protected void OpenModal_OnClick(object sender, EventArgs e)
    {
        string script = "$('#createModal').modal('show');";
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
    }

    protected void CreateBtn_OnClick(object sender, EventArgs e)
    {
        string script = "$('#editModal').modal('show');";


        string folder = Server.MapPath("~/Media/Images/");
        string id = Request.QueryString["id"];


        String name = EventName.Text;
        if (name.Length <= 3)
        {
            Session["error"] = "Event Name shouldn't be less than 3 characters!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }

        String det = EventDetails.Text;


        DateTime start_time;
        DateTime end_time;
        try
        {
            String start_time_str = EventStartTime.Text;
            String end_time_str = EventEndTime.Text;
            start_time = DateTime.Parse(start_time_str);
            end_time = DateTime.Parse(end_time_str);

        }
        catch (Exception ex)
        {
            Session["error"] = "Date Time Value Error!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }


        if (end_time <= start_time)
        {
            Session["error"] = "Time Start should be earlier than Time End!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }

        int capacity = Convert.ToInt32(EventCapacity.Text);

        string price_str = EventPrice.Text;
        int price = Convert.ToInt32(price_str);

        int stt = Convert.ToInt32(EventStatus.SelectedValue);

        String newFileName = null;
        String filename = EventImage.PostedFile.FileName;
        if (!filename.IsNullOrWhiteSpace())
        {

            Guid uuid = Guid.NewGuid();
            string uuidStr = uuid.ToString();


            FileInfo fileinfo = new FileInfo(filename);
            string ext = fileinfo.Extension;

            newFileName = uuidStr + ext;
            string filePath = folder + newFileName;
            EventImage.PostedFile.SaveAs(filePath);
        }



        SqlCommand cmd;

        if (newFileName != null)
        {
            cmd = new SqlCommand("UPDATE [Event] SET [Name] = @name, [Details] = @details, [Start_time] = @start_time, [End_time] = @end_time, [Capacity] = @capacity, [Image] = @image, [Price] = @price, [Status] = @status WHERE [EventId] = @eventid;", conn);
            cmd.Parameters.Add("@image", SqlDbType.NVarChar).Value = newFileName;

        }
        else
        {
            cmd = new SqlCommand("UPDATE [Event] SET [Name] = @name, [Details] = @details, [Start_time] = @start_time, [End_time] = @end_time, [Capacity] = @capacity, [Price] = @price, [Status] = @status WHERE [EventId] = @eventid;", conn);

        }
        conn.Open();
        cmd.Parameters.Add("@eventid", SqlDbType.Int).Value = id;
        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
        cmd.Parameters.Add("@details", SqlDbType.Text).Value = det;
        cmd.Parameters.Add("@start_time", SqlDbType.DateTime).Value = start_time;
        cmd.Parameters.Add("@end_time", SqlDbType.DateTime).Value = end_time;
        cmd.Parameters.Add("@capacity", SqlDbType.Int).Value = capacity;
        cmd.Parameters.Add("@price", SqlDbType.Int).Value = price;
        cmd.Parameters.Add("@status", SqlDbType.Int).Value = stt;


        if (cmd.ExecuteNonQuery() > 0)
        {
            Session["success"] = "Event has been updated!";
        }
        else
        {
            Session["error"] = "Oops! Something wrong!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }

        conn.Close();


        Response.Redirect("Show.aspx?id=" + id);

    }


}