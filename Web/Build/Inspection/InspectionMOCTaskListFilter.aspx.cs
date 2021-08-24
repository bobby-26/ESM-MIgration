using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class InspectionMOCTaskListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
            MenuMOCTaskFilterMain.AccessRights = this.ViewState;
            MenuMOCTaskFilterMain.MenuList = toolbar.Show();
            MenuMOCTaskFilterMain.Title = "MOC Task Filter";
            ucVessel.Enabled = true;
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
            }
        }
    }

    protected void MenuMOCTaskFilterMain_TabStripCommand(object sender, EventArgs e)
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
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ddlStatus", ddlStatus.SelectedHard);
            criteria.Add("txtFrom", txtFrom.Text);
            criteria.Add("txtTo", txtTo.Text);
            criteria.Add("txtDoneDateFrom", txtDoneDateFrom.Text);
            criteria.Add("txtDoneDateTo", txtDoneDateTo.Text);
            criteria.Add("txtMOCRefNo", txtMOCRefNo.Text);
            Filter.CurrentMOCTaskFilter = criteria;
            Filter.CurrentSelectedMOCTask = null;
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
