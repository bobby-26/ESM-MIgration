using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;

public partial class RegistersBatchFilter : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO");
        toolbar.AddButton("Cancel", "CANCEL");
        MenuBatchFilterMain.AccessRights = this.ViewState;
        MenuBatchFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
		{
			txtFromDate.Focus();
		}

	}

	protected void BatchFilterMain_TabStripCommand(object sender, EventArgs e)
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

			criteria.Add("txtFromDate", txtFromDate.Text);
			criteria.Add("txtToDate", txtToDate.Text);
			criteria.Add("txtBatch", txtBatch.Text);
			criteria.Add("ucStatus", ucStatus.SelectedHard);

			Filter.CurrentBatchFilter = criteria;
		}

		ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo');", true);
	}

	
}
