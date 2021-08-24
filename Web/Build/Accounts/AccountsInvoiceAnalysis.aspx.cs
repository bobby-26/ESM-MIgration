using System;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class Accounts_AccountsInvoiceAnalysis : PhoenixBasePage
{
    public int iPendingcount = 0;
    public int iReceivedcount = 0;
    public int iClearedcount = 0;
    public int iReClearedcount = 0;
    public int iActualClearedcount = 0;
    public int iReOpenedcount = 0;
    public int iTotalcount = 0;
    public int iTotalCancelled = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Accounts/AccountsInvoiceAnalysis.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvFormDetails')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Accounts/AccountsInvoiceAnalysis.aspx", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuOrderForm.AccessRights = this.ViewState;
        MenuOrderForm.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 59);
            DataTable dt = ds.Tables[0];
            chkInvoiceTypeList.DataSource = dt;
            chkInvoiceTypeList.DataBind();
            ViewState["SelectedFleetList"] = "";
            BindVesselPurchaseSuptList();
        }
    }

    private void BindVesselPurchaseSuptList()
    {
        DataSet ds = PhoenixRegistersVessel.ListVesselPurchaseSupdt();
        ddlSuptList.DataSource = ds;
        ddlSuptList.DataBind();
        ddlSuptList.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
    }

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    Rebind();
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

    protected void Rebind()
    {
        gvFormDetails.SelectedIndexes.Clear();
        gvFormDetails.EditIndexes.Clear();
        gvFormDetails.DataSource = null;
        gvFormDetails.Rebind();
    }

    protected void gvFormDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        string strInvoiceTypeList = "";

        foreach (ButtonListItem item in chkInvoiceTypeList.Items)
        {
            if (item.Selected)
            {
                strInvoiceTypeList = strInvoiceTypeList + item.Value + ",";
            }
        }

        string[] alColumns = { "FLDUSERNAME", "FLDPENDINGCOUNT", "FLDRECEIVED", "FLDCLEARED", "FLDREOPENED", "FLDRECLEARED", "FLDCANCELLED", "FLDFINALTOTAL", "FLDACTUALCLEARED" };
        string[] alCaptions = { "User Name", "Inital Pending", "Received", "Cleared", "Re Opened", "Re Cleared", "Cancelled", "Total Pending", "Actual Cleared" };

        DataSet ds = new DataSet();

        ds = PhoenixAccountsInvoice.InvoiceAnalysisReport(General.GetNullableDateTime(ucFromDate.Text), General.GetNullableDateTime(ucToDate.Text), null, General.GetNullableString(ViewState["SelectedFleetList"].ToString()), General.GetNullableInteger(ddlSuptList.SelectedValue), strInvoiceTypeList, ref iPendingcount, ref iReceivedcount, ref iClearedcount, ref iTotalcount, ref iReClearedcount, ref iActualClearedcount, ref iReOpenedcount, ref iTotalCancelled);

        gvFormDetails.DataSource = ds;

        General.SetPrintOptions("gvFormDetails", "Invoice Analysis", alCaptions, alColumns, ds);
    }

    public void ShowExcel()
    {
        string strInvoiceTypeList = "";

        foreach (ButtonListItem item in chkInvoiceTypeList.Items)
        {
            if (item.Selected)
            {
                strInvoiceTypeList = strInvoiceTypeList + item.Value + ",";
            }
        }
        string[] alColumns = { "FLDUSERNAME", "FLDPENDINGCOUNT", "FLDRECEIVED", "FLDCLEARED", "FLDREOPENED", "FLDRECLEARED", "FLDCANCELLED", "FLDFINALTOTAL", "FLDACTUALCLEARED" };
        string[] alCaptions = { "User Name", "Inital Pending", "Received", "Cleared", "Re Opened", "Re Cleared", "Cancelled", "Total Pending", "Actual Cleared" };

        DataSet ds = new DataSet();
        ds = PhoenixAccountsInvoice.InvoiceAnalysisReport(General.GetNullableDateTime(ucFromDate.Text), General.GetNullableDateTime(ucToDate.Text), null, General.GetNullableString(ViewState["SelectedFleetList"].ToString()), int.Parse(ddlSuptList.SelectedValue), strInvoiceTypeList, ref iPendingcount, ref iReceivedcount, ref iClearedcount, ref iTotalcount, ref iReClearedcount, ref iActualClearedcount, ref iReOpenedcount, ref iTotalCancelled);

        Response.AddHeader("Content-Disposition", "attachment; filename=InvoiceAnalysis.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<h3><center>Invoice Analysis</center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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

        if (ddlSuptList.SelectedValue == "Dummy")
            ucError.ErrorMessage = "Select a superintendent";

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

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                NameValueCollection criteria = new NameValueCollection();
                string lblPurchaserId = ((RadLabel)e.Item.FindControl("lblPurchaserId")).Text;
                criteria.Clear();
                criteria.Add("ddlPurchaserList", lblPurchaserId);
                criteria.Add("invoiceStatusList", ",241,242,632,");
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
}
