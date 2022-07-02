using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;



public partial class WebForm1 : System.Web.UI.Page
{
    private const string AntiXsrfTokenKey = "__AntiXsrfToken";
    private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
    private string _antiXsrfTokenValue;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString2"].ConnectionString);
    //SqlConnection user = new SqlConnection(ConfigurationManager.ConnectionStrings["User"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        //user.Open();
        con.Open();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand("Insert into utbl values('" + TextBox1.Text + "','" + TextBox2.Text + "','" + TextBox3.Text + "')", con);
        cmd.ExecuteNonQuery();
        con.Close();
        Label1.Text = "Data has been inserted";
        GridView1.DataBind();
        TextBox1.Text = ""; 
        TextBox2.Text = "";
        TextBox3.Text = "";
     
    }
    /*
    protected void Button2_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand("update utbl set name='"+TextBox2.Text+ "',age'"+TextBox3.Text+"'where Id='"+TextBox1.Text+"'",con);
        cmd.ExecuteNonQuery();
        con.Close();
        Label1.Text = "Data has been updated";
        GridView1.DataBind();
        TextBox1.Text = ""; 
        TextBox2.Text = "";
        TextBox3.Text = "";
     
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand("delete from utbl where Id='" + Convert.ToInt32(TextBox1.Text).ToString() + "'", con);
        cmd.ExecuteNonQuery();
        con.Close();
        Label1.Text = "Data has been deleted";
        GridView1.DataBind();
        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";

    } 
    protected void Button4_Click(object sender, EventArgs e)
    {
        string find = "select * from utbl where (Id like '%' +@Id+'%')";
        SqlCommand cmd = new SqlCommand(find, con);
        cmd.Parameters.Add("@Id",SqlDbType.NVarChar).Value=TextBox4.Text;
        cmd.ExecuteNonQuery();
        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = cmd;
        DataSet ds = new DataSet();
        da.Fill(ds, "Id");
        GridView1.DataSourceID = null;
        GridView1.DataSource = ds;
        GridView1.DataBind();
        con.Close();
        Label1.Text = "data has been selected";
    }*/
    protected void Page_Init(object sender, EventArgs e)
    {
        // The code below helps to protect against XSRF attacks
        var requestCookie = Request.Cookies[AntiXsrfTokenKey];
        Guid requestCookieGuidValue;
        if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
        {
            // Use the Anti-XSRF token from the cookie
            _antiXsrfTokenValue = requestCookie.Value;
            Page.ViewStateUserKey = _antiXsrfTokenValue;
        }
        else
        {
            // Generate a new Anti-XSRF token and save to the cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
            Page.ViewStateUserKey = _antiXsrfTokenValue;

            var responseCookie = new HttpCookie(AntiXsrfTokenKey)
            {
                HttpOnly = true,
                Value = _antiXsrfTokenValue
            };
           if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            } 
            Response.Cookies.Set(responseCookie);
        }

        Page.PreLoad += _Default_Page_PreLoad;
    }

    protected void _Default_Page_PreLoad(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set Anti-XSRF token
            ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
            ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
        }
        else
        {
            // Validate the Anti-XSRF token
            if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
            {
                throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
            }
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand("update utbl set name='"+TextBox2.Text+"',age='" + TextBox3.Text + "'where Id='" + TextBox1.Text + "'", con);
        cmd.ExecuteNonQuery();
        con.Close();
        Label1.Text = "Data has been updated";
        GridView1.DataBind();
        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand("delete from utbl where Id='"+Convert.ToInt32(TextBox1.Text).ToString()+"'", con);
        cmd.ExecuteNonQuery();
        con.Close();
        Label1.Text = "Data has been deleted";
        GridView1.DataBind();
        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        string find = "select * from utbl where (Id like '%' +@Id+'%')";
        SqlCommand cmd = new SqlCommand(find, con);
        cmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = TextBox4.Text;
        cmd.ExecuteNonQuery();
        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = cmd;
        DataSet ds = new DataSet();
        da.Fill(ds, "Id");
        GridView1.DataSourceID = null;
        GridView1.DataSource = ds;
        GridView1.DataBind();
        con.Close();
        Label1.Text = "data has been selected";
    }

    protected void Button8_Click(object sender, EventArgs e)
    {
        string find = "select * from utbl where (Name like '" + TextBox6.Text + "'AND age like'" + TextBox7.Text + "')";
        SqlCommand cmd = new SqlCommand(find, con);
        cmd.Parameters.Add("Name", SqlDbType.NVarChar).Value = TextBox6.Text;
        cmd.ExecuteNonQuery();
        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = cmd;
        DataSet ds = new DataSet();
        da.Fill(ds, "Name");
        GridView1.DataSourceID = null;
        GridView1.DataSource = ds;
        GridView1.DataBind();
        con.Close();
        Label2.Text = "Welcome "+ TextBox6.Text;
    }
}
