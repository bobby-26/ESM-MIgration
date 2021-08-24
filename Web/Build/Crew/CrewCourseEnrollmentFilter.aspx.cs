using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewCourseEnrollmentFilter : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		PhoenixToolbar toolbar = new PhoenixToolbar();
		toolbar.AddButton("Go", "GO");
		toolbar.AddButton("Cancel", "CANCEL");
		CourseEnrollmentFilterMain.AccessRights = this.ViewState;
		CourseEnrollmentFilterMain.MenuList = toolbar.Show();
		
	}

	protected void CourseEnrollmentFilterMain_TabStripCommand(object sender, EventArgs e)
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
			criteria.Add("txtFileno",txtFileno.Text);
			criteria.Add("ddlRank", ddlRank.SelectedRank);
			criteria.Add("txtName", txtName.Text);
			criteria.Add("txtFromDate", txtFromDate.Text);
			criteria.Add("txtToDate", txtToDate.Text);
			Filter.CurrentCourseEnrollmentFilter = criteria;
		}
		ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo');", true);
		//Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
	}
}
