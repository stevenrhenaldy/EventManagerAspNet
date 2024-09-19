using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Microsoft.Ajax.Utilities;
using System.IO;
using System.Xml.Linq;

public partial class Venue_Reservation_Client : Page
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
        SqlCommand cmd = new SqlCommand("SELECT vr.*, [Venue].Name AS venuename " +
            "FROM [VenueRent] vr " +
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

}