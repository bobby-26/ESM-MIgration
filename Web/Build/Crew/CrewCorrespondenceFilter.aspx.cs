using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewCorrespondenceFilter : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        MenuCorrespondenceFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
		{		
			txtSubject.Focus();
		}
	}

	protected void CorrespondenceFilterMain_TabStripCommand(object sender, EventArgs e)
	{
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language='JavaScript' id='BookMarkScript'>";
        Script += "fnReloadList('CI','ifMoreInfo');";
        Script += "</script>";


        if (CommandName.ToUpper().Equals("GO"))
		{
			ViewState["PAGENUMBER"] = 1;
			NameValueCollection criteria = new NameValueCollection();
			criteria.Clear();
			criteria.Add("txtSubject", txtSubject.Text);
			criteria.Add("ddlCorrespondenceType", ddlCorrespondenceType.SelectedQuick);
			Filter.CurrentCorrespondenceFilter = criteria;
		}
		Session["corres"] = "0";//to differentiate between lock and correspondence filter

        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
    }
}
