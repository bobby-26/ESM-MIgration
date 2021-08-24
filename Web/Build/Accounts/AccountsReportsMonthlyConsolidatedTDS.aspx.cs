using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsReportsMonthlyConsolidatedTDS : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            Menuledger.AccessRights = this.ViewState;
            Menuledger.MenuList = toolbargrid.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
            }
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=MONTHLYCONSOLIDATEDTDS&month=null&year=null&showmenu=0";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Menuledger_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidData(ucFinancialYear.SelectedQuick, ddlMonth.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=MONTHLYCONSOLIDATEDTDS&month=" + ddlMonth.SelectedValue + "&year=" + ucFinancialYear.SelectedQuick + "&showmenu=0";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidData(string year, string month)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (year.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Year is required.";

        if (year.Trim().Equals(""))
            ucError.ErrorMessage = "Year is required.";

        if (month.Trim().Equals(""))
            ucError.ErrorMessage = "Month is required.";

        return (!ucError.IsError);
    }
}

