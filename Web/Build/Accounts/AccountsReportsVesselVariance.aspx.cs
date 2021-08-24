using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class AccountsReportsVesselVariance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT");
            MenuVesselVariance.AccessRights = this.ViewState;
            MenuVesselVariance.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
            }
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=VESSELVARIANCE&vessel=null&type=Accumulated&AsOnDate=null&showmenu=0";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVesselVariance_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidData(ddlVessel.SelectedVessel,ddlType.SelectedValue,ucAsOnDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=VESSELVARIANCE&vessel="+ddlVessel.SelectedVessel+"&type="+ddlType.SelectedValue+"&AsOnDate="+ucAsOnDate.Text+"&showmenu=0";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidData(string vessel,string type, string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessel.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Vessel is required.";

        if (type.Trim().Equals("0"))
            ucError.ErrorMessage = "Type is required.";

        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "As on date is required.";

        return (!ucError.IsError);
    }
}
