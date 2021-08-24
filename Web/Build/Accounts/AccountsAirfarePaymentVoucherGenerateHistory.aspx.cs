using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class AccountsAirfarePaymentVoucherGenerateHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAirfarePaymentVoucherGenerateHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvInvoiceHistory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuHistory.AccessRights = this.ViewState;
            MenuHistory.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["AGENTINVOICEID"] = "";
                if (Request.QueryString["AGENTINVOICEID"] != null)
                    ViewState["AGENTINVOICEID"] = Request.QueryString["AGENTINVOICEID"].ToString();

            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("History", "HISTORY");

            MenuHistoryMain.AccessRights = this.ViewState;
            MenuHistoryMain.MenuList = toolbarmain.Show();
            MenuHistoryMain.SelectedMenuIndex = 0;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuHistoryMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuHistory_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alCaptions = { "Date/Time of Change", "Type of Change", "User Name", "Field", "Old Value", "New Value", "Procedure Used" };

        string[] alColumns = { "FLDUPDATEDATE", "FLDTYPENAME", "FLDUSERNAME", "FLDFIELD", "FLDPREVIOUSVALUE", "FLDCURRENTVALUE", "FLDPROCEDURENAME" };

        string type = rblHistoryType.SelectedItem.Value;

        ds = PhoenixAccountsAirfarePaymentVoucherGenerate.TravelAgentPVHistoryList(new Guid(ViewState["AGENTINVOICEID"].ToString()), type);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Travel Invoice History- " + ViewState["Invoicenumber"], ds.Tables[0], alColumns, alCaptions, null, null);

    }

    private void BindData()
    {
        string type = "";
        DataSet ds = new DataSet();

        string[] alCaptions = { "Date/Time of Change", "Type of Change", "User Name", "Field", "Old Value", "New Value", "Procedure Used" };

        string[] alColumns = { "FLDUPDATEDATE", "FLDTYPENAME", "FLDUSERNAME", "FLDFIELD", "FLDPREVIOUSVALUE", "FLDCURRENTVALUE", "FLDPROCEDURENAME" };

        type = rblHistoryType.SelectedItem.Value;

        ds = PhoenixAccountsAirfarePaymentVoucherGenerate.TravelAgentPVHistoryList(new Guid(ViewState["AGENTINVOICEID"].ToString()), type);

        General.SetPrintOptions("gvInvoiceHistory", "Travel Invoice History", alCaptions, alColumns, ds);

        gvInvoiceHistory.DataSource = ds;

    }

    protected void ReBindData(object sender, EventArgs e)
    {
        BindData();
        gvInvoiceHistory.Rebind();
    }

    protected void gvInvoiceHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvInvoiceHistory_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvInvoiceHistory_ItemDataBound1(object sender, GridItemEventArgs e)
    {

    }

}
