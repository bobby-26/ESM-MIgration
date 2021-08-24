using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Text;
using Telerik.Web.UI;

public partial class AccountsInvoiceAnalysisForAccounts : PhoenixBasePage
{
    public int iInitialPendingcount = 0;
    public int iReceivedcount = 0;
    public int iTotalInvoicecount = 0;
    public int iReOpenedcount = 0;
    public int iReClearedcount = 0;
    public int iCurrentPendingCount = 0;
    public int iClearedcount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceAnalysisForAccounts.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvFormDetails')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsInvoiceAnalysisForAccounts.aspx", "Search", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
         //   MenuOrderForm.SetTrigger(pnlOrderForm);

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;

                    BindData();
                }
            }
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

    public void BindData()
    {
        string[] alColumns = { "FLDUSERNAME", "FLDINITIALPENDING", "FLDRECEIVED", "FLDTOTALINVOICE", "FLDINVOICEGIVENBACK", "FLDINVOICERECEIVEDBACK", "FLDCURRENTPENDING", "FLDCLEARED" };
        string[] alCaptions = { "User Name", "Inital Pending", "Received", "Total Invoice", "Invoices given back to tech/crew for rework", "Invoices received from tech/crew after rework", "Pending", "Cleared from accounts checking" };

        DataSet ds = new DataSet();

        ds = PhoenixAccountsInvoice.InvoiceAnalysisReportForAccounts(General.GetNullableDateTime(ucFromDate.Text), General.GetNullableDateTime(ucToDate.Text), General.GetNullableInteger(ddlInvoiceType.SelectedHard), ref iInitialPendingcount, ref iReceivedcount, ref iTotalInvoicecount, ref iReOpenedcount, ref iReClearedcount, ref iCurrentPendingCount, ref iClearedcount);
        

        General.SetPrintOptions("gvFormDetails", "Invoice Analysis", alCaptions, alColumns, ds);

        gvFormDetails.DataSource = ds;

    }

    public void ShowExcel()
    {
        string[] alColumns = { "FLDUSERNAME", "FLDINITIALPENDING", "FLDRECEIVED", "FLDTOTALINVOICE", "FLDINVOICEGIVENBACK", "FLDINVOICERECEIVEDBACK", "FLDCURRENTPENDING", "FLDCLEARED" };
        string[] alCaptions = { "User Name", "Inital Pending", "Received", "Total Invoice", "Invoices given back to tech/crew for rework", "Invoices received from tech/crew after rework", "Pending", "Cleared from accounts checking" };

        DataSet ds = new DataSet();
        ds = PhoenixAccountsInvoice.InvoiceAnalysisReportForAccounts(General.GetNullableDateTime(ucFromDate.Text), General.GetNullableDateTime(ucToDate.Text), General.GetNullableInteger(ddlInvoiceType.SelectedHard), ref iInitialPendingcount, ref iReceivedcount, ref iTotalInvoicecount, ref iReOpenedcount, ref iReClearedcount, ref iCurrentPendingCount, ref iClearedcount);

        Response.AddHeader("Content-Disposition", "attachment; filename=InvoiceAnalysisForAccounts.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<h3><center>Invoice Analysis For Accounts</center></h3></td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

    }

   
    public bool IsValidFilter()
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (ucFromDate.Text == "")
            ucError.ErrorMessage = "Enter from date";

        if (ucToDate.Text == "")
            ucError.ErrorMessage = "Enter to date";

        return (!ucError.IsError);

    }

    

    protected void gvFormDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

           
           else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                NameValueCollection criteria = new NameValueCollection();

                string lblAccountUserId = ((RadLabel)e.Item.FindControl("lblAccountUserId")).Text;
                criteria.Clear();
                criteria.Add("ucPIC", lblAccountUserId);
                criteria.Add("invoiceStatusList", ",243,");

                Filter.CurrentInvoiceSelection = criteria;

                Response.Redirect("../Accounts/AccountsInvoiceMaster.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    
    protected void gvFormDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFormDetails.CurrentPageIndex + 1;
        BindData();
    }
}
