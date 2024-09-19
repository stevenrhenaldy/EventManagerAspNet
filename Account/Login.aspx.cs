using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.UI;
using EventManager;
using System.Data.SqlClient;
using System.Web.Security;
using System.Configuration;

public partial class Account_Login : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterHyperLink.NavigateUrl = "Register";
        var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        if (!String.IsNullOrEmpty(returnUrl))
        {
            RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
        }
    }

    protected void LogIn(object sender, EventArgs e)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        conn.Open();

        String username = UserName.Text;
        String password = Password.Text;
        
        var passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5"); ;


        SqlCommand cmd_check = new SqlCommand("SELECT * FROM [User] WHERE [Username] = @username OR [Email] = @username", conn);
        cmd_check.Parameters.AddWithValue("@username", username);
        using (SqlDataReader reader = cmd_check.ExecuteReader())
        {
            while (reader.Read())
            {
                if (String.Format("{0}", reader["password"]) != passwordHash)
                {
                    Session["error"] = "Incorrect username or password. Please try again!";
                    conn.Close();
                    Response.Redirect(Request.RawUrl);
                    return;
                }

                Session["id"] = reader.GetInt32(reader.GetOrdinal("UserId"));
                Session["name"] = String.Format("{0}", reader["name"]);
                Session["email"] = String.Format("{0}", reader["email"]);
                Session["role"] = reader.GetInt32(reader.GetOrdinal("role"));
                Session["success"] = "Welcome, " + String.Format("{0}", reader["name"]) + "!";

                Response.Redirect("~/");
                return;
            }
        }

        conn.Close();
        Session["error"] = "Incorrect username or password. Please try again!";
        Response.Redirect(Request.RawUrl);


    }
}