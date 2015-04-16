using System;

namespace Suprise.Admin
{
    public partial class AdminMaster : System.Web.UI.MasterPage
    {
        public string PageTitle { set; get; }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["IsAuthenticated"] == null)
            {
                Response.Redirect("~/");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = PageTitle;
        }
    }
}