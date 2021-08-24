using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        CrewListFilterMain.AccessRights = this.ViewState;
        CrewListFilterMain.MenuList = toolbar.Show();

    }

    protected void CrewListFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string Script = "";
        Script += "<script language='JavaScript' id='BookMarkScript1'>";
        Script += "fnReloadList();";
        Script += "</script>";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            if (!IsValidCrewList())
            {
                ucError.Visible = true;
                return;
            }
            criteria.Clear();
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("txtOnDate", txtOnDate.Text);
            criteria.Add("chkSailOnly", chkSailOnly.Checked == true ? "1" : null);
            criteria.Add("lstRank", lstRank.SelectedRankValue);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("lstNationality", lstNationality.SelectedNationalityValue);
            criteria.Add("rblExtraCrew", rblExtraCrew.SelectedValue);
            criteria.Add("chkRankExp", chkRankExp.Checked == true ? "1" : null);

            Filter.CurrentCrewListSelection = criteria;
        }
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        
    }
    public bool IsValidCrewList()
    {
        int result;
        if (int.TryParse(ddlVessel.SelectedVessel, out result) == false)
            ucError.ErrorMessage = "Please select a vessel";

        return (!ucError.IsError);
    }

}
