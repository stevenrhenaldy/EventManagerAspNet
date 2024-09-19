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

public partial class Admin_Index : Page
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if(2 != Convert.ToInt32(Session["role"]))
        {
            Response.Redirect("~/");
        }        
    }
    
}