using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsReportsConsolidatedVendorAging : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            Menuledger.AccessRights = this.ViewState;
            Menuledger.MenuList = toolbargrid.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("General", "GENERAL");
            MenuSubsidiaryLedger.AccessRights = this.ViewState;
            MenuSubsidiaryLedger.MenuList = toolbar.Show();
            MenuSubsidiaryLedger.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
            }
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=CONSOLIDATEDVENDORAGING&AsOnDate=null&showmenu=0";
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
                if (!IsValidData(ucAsOnDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=CONSOLIDATEDVENDORAGING&AsOnDate=" + ucAsOnDate.Text + "&showmenu=0";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidData(string asOnDate)
    {
        DateTime resultdate;

        if (string.IsNullOrEmpty(asOnDate))
        {
            ucError.ErrorMessage = "As on date is required";
        }
        else if (DateTime.TryParse(asOnDate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "As on date should be earlier than current date";
        }

        ucError.HeaderMessage = "Please provide the following required information";
        return (!ucError.IsError);
    }
}
