using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;

public partial class PlannedMaintenanceWorkOrderLogFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            MenuWorkOrderLogFilter.MenuList = toolbar.Show();
        }

    }
    protected void WorkOrderLog_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("txtWorkOrderNumber", txtWorkOrderNumber.Text);
            criteria.Add("txtWorkOrderTitle", txtWorkOrderTitle.Text);
            criteria.Add("txtWorkDoneDateFrom", txtWorkDoneDateFrom.Text);
            criteria.Add("txtWorkDoneDateTo", txtWorkDoneDateTo.Text);
            criteria.Add("ucWorkDoneBy", ucWorkDoneBy.SelectedDiscipline);
            Filter.CurrentWorkOrderLogFilter = criteria;
            Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderLog.aspx", false);
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}

