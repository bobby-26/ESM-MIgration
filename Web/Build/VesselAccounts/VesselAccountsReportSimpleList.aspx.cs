using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class VesselAccountsSimpleCrewList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "Display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
                ucVessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=7&reportcode=SIMPLECREWLIST&showexcel=no&showword=no&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID + "&showmenu=0";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucVessel.SelectedVessel.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=7&reportcode=SIMPLECREWLIST&vesselid=" + ucVessel.SelectedVessel.ToString() + "&showmenu=0";
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter(string vessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessel.Equals("") || vessel.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Select Vessel";
        }

        return (!ucError.IsError);
    }
}
