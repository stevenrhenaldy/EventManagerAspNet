using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        

    }

    public IEnumerable<IDataRecord> VenueListGenerator()
    {
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT * FROM [Event] WHERE Status > 0", conn);
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                yield return reader;
            }
        }
    }
}