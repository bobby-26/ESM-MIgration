using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class DryDock_DrydockCostSummary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["ORDERID"] = Request.QueryString["projectid"];
            ViewState["VESSELID"] = Request.QueryString["vesselid"];

        }


        radreportbutton.Attributes["onclick"] = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=20&reportcode=DRYDOCKCOSTSUMMARY&projectid=" + ViewState["ORDERID"]+"&d="+RadRadioButtontype.SelectedValue + "&showmenu=0&showword=NO&showexcel=NO','false','800px','600px'); return false;";

    }
}