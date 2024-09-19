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

public partial class Venue_Index : Page
{
    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (2 != Convert.ToInt32(Session["role"]))
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
        SqlCommand cmd = new SqlCommand("SELECT [Venue].*, [User].name AS usersname FROM [Venue], [User] WHERE [Venue].created_by = [User].UserId", conn);

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

    public enum PricePer
    {
        Hourly,
        Daily,
    }

    protected void CreateModal_OnClick(object sender, EventArgs e)
    {
        string script = "$('#createModal').modal('show');";
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
    }

    protected void CreateBtn_OnClick(object sender, EventArgs e)
    {
        string script = "$('#editModal').modal('show');";


        string folder = Server.MapPath("~/Media/Images/");


        String name = Name.Text;
        if (name.Length <= 3)
        {
            Session["error"] = "Name shouldn't be less than 3 characters!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }
        String description = Description.Text;

        String location = Location.Text;

        String capacity = Capacity.Text;
        String price = Price.Text;
        int priceper = PricePerSelect.SelectedIndex;
        
        
        String newFileName = null;
        String filename = Image.PostedFile.FileName;
        if (!filename.IsNullOrWhiteSpace())
        {

            Guid uuid = Guid.NewGuid();
            string uuidStr = uuid.ToString();


            FileInfo fileinfo = new FileInfo(filename);
            string ext = fileinfo.Extension;

            newFileName = uuidStr + ext;
            string filePath = folder + newFileName;
            Image.PostedFile.SaveAs(filePath);
        }


        int created_by = Convert.ToInt32(Session["id"]);

        conn.Open();
        SqlCommand cmd;

        cmd = new SqlCommand("INSERT INTO [Venue] ([Name], [Description], [Location], [Capacity], [Price], [Priceper], [Image], [Created_by], [Created_at], [Updated_at]) VALUES (@name, @description, @location, @capacity, @price, @priceper, @image, @created_by, @created_at, @updated_at);", conn);

        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
        cmd.Parameters.Add("@description", SqlDbType.Text).Value = description;
        cmd.Parameters.Add("@location", SqlDbType.Text).Value = location;
        cmd.Parameters.Add("@capacity", SqlDbType.Int).Value = capacity;
        cmd.Parameters.Add("@price", SqlDbType.Int).Value = price;
        cmd.Parameters.Add("@priceper", SqlDbType.Int).Value = priceper;
        cmd.Parameters.Add("@image", SqlDbType.Text).Value = newFileName;
        cmd.Parameters.Add("@created_by", SqlDbType.Int).Value = created_by;
        cmd.Parameters.Add("@updated_at", SqlDbType.DateTime).Value = DateTime.Now;
        cmd.Parameters.Add("@created_at", SqlDbType.DateTime).Value = DateTime.Now;


        if (cmd.ExecuteNonQuery() > 0)
        {
            Session["success"] = "Venue " + name + " has been updated!";
        }

        conn.Close();

        Response.Redirect("index");

    }

    protected void gridService_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "EditModal") return;

        int id = Convert.ToInt32(e.CommandArgument);
        conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT * FROM [Venue] WHERE [VenueId] = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                EditVenueId.Value = String.Format("{0}", reader["VenueId"]);
                EditName.Text = String.Format("{0}", reader["Name"]);
                EditDescription.Text = String.Format("{0}", reader["Description"]);
                EditLocation.Text = String.Format("{0}", reader["Location"]);
                EditCapacity.Text = String.Format("{0}", reader["Capacity"]);
                EditPrice.Text = String.Format("{0}", reader["Price"]);
                EditPriceper.SelectedIndex = reader.GetInt32(reader.GetOrdinal("Priceper"));
                EditImageShow.ImageUrl = ResolveUrl("~/Media/Images/" + String.Format("{0}", reader["Image"]));
            }
        }

        string script = "$('#editModal').modal('show');";
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);

        conn.Close();
    }

    protected void SaveBtn_OnClick(object sender, EventArgs e)
    {
        string script = "$('#editModal').modal('show');";


        string folder = Server.MapPath("~/Media/Images/");


        String name = EditName.Text;
        if (name.Length <= 3)
        {
            Session["error"] = "Name shouldn't be less than 3 characters!";
            ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
            return;
        }
        String description = EditDescription.Text;

        String location = EditLocation.Text;

        String capacity = EditCapacity.Text;
        String price = EditPrice.Text;
        int priceper = EditPriceper.SelectedIndex;


        String newFileName = null;
        String filename = EditImage.PostedFile.FileName;
        if (!filename.IsNullOrWhiteSpace())
        {

            Guid uuid = Guid.NewGuid();
            string uuidStr = uuid.ToString();


            FileInfo fileinfo = new FileInfo(filename);
            string ext = fileinfo.Extension;

            newFileName = uuidStr + ext;
            string filePath = folder + newFileName;
            Image.PostedFile.SaveAs(filePath);
        }


        int venue_id = Int32.Parse(EditVenueId.Value);

        conn.Open();
        SqlCommand cmd;

        if (!filename.IsNullOrWhiteSpace())
        {
            cmd = new SqlCommand("UPDATE [Venue] SET [Name] = @name, [Description] = @description, [Location] = @location, [Capacity] = @capacity, [Price] = @price, [Priceper] = @priceper, [Image] = @image, [Updated_at] = @updated_at WHERE [VenueId] = @id", conn);

            cmd.Parameters.Add("@image", SqlDbType.Text).Value = newFileName;
        }
        else
        {
            cmd = new SqlCommand("UPDATE [Venue] SET [Name] = @name, [Description] = @description, [Location] = @location, [Capacity] = @capacity, [Price] = @price, [Priceper] = @priceper, [Updated_at] = @updated_at WHERE [VenueId] = @id", conn);
        }

        cmd.Parameters.Add("@id", SqlDbType.Int).Value = venue_id;
        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
        cmd.Parameters.Add("@description", SqlDbType.Text).Value = description;
        cmd.Parameters.Add("@location", SqlDbType.Text).Value = location;
        cmd.Parameters.Add("@capacity", SqlDbType.Int).Value = capacity;
        cmd.Parameters.Add("@price", SqlDbType.Int).Value = price;
        cmd.Parameters.Add("@priceper", SqlDbType.Int).Value = priceper;
        cmd.Parameters.Add("@updated_at", SqlDbType.DateTime).Value = DateTime.Now;


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

    protected void DeleteModal_OnClick(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(EditVenueId.Value);
        String username = EditNameHidden.Value;

        ViewState["username"] = username;
        UserIdDelete.Value = EditVenueId.Value;

        string script = "$('#deleteModal').modal('show');";
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", script, true);
    }

    protected void DeleteBtn_OnClick(object sender, EventArgs e)
    {
        int user_id = Int32.Parse(UserIdDelete.Value);

        if (user_id == Convert.ToInt32(Session["id"]))
        {
            return;
        }

        conn.Open();
        SqlCommand cmd;

        cmd = new SqlCommand("DELETE [Venue] WHERE [VenueId] = @id", conn);
        cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = user_id;


        if (cmd.ExecuteNonQuery() > 0)
        {
            Session["success"] = "User has been deleted!";
        }

        conn.Close();

        Response.Redirect("Index");
    }

}