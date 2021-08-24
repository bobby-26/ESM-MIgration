using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class AccountsReportsVariance : PhoenixBasePage
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
                ViewState["PAGEURL"] = "../Reports/ReportsViewWithSubReport.aspx";
                ddlMonth.DataBind();
                ddlMonth.Items.Insert(0, "--Select--");
            }
            //ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=VARIANCEREPORT&vessel=null&month=null&year=null&budgetgroup=null&budgetyn=null&showmenu=0";
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
                if (!IsValidData(ddlVessel.SelectedVessel, ddlMonth.SelectedValue, ucFinancialYear.SelectedQuick, ddlType.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=VARIANCEREPORT&vessel=" + ddlVessel.SelectedVessel + 
                        "&month=" + ddlMonth.SelectedValue + "&year=" + ucFinancialYear.SelectedQuick +"&budgetgroup=" +ddlType.SelectedValue +"&budgetyn="+ (chkBudget.Checked?1:0).ToString()+ "&showmenu=0";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidData(string vessel, string month, string year, string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessel.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Vessel is required.";

        if (type.Trim().Equals("0"))
            ucError.ErrorMessage = "Type is required.";

        if (month.Trim().Equals("--Select--"))
            ucError.ErrorMessage = "Month is required.";

        if (year.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Year is required.";

        return (!ucError.IsError);
    }
}
