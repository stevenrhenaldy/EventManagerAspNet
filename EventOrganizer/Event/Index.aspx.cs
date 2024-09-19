using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.UI;
using EventManager;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using System.Web.Providers.Entities;
using System.Runtime.Remoting.Messaging;
using System.Xml.Linq;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Ajax.Utilities;
using System.Security.Policy;

public partial class Event_Index_Client : Page
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Session["role"]) < 1)
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
        int user_id = Convert.ToInt32(Session["id"]);
        SqlCommand cmd = new SqlCommand("SELECT * FROM [Event] WHERE [Created_by] = @user_id", conn);
        cmd.Parameters.AddWithValue("@user_id", user_id);
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

    protected void CreateModal_OnClick(object sender, EventArgs e)
    {
        string script = "$('#createModal').modal('show');";
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
        //if (!IsPostBack)
        //{
            // Bind data to the GridView
            int user_id = Convert.ToInt32(Session["id"]);

            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT [VenueRent].*, CONCAT( [Venue].name , ' (', Start_Time, ' - ', End_Time , ') ' ) AS text FROM [VenueRent] LEFT JOIN [Venue] ON [Venue].[VenueId] = [VenueRent].[VenueId] WHERE [VenueRent].[Created_by] = @user_id ", conn);
            cmd.Parameters.AddWithValue("@user_id", user_id);
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = conn;
                sda.SelectCommand = cmd;
                using (DataTable dt = new DataTable())
                {
                    sda.Fill(dt);
                    DropDownReservation.DataSource = dt;
                    DropDownReservation.DataBind();
                    DropDownReservation.DataTextField = "text";
                    DropDownReservation.DataValueField = "RentId";
                    DropDownReservation.DataBind();
                }
            }
        //}

    }

    protected void CreateBtn_OnClick(object sender, EventArgs e)
    {
        string script = "$('#createModal').modal('show');";


        string folder = Server.MapPath("~/Media/Images/");


        String name = Name.Text;
        if (name.Length <= 3)
        {
            Session["error"] = "Name shouldn't be less than 3 characters!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }
        
        int venue_reservation_id = Convert.ToInt32(DropDownReservation.SelectedItem.Value);

        DateTime start_time = DateTime.Parse(Time_start.Text);
        DateTime end_time = DateTime.Parse(Time_end.Text);

        int created_by = Convert.ToInt32(Session["id"]);

        if ( start_time > end_time)
        {
            Session["error"] = "Event end time should not be earlier than event start time!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }

        int? venue_capacity = 0;

        conn.Open();

        SqlCommand cmd_check_time = new SqlCommand("SELECT [VenueRent].*, [Venue].[Capacity] AS [Capacity] FROM [VenueRent] LEFT JOIN [Venue] ON [Venue].[VenueId] = [VenueRent].[VenueId] WHERE RentId = @rentid;", conn);

        cmd_check_time.Parameters.Add("@rentid", SqlDbType.Int).Value = venue_reservation_id;

        using (SqlDataReader reader = cmd_check_time.ExecuteReader())
        {
            if (reader.Read())
            {
                DateTime? rent_start_time = reader["start_time"] as DateTime?;
                if (start_time < rent_start_time)
                {
                    Session["error"] = "Event start time should not be earlier than the venue reservation start time!";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
                    return;
                }
                DateTime? rent_end_time = reader["end_time"] as DateTime?;
                if (end_time > rent_end_time)
                {
                    Session["error"] = "Event end time should not be later than the venue reservation end time!";
                    ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
                    return;
                }
                venue_capacity = reader["Capacity"] as Int32?;
            }
        }

        SqlCommand cmd_check = new SqlCommand("SELECT * FROM [Event] WHERE VenueRentId = @rentid AND start_time < @endtime AND end_time > @starttime;", conn);

        cmd_check.Parameters.Add("@rentid", SqlDbType.Int).Value = venue_reservation_id;
        cmd_check.Parameters.Add("@starttime", SqlDbType.DateTime).Value = start_time;
        cmd_check.Parameters.Add("@endtime", SqlDbType.DateTime).Value = end_time;

        using (SqlDataReader reader = cmd_check.ExecuteReader())
        {
            if (reader.Read())
            {
                Session["error"] = "Another event on the same time is found ["+ reader["Name"] as String +"]!";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
                return;
            }
        }

        SqlCommand cmd;

        cmd = new SqlCommand("INSERT INTO [Event] ([Name], [VenueRentId], [Start_Time], [End_time], [Created_by], [Created_at], [Capacity]) VALUES (@name, @venue_rent_id, @start_time, @end_time, @created_by, @created_at, @capacity);", conn);

        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
        cmd.Parameters.Add("@venue_rent_id", SqlDbType.Int).Value = venue_reservation_id;
        cmd.Parameters.Add("@capacity", SqlDbType.Int).Value = venue_capacity;
        cmd.Parameters.Add("@start_time", SqlDbType.DateTime).Value = start_time;
        cmd.Parameters.Add("@end_time", SqlDbType.DateTime).Value = end_time;
        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = created_by;
        cmd.Parameters.Add("@created_at", SqlDbType.DateTime).Value = DateTime.Now;


        if (cmd.ExecuteNonQuery() > 0)
        {
            Session["success"] = "Venue " + name + " has been updated!";
        }

        conn.Close();

        Response.Redirect("index");

    }

    protected void CloseModal_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("index");
    }
}