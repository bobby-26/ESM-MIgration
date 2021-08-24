using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CommonBudgetVesselRemainingBudget : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
                ucVessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
                      General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), 1);
                    ddlAccountDetails.DataBind();
                }
            }
            
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
                if (!IsValidFilter(ucVessel.SelectedVessel.ToString(),ddlYearlist.SelectedQuick.ToString(),ddlAccountDetails.SelectedValue.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=8&reportcode=VESSELBUDGET&year=" + ddlYearlist.SelectedQuick + "&vesselid=" + ucVessel.SelectedVessel + "&accountid=" + ddlAccountDetails.SelectedValue + "&showmenu=0";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter(string vessel, string year,string accountid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //GridView _gridview = gvCrew;

        if (vessel.Equals("Dummy") || vessel.Equals(""))
        {
            ucError.ErrorMessage = "Vessel is required.";
        }
        if (year.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Year is required.";
        }
        if (accountid.Equals(""))
        {
            ucError.ErrorMessage = "Vessel Account is required.";
        }
        return (!ucError.IsError);

    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        BindVesselAccount(General.GetNullableInteger(ucVessel.SelectedVessel) == null ? 0 : General.GetNullableInteger(ucVessel.SelectedVessel));
    }
    protected void BindVesselAccount(int? vesselid)
    {
        ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
            vesselid, 1);
        ddlAccountDetails.DataBind();
        if (ddlAccountDetails.Items.Count == 2)
            ddlAccountDetails.SelectedIndex = 1;
    }
    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new DropDownListItem("--Select--", ""));
    }

}
