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

public partial class User_Index : Page
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if(2 != Convert.ToInt32(Session["role"]))
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
        SqlCommand cmd = new SqlCommand("SELECT * FROM [User]", conn);

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

    public enum Role
    {
        Customer = 0,
        Event_Manager = 1,
        Venue_Manager = 2,

    }

    protected void gridService_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "EditModal") return;

        int id = Convert.ToInt32(e.CommandArgument);
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM [User] WHERE [UserId] = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                UserId.Value = String.Format("{0}", reader["UserId"]);
                UsernameHidden.Value = String.Format("{0}", reader["Username"]);
                Username.Text = String.Format("{0}", reader["Username"]);
                Name.Text = String.Format("{0}", reader["Name"]);
                Email.Text = String.Format("{0}", reader["Email"]);
                RoleSelect.SelectedIndex = reader.GetInt32(reader.GetOrdinal("role"));

                btndelete.Enabled = Convert.ToInt32(reader["UserId"]) != Convert.ToInt32(Session["id"]);
            }
        }

        string script = "$('#editModal').modal('show');";
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);

        conn.Close();
    }

    protected void SaveBtn_OnClick(object sender, EventArgs e)
    {
        
        String username = Username.Text;
        string script = "$('#editModal').modal('show');";
        if (username.Length <= 3)
        {
            Session["error"] = "Username shouldn't be less than 3 characters!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }
        String name = Name.Text;
        if (username.Length == 0)
        {
            Session["error"] = "Name shouldn't be empty!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }
        String email = Email.Text;
        if (email.Length == 0)
        {
            Session["error"] = "Email shouldn't be empty!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }
        String password = Password.Text;
        if (password.Length > 0 && password.Length < 8)
        {
            Session["error"] = "Password shouldn't be less than 8 characters!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }

        int role_id = RoleSelect.SelectedIndex;
        int user_id = Int32.Parse(UserId.Value);

        conn.Open();
        SqlCommand cmd;

        if (password.Length > 0)
        {
            var passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");

            cmd = new SqlCommand("UPDATE [User] SET [Username] = @username, [Password] = @password, [Name] = @name, [Role] = @role, [Email] = @email, [Updated_at] = @updated_at WHERE [UserId] = @id", conn);
            cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = passwordHash;
        }
        else
        {
            cmd = new SqlCommand("UPDATE [User] SET [Username] = @username, [Name] = @name, [Role] = @role, [Email] = @email, [Updated_at] = @updated_at WHERE [UserId] = @id", conn);
        }

        cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = user_id;
        cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
        cmd.Parameters.Add("@role", SqlDbType.Int).Value = role_id;
        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
        cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
        cmd.Parameters.Add("@updated_at", SqlDbType.DateTime).Value = DateTime.Now;


        if (cmd.ExecuteNonQuery() > 0)
        {
            Session["success"] = "User " + username + " has been updated!";
        }

        conn.Close();

        Response.Redirect("index");
    }

    protected void CloseModal_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("index");
    }

    protected void DeleteModal_OnClick(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(UserId.Value);
        String username = UsernameHidden.Value;

        ViewState["username"] = username;
        UserIdDelete.Value = UserId.Value;

        string script = "$('#deleteModal').modal('show');";
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
    }

    protected void DeleteBtn_OnClick(object sender, EventArgs e)
    {
        int user_id = Int32.Parse(UserIdDelete.Value);

        if(user_id == Convert.ToInt32(Session["id"]))
        {
            return;
        }

        conn.Open();
        SqlCommand cmd;

        cmd = new SqlCommand("DELETE [User] WHERE [UserId] = @id", conn);
        cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = user_id;


        if (cmd.ExecuteNonQuery() > 0)
        {
            Session["success"] = "User has been deleted!";
        }

        conn.Close();

        Response.Redirect("Index");
    }

}