using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenanceCounterFilter : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("Go", "GO");

			MenuCounterFilter.MenuList = toolbar.Show();
		}
	}

	protected void JobFilter_TabStripCommand(object sender, EventArgs e)
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

            criteria.Add("txtComponentCode", txtComponentCode.Text);
            criteria.Add("txtNameTitle", txtNameTitle.Text);
            Filter.CurrentCounterUpdateFilter = criteria;
            Response.Redirect("../PlannedMaintenance/PlannedMaintenanceCounterUpdate.aspx", false);
		}

		Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
	}

}
