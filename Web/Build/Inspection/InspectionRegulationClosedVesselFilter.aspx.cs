using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulationClosedVesselFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        gvTabStrip.AccessRights = this.ViewState;
        gvTabStrip.MenuList = toolbar.Show();
        if (IsPostBack == false)
        {
            chkComplianceStatusAdd.SelectedIndex = 0;
        }
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GO"))
            {
                string Script = "";
                Script += "<script language='JavaScript' id='BookMarkScript'>";
                Script += "fnReloadList();";
                Script += "</script>";
                var criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("chkComplianceStatusAdd", chkComplianceStatusAdd.SelectedValue);
                Filter.CurrentRegulationClosedVesselFilterCriteria = criteria;
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ClearUserInput();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearUserInput()
    {
        chkComplianceStatusAdd.SelectedIndex = 0;
    }
}