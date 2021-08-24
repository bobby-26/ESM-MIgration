using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulationComplianceStatusFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        toolbar.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        ComplianceStatus.AccessRights = this.ViewState;
        ComplianceStatus.MenuList = toolbar.Show();
        if (IsPostBack == false)
        {
            chkComplianceStatusAdd.SelectedIndex = 1;
        }

    }

    protected void ComplianceStatus_TabStripCommand(object sender, EventArgs e)
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
                Filter.CurrentRegulationComplianceStatusFilterCriteria = criteria;
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