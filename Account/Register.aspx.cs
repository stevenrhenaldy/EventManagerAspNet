using System;
using EventManager;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using BCrypt.Net;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class Account_Register : System.Web.UI.Page
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        conn.Open();
    }

    [Obsolete]
    protected void CreateUser_Click(object sender, EventArgs e)
    {
        String username = UserName.Text;
        if (username.Length <= 3)
        {
            Session["error"] = "Username shouldn't be less than 3 characters!";
            Response.Redirect("register.aspx");
            return;
        }
        String name = Name.Text;
        if (username.Length == 0)
        {
            Session["error"] = "Name shouldn't be empty!";
            Response.Redirect("register.aspx");
            return;
        }
        String email = Email.Text;
        if (email.Length == 0)
        {
            Session["error"] = "Email shouldn't be empty!";
            Response.Redirect("register.aspx");
            return;
        }
        String password = Password.Text;
        if (password.Length < 8)
        {
            Session["error"] = "Password shouldn't be less than 8 characters!";
            Response.Redirect("register.aspx");
            return;
        }

        String confirm_password = ConfirmPassword.Text;

        if (password != confirm_password)
        {
            Session["error"] = "Please confirm your password!";
            return;
        }

        var passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5"); 
        int role_id = 0;
        DateTime created_at = DateTime.Now;
        DateTime updated_at = DateTime.Now;

        SqlCommand cmd_check = new SqlCommand("SELECT username FROM [User] WHERE [Username] = @username", conn);
        cmd_check.Parameters.AddWithValue("@username", username);
        using (SqlDataReader reader = cmd_check.ExecuteReader())
        {
            while (reader.Read())
            {
                if (String.Format("{0}", reader["username"]) == username)
                {
                    Session["error"] = "Username has been registered. Please try again!";
                    Response.Redirect("register.aspx");
                    return;
                }
            }
        }


        SqlCommand cmd = new SqlCommand("INSERT INTO [User] ([Username], [Password], [Name], [Role], [Email], [Created_at], [Updated_at]) values(@username, @password, @name, @role, @email, @created_at, @updated_at)", conn);
        cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
        cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = passwordHash;
        cmd.Parameters.Add("@role", SqlDbType.Int).Value = role_id;
        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
        cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        cmd.Parameters.Add("@created_at", SqlDbType.DateTime).Value = created_at;
        cmd.Parameters.Add("@updated_at", SqlDbType.DateTime).Value = updated_at;
        cmd.ExecuteNonQuery();
        conn.Close();
        Response.Redirect("login.aspx");

    }
}