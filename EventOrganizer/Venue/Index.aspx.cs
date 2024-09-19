using System;
using System.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections.Generic;

public partial class Venue_Index_Client : Page
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Convert.ToInt32(Session["role"]) < 1)
        {
            Response.Redirect("~/Default");
        }

    }

    public IEnumerable<IDataRecord> VenueListGenerator()
    {
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM [Venue]", conn);
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                yield return reader;
            }
        }
    }


    public enum PricePer
    {
        Hourly,
        Daily,
    }

    protected void CloseModal_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("index");
    }

    protected void OpenModal_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("index");
    }


}