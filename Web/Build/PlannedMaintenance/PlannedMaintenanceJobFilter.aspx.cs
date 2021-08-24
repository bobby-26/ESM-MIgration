using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PlannedMaintenanceJobFilter : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("Go", "GO",ToolBarDirection.Right);

			MenuJobFilter.MenuList = toolbar.Show();
		}
	}

	protected void JobFilter_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
		Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
		Script += "fnReloadList();";
		Script += "</script>" + "\n";

		if (CommandName.ToUpper().Equals("GO"))
		{
			NameValueCollection criteria = new NameValueCollection();

			criteria.Clear();

			criteria.Add("txtJobCode", txtJobCode.Text);
			criteria.Add("txtJobTitle", txtJobTitle.Text);
			criteria.Add("ucJobClass", ucJobClass.SelectedQuick);
            criteria.Add("ucJobCategory", ucJobCategory.SelectedQuick);
			Filter.CurrentJobFilter = criteria;
			Response.Redirect("../PlannedMaintenance/PlannedMaintenanceJob.aspx", false);
		}

		Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
	}

}
