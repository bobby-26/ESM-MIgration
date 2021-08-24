using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class AccountsCtmReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["vesselid"].ToString() != "" && Request.QueryString["captaincashid"].ToString() != "")
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=5&showexcel=no&showword=no&reportcode=CTM&vesselid=" + Request.QueryString["vesselid"].ToString() + "&captaincashid=" + Request.QueryString["captaincashid"].ToString());
            }
        }

    }
}
