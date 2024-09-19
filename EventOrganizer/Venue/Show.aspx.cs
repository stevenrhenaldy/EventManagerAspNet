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

    public int VenueId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public int Capacity { get; set; }
    public int Price { get; set; }
    public PricePer Priceper { get; set; }
    public string Image { get; set; }
    public string Created_at { get; set; }
    public DateTime Updated_at { get; set; }



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
            Response.Redirect("~/Venue/Index");
            return;
        }
        int VenueId = Convert.ToInt32(id);

        SqlCommand cmd = new SqlCommand("SELECT * FROM [Venue] WHERE [VenueId] = @id", conn);
        conn.Open();
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = VenueId;
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                VenueId = Convert.ToInt32(reader["VenueId"]);
                Name = String.Format("{0}", reader["Name"]);
                Description = String.Format("{0}", reader["Description"]);
                Image = String.Format("{0}", reader["Image"]);
                Location = String.Format("{0}", reader["Location"]);
                Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")); 
                Price = reader.GetInt32(reader.GetOrdinal("Price"));
                Priceper = (PricePer)reader.GetInt32(reader.GetOrdinal("PricePer"));
                Updated_at = (DateTime)reader["updated_at"];
            }
        }
        conn.Close();
    }


    public enum PricePer
    {
        Hourly,
        Daily,
    }

    protected void CloseModal_OnClick(object sender, EventArgs e)
    {
        string id = Request.QueryString["id"];
        Response.Redirect("Show.aspx?id="+ id);
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


        String description = RentDescription.Text;
        if (description.Length <= 3)
        {
            Session["error"] = "Description shouldn't be less than 3 characters!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }

        String start_time_str = RentStartTime.Text;
        String end_time_str = RentEndTime.Text;

        DateTime start_time = DateTime.Parse(start_time_str);
        DateTime end_time = DateTime.Parse(end_time_str);

        if (end_time <= start_time)
        {
            Session["error"] = "Time Start should be earlier than Time End!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }

        int created_by = Convert.ToInt32(Session["id"]);

        conn.Open();
        SqlCommand cmd;

        cmd = new SqlCommand("INSERT INTO [VenueRent] ([Description], [Start_Time], [End_Time], [Created_by], [Created_at], [VenueId], [status]) VALUES (@description, @start_time, @end_time, @created_by, @created_at, @venue_id, @status);", conn);

        cmd.Parameters.Add("@description", SqlDbType.Text).Value = description;
        cmd.Parameters.Add("@start_time", SqlDbType.DateTime).Value = start_time;
        cmd.Parameters.Add("@end_time", SqlDbType.DateTime).Value = end_time;

        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = created_by;
        cmd.Parameters.Add("@created_at", SqlDbType.DateTime).Value = DateTime.Now;
        cmd.Parameters.Add("@venue_id", SqlDbType.Int).Value = id;
        cmd.Parameters.Add("@status", SqlDbType.Int).Value = 0;


        if (cmd.ExecuteNonQuery() > 0)
        {
            Session["success"] = "Venue rental form has been submitted! Please wait for the confirmation email!";
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