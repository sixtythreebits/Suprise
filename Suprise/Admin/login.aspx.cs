using System;
using Core;
using Core.Utilities;

namespace Admin
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorLabel.Text = "Invalid username or password";
        }

        protected void SignInButton_Click(object sender, EventArgs e)
        {            
            if (UsernameTextBox.Text == AppSettings.AdminUser && PasswordTextBox.Text == AppSettings.AdminPass)
            {
                Session["IsAuthenticated"] = true;
                Response.Redirect("Orders.aspx");
            }
            else
            {
                ErrorLabel.Visible = true;
            }
        }
    }
}