using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class VesselAccountsReimbursementFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuPD.AccessRights = this.ViewState;
        MenuPD.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

        }
    }

    protected void PD_TabStripCommand(object sender, EventArgs e)
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

            criteria.Add("ddlRank", ddlRank.SelectedRank);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("txtFileNo", txtFileNo.Text);
            criteria.Add("chkActive", ddlStatus.SelectedValue);
            criteria.Add("ddlApproved", ddlApproved.SelectedValue);
            criteria.Add("ddlEarDed", ddlEarDed.SelectedValue);
            criteria.Add("ddlPurpose", ddlPurpose.SelectedHard);
            Filter.CrewReimbursementFilterSelection = criteria;
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    protected void ddlEarDed_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (General.GetNullableInteger(ddlEarDed.SelectedValue).HasValue && General.GetNullableInteger(ddlEarDed.SelectedValue).Value < 0)
            {

                //ddlPurpose.HardList = PhoenixRegistersHard.ListHard(1, 129, 0, "CRR,ADL,TFR");
                ddlPurpose.HardList = PhoenixRegistersReimbursementRecovery.ListReimbursementRecovery(1, 0, 1, null);
            }
            else
            {
                ddlPurpose.HardList = PhoenixRegistersReimbursementRecovery.ListReimbursementRecovery(0, 0, 1, null);
            }

        }
        catch
        {
        }
    }
}
