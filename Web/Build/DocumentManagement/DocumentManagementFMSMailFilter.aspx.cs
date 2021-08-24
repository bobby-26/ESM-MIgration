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
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;

public partial class DocumentManagementFMSMailFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO", ToolBarDirection.Right);

            MenuOfficeFilterMain.AccessRights = this.ViewState;
            MenuOfficeFilterMain.MenuList = toolbar.Show();
            txtFileNumber.Focus();
        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language='JavaScript' id='BookMarkScript'>";
        Script += "fnReloadList();";
        Script += "</script>";

        if (CommandName.ToUpper().Equals("GO"))
        {
            ViewState["PAGENUMBER"] = 1;
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();

            criteria.Add("txtFileNumber", txtFileNumber.Text);
            criteria.Add("txtvesselid", ddlVessel.SelectedVessel);
            criteria.Add("txtFromDate", txtFromDate.Text);
            criteria.Add("txtToDate", txtToDate.Text);
            criteria.Add("txtSender", txtSender.Text);
            criteria.Add("txtRecipient", txtRecipient.Text);
            criteria.Add("txtSubject", txtSubject.Text);

            Filter.CurrentFMSMAILFilterSelection = criteria;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
    }
}
