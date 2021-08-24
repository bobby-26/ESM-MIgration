using System;

using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;


public partial class CrewPlanEventAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("ADD", "Add", ToolBarDirection.Right);

        CrewPlanTabs.AccessRights = this.ViewState;
        CrewPlanTabs.MenuList = toolbargrid.Show();

        ViewState["VESSELID"] = "";

        if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
            ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
        {
            ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();         
            ddlVessel.Enabled = false;
        }
        else
        {
            if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                ddlVessel.SelectedVessel = ViewState["VESSELID"].ToString();
        }

    }

    protected void CrewPlanTabs_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("ADD"))
            {

                PhoenixCrewChangeEvent.InsertCrewPlanEvent(
                            General.GetNullableInteger(ddlVessel.SelectedVessel)
                            , General.GetNullableDateTime(txtEventDate.Text)
                             , General.GetNullableDateTime(txtEventToDate.Text)
                            , General.GetNullableInteger(ucport.SelectedValue)
                            , General.GetNullableInteger(ucAddrPortAgent.SelectedValue)
                            , General.GetNullableDateTime(txtVesselArrival.Text)
                            , General.GetNullableDateTime(txtVesselDepature.Text)
                            , txtRemarks.Text.Trim()
                            );

                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected bool IsValidEvent(string vessel, string eventdate, string eventtodate, string arrival, string depart)
    {
        if (vessel.Trim() != "" && vessel != null && eventdate != null && eventtodate != null  && arrival != null && depart != null)
            return true;
        else
            return false;
    }
}